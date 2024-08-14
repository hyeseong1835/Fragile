using DG.DemiEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomObjectRule))]
public class RoomObjectRuleInspector : UnityEditor.Editor
{
    public class ComponentInfo
    {
        public Component component;
        public Type type;
        public PropertyInfo[] fields;
        public UnityEditor.Editor editor;

        public ComponentInfo(Component component)
        {
            this.component = component;
            type = component.GetType();
            editor = CreateEditor(component);

            // Examining the name of all variables in a C# object
            // In this case, we'll list the variable in this NameToBlorp
            // class
            Debug.Log("*****" + type.Name + "******************");
            List<PropertyInfo> result = new List<PropertyInfo>();
            {
                foreach (PropertyInfo field in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty))
                {
                    Debug.Log("=Private=" + field.Name + "=============================");
                    foreach (CustomAttributeData attribute in field.CustomAttributes)
                    {
                        if (attribute.AttributeType == typeof(ObsoleteAttribute)) break;

                        Debug.Log(attribute.AttributeType.Name);
                        if (attribute.AttributeType == typeof(NonSerializedAttribute))
                        {
                            result.Add(field);
                            break;
                        }
                    }
                }
                foreach (PropertyInfo field in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty))
                {
                    Debug.Log("=Public="+field.Name+"=============================");
                    foreach (CustomAttributeData attribute in field.CustomAttributes)
                    {
                        if (attribute.AttributeType == typeof(ObsoleteAttribute)) break;

                        Debug.Log(attribute.AttributeType.Name);
                        if (attribute.AttributeType == typeof(SerializeAttribute))
                        {
                            result.Add(field);
                            break;
                        }
                    }
                }
            }
            fields = result.ToArray();
        }
    }
    ComponentInfo[] infos;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RoomObjectRule rule = (RoomObjectRule)target;

        if (GUI.Button(GUILayoutUtility.GetRect(0, 30, GUILayout.ExpandHeight(false)), "Refresh"))
        {
            if (rule.prefab != null)
            {
                Component[] components = rule.prefab.GetComponents(typeof(Component));
                infos = new ComponentInfo[components.Length];
                for (int i = 0; i < components.Length; i++)
                {
                    infos[i] = new ComponentInfo(components[i]);
                }
            }
        }

        if (infos != null)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                ComponentInfo info = infos[i];
                CustomGUILayout.TitleHeaderLabel(info.type.Name);
                foreach (PropertyInfo field in info.fields)
                {
                    EditorGUILayout.LabelField(field.Name);
                }
                //info.editor.DrawHeader();
                //info.editor.OnInspectorGUI();
            }
        }
    }
}
