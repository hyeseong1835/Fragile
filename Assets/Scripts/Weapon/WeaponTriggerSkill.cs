using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public abstract class WeaponTriggerSkill : WeaponSkill
{
    [SerializeReference] public WeaponBehavior[] executeBehavior = new WeaponBehavior[0];

#if UNITY_EDITOR
    public static WeaponTriggerSkill CreateDefault() => new Skill_Swing();

    public override void OnGUI(SerializedProperty property)
    {
        EditorGUILayout.LabelField("-Áï½Ã");
        SerializedProperty executeBehaviorArrayProperty
            = property.FindPropertyRelative(nameof(executeBehavior));
        CustomGUILayout.BeginTab();
        {
            CustomGUILayout.ArrayField(
                ref executeBehavior, 
                i => {
                    if (executeBehavior == null)
                    {
                        Debug.LogError($"{i}: BehaviorArray is Null");
                        return true;
                    }
                    if (executeBehaviorArrayProperty == null)
                    {
                        Debug.LogError($"{i}: Serialized Property is Null");
                        return true;
                    }

                    if (executeBehavior[i] == null)
                    {
                        EditorGUILayout.LabelField($"{i}: Behavior is null");
                        return false;
                    }
                    executeBehavior[i].OnGUI(executeBehaviorArrayProperty.GetArrayElementAtIndex(i));
                    return false;
                }, 
                WeaponBehavior.GetDefault
            );
        }
        CustomGUILayout.EndTab();}
#endif
}
