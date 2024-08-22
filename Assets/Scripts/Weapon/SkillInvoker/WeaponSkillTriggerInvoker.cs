using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
#if UNITY_EDITOR
[InvokerInfo("트리거", "버튼을 누르는 즉시 단 한번 실행합니다.")]
#endif
public class WeaponSkillTriggerInvoker : WeaponSkillInvoker
{
    [SerializeReference] public WeaponTriggerSkill[] onTrigger = new WeaponTriggerSkill[0];
    public bool canInvoke = true;

    public WeaponSkillTriggerInvoker()
    {
        Debug.Log("WeaponSkillTriggerInvoker is Created!");
    }
    ~WeaponSkillTriggerInvoker()
    {
        Debug.Log("WeaponSkillTriggerInvoker is Disposed!");
    }

    public override void OnWeaponUpdate()
    {
        if (input)
        {
            if (canInvoke)
            {
                foreach (WeaponTriggerSkill skill in onTrigger)
                {
                    skill.Execute();
                }
                canInvoke = false;
            }
        }
    }

#if UNITY_EDITOR
    public override void OnGUI(SerializedProperty property)
    {
        EditorGUILayout.LabelField("트리거 시");
        CustomGUILayout.BeginTab();
        {
            SerializedProperty onTriggerArrayProperty = property.FindPropertyRelative(nameof(onTrigger));
            CustomGUILayout.ArrayField(
                ref onTrigger, 
                i =>
                {
                    if (onTrigger == null)
                    {
                        Debug.LogError($"{i}: onTrigger is Null!!");
                        return true;
                    }
                    if (onTriggerArrayProperty == null)
                    {
                        Debug.LogError($"{i}: Serialized Array Property is Null");
                        return true;
                    }
                    if (onTrigger[i] == null)
                    {
                        EditorGUILayout.LabelField($"{i}: null");
                        //onTrigger[i] = WeaponTriggerSkill.CreateDefault();
                        //if (WeaponTriggerSkill.CreateDefault() == null) Debug.Log($"{i}: Null!!");
                        return false;
                    }
                    if (i >= onTriggerArrayProperty.arraySize)
                    {
                        EditorGUILayout.LabelField($"{i}: 시리얼라이즈 프로퍼티 길이가 맞지 않음");
                        return false;
                    }
                    onTrigger[i].OnGUI(onTriggerArrayProperty.GetArrayElementAtIndex(i));
                    return false;
                },
                WeaponTriggerSkill.CreateDefault
            );
        }
        CustomGUILayout.EndTab();
    }
#endif
}