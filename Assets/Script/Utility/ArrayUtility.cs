using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EnumArrayAttribute : PropertyAttribute
{
    public Type enumType { get; private set; }

    public EnumArrayAttribute(Type enumType)
    {
        this.enumType = enumType;
    }
}
[CustomPropertyDrawer(typeof(EnumArrayAttribute))]
public class EnumArrayPropertyDrawer : PropertyDrawer
{
    string[] column;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (column == null) return Editor.titleHeight;
        else return column.Length * Editor.propertyHeight + Editor.titleHeight;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumArrayAttribute enumArrayAttribute = attribute as EnumArrayAttribute;
        
        string[] enumNames = Enum.GetNames(enumArrayAttribute.enumType);
        Array enumValues = Enum.GetValues(enumArrayAttribute.enumType);
        if (column == null) column = enumNames;

        Rect rect = new Rect(position.position, new Vector2(position.size.x, Editor.propertyHeight * column.Length + Editor.titleHeight));

        //Property
        EditorGUI.BeginProperty(rect, GUIContent.none, property.serializedObject.FindProperty(property.name));
        {
            EditorGUI.LabelField(new Rect(rect.position, new Vector2(rect.size.x - Editor.shortButtonWidth, Editor.titleHeight)), label);

            if (GUI.Button(new Rect(rect.position + new Vector2(rect.size.x - Editor.shortButtonWidth, 10), new Vector2(Editor.shortButtonWidth, Editor.propertyHeight)), "Remap"))
            {
                ArrayRemap();
            }
            /*
            Rect fieldRect = new Rect(rect.position.x, rect.position.y + Editor.titleHeight,
                                      rect.size.x, Editor.propertyHeight * property.arraySize);
            for (int arrayIndex = 0; arrayIndex < column.Length; arrayIndex++)
            { 
                //라벨
                {
                    Rect labelArea = new Rect(
                        fieldRect.position.x, fieldRect.position.y + Editor.propertyHeight * arrayIndex,
                        Editor.propertyLabelWidth, Editor.propertyHeight
                    );
                    
                    GUI.Label(labelArea, column[arrayIndex]);
                }
                //필드
                {
                    Rect interactionArea = new Rect(
                        fieldRect.position.x + Editor.propertyLabelWidth, fieldRect.position.y + Editor.propertyHeight * arrayIndex,
                        fieldRect.size.x - Editor.propertyLabelWidth, Editor.propertyHeight
                    );
                    object elementValue = property.GetArrayElementAtIndex(arrayIndex);

                    EditorGUI.EnumPopup(interactionArea, (Enum)enumValues.GetValue(property.enumValueIndex));
                }
            }
        */
        }
        EditorGUI.EndProperty();


        void ArrayRemap()
        {
            List<string> columnList = column.ToList();
            IEnumerator columnListReadIt = columnList.GetEnumerator();

            for (int columnListReadIndex = 0; ;)
            {
                if (columnListReadIndex >= enumNames.Length)
                {
                    columnList.RemoveRange(columnListReadIndex, columnList.Count - enumNames.Length);
                    break;
                }
                if (columnListReadIndex >= columnList.Count)
                {
                    for (int i = columnList.Count; i < enumNames.Length; i++)
                    {
                        columnList.Add(enumNames[i]);
                    }
                    break;
                }

                IEnumerator columnListIt = columnList.GetEnumerator();
                string currentColumn = (string)columnListIt.Current;
                
                if ((string)columnListIt.Current != enumNames[columnListReadIndex])
                {
                    for (int compareIndex = columnListReadIndex; compareIndex < enumNames.Length; compareIndex++)
                    {
                        currentColumn = (string)columnListIt.Current;

                        if (enumNames[compareIndex] == currentColumn)
                        {
                            columnList.MoveElement(currentColumn, columnListReadIndex);

                            property.MoveArrayElement(compareIndex, columnListReadIndex);

                            columnListReadIndex++;
                            break;
                        }
                    }
                    columnList.Remove((string)columnListReadIt.Current);
                    property.DeleteArrayElementAtIndex(columnListReadIndex);
                }
                else columnListReadIndex++;
            }
            column = columnList.ToArray();

        }
    }
}