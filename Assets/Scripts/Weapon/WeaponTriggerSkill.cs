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
        EditorGUILayout.LabelField("-즉시");
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
                        Debug.LogError($"{i}: Serialized Array Property is Null");
                        return true;
                    }
                    if (executeBehavior[i] == null)
                    {
                        EditorGUILayout.LabelField($"{i}: Behavior is null");
                        return false;
                    }
                    if (i >= executeBehaviorArrayProperty.arraySize)
                    {
                        EditorGUILayout.LabelField($"{i}: 시리얼라이즈 프로퍼티 길이가 맞지 않음");
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
