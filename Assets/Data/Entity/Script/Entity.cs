using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Rendering;
using System.Linq;
using System.Collections.Generic;

public enum EntityLayer
{
    Player, Enemy, Obstacle
}
public enum EntityLayerInteraction
{
    None, Friend, Hostile, Neutral
}
[Serializable] public struct EntityLayerInteractionArray { public EntityLayerInteraction[] array; }
[CustomPropertyDrawer(typeof(EntityLayerInteractionArray))]
public class EntityLayerPropertyDrawer : PropertyDrawer
{
    string[] column;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty enumArrayProperty = property.FindPropertyRelative("array");
        string[] layerNames = Enum.GetNames(typeof(EntityLayer));
        if (column == null) column = layerNames;
        Rect rect = new Rect(position.position, new Vector2(position.size.x, Editor.propertyHeight * column.Length + Editor.titleHeight));
        
        //Property
        EditorGUI.BeginProperty(rect, GUIContent.none, property);
        {
            EditorGUI.LabelField(new Rect(rect.position, new Vector2(rect.size.x - Editor.shortButtonWidth, Editor.titleHeight)), label);


            if (GUI.Button(new Rect(rect.position + new Vector2(rect.size.x - Editor.shortButtonWidth, 10), new Vector2(Editor.shortButtonWidth, Editor.propertyHeight)), "Reset"))
            {
                ArrayRemap();
            }

            Enum[] interactionValues = Enum.GetValues(typeof(EntityLayerInteraction)).Cast<Enum>().ToArray();

            Rect fieldRect = new Rect(rect.position.x, rect.position.y + Editor.titleHeight,
                                      rect.size.x, Editor.propertyHeight * enumArrayProperty.arraySize);
            for (int arrayIndex = 0; arrayIndex < column.Length; arrayIndex++)
            {
                Rect labelArea = new Rect(
                    fieldRect.position.x, fieldRect.position.y + Editor.propertyHeight * arrayIndex,
                    Editor.propertyLabelWidth, Editor.propertyHeight
                );
                Rect interactionArea = new Rect(
                    fieldRect.position.x + Editor.propertyLabelWidth, fieldRect.position.y + Editor.propertyHeight * arrayIndex,
                    fieldRect.size.x - Editor.propertyLabelWidth, Editor.propertyHeight
                );
                GUI.Label(labelArea, column[arrayIndex]);

                SerializedProperty arrayElement = enumArrayProperty.GetArrayElementAtIndex(arrayIndex);
                Enum value = interactionValues[enumArrayProperty.GetArrayElementAtIndex(arrayIndex).enumValueIndex];
                arrayElement.SetEnumValue(EditorGUI.EnumPopup(interactionArea, value));
            }
        } EditorGUI.EndProperty();


        void ArrayRemap()
        {
            List<string> columnList = column.ToList();
            for(int readIndex = 0; ;)
            {
                if (readIndex >= layerNames.Length)
                {
                    columnList.RemoveRange(readIndex, columnList.Count - layerNames.Length);
                    break;
                }
                if (readIndex >= columnList.Count)
                {
                    for(int i = columnList.Count; i < layerNames.Length; i++ )
                    {
                        columnList.Add(layerNames[i]);
                    }
                    break;
                }
                
                if(columnList[readIndex] != layerNames[readIndex])
                {
                    for (int compareIndex = readIndex; compareIndex < layerNames.Length; compareIndex++)
                    {
                        if (columnList[readIndex] == layerNames[compareIndex])
                        {
                            columnList.Insert(readIndex, layerNames[compareIndex]);
                            columnList.RemoveAt(compareIndex + 1);

                            enumArrayProperty.MoveArrayElement(compareIndex, readIndex);

                            readIndex++;
                            break;
                        }
                    }
                    columnList.RemoveAt(readIndex);
                    enumArrayProperty.DeleteArrayElementAtIndex(readIndex);
                }
                else readIndex++;
            }
            column = columnList.ToArray();
            enumArrayProperty.arraySize = columnList.Count;
        }
    }
}
public abstract class Entity : MonoBehaviour 
{
    public abstract EntityData EntityData { get; set; }
    public abstract Type DataType { get; }


    [FoldoutGroup("Stat")]
    #region Foldout Stat - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|                                         

        [HorizontalGroup("Stat/HP")]
        #region Horizontal HP

            [LabelWidth(Editor.propertyLabelWidth)]
            #if UNITY_EDITOR
            [ProgressBar(0, nameof(_maxHp), ColorGetter = nameof(_hpColor))]
            #endif
            public float hp = 1;

            #if UNITY_EDITOR
                                                                                 [HorizontalGroup("Stat/HP", Width = Editor.shortNoLabelPropertyWidth)]
            [ShowInInspector][HideLabel]
            [DelayedProperty]
            float _maxHp{
                get { 
                    if(EntityData == null) return default;
                    return EntityData.maxHp; 
                }
                set {
                    if (hp == EntityData.maxHp || hp > value) hp = value;
            EntityData.maxHp = value;
                }
            }

//          HideInInspector_____________________________________________________|
            Color _hpColor {
                get {
                    if(EntityData == null) return default;
                    
                    Gradient gradient = new Gradient();
                    gradient.SetKeys(
                        new GradientColorKey[] {
                            new GradientColorKey(Color.yellow, 0),
                            new GradientColorKey(Color.red, 1)
                        },
                        new GradientAlphaKey[] { new GradientAlphaKey(1, 0) }//-|
                    );
                    return gradient.Evaluate(hp / EntityData.maxHp);
                }
            }

            #endif

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|    


    #endregion - - - - - - - - - - - - - - - - - - - - -|

    public EntityLayer entityLayer;
    public EntityLayerInteractionArray entityLayerType;
    [PropertySpace(EntityLayerInteractionArrayHeight)]
    [ShowInInspector]
    public const float EntityLayerInteractionArrayHeight = 3 * Editor.propertyHeight + Editor.titleHeight - Editor.propertyHeight;

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0) Die();
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}