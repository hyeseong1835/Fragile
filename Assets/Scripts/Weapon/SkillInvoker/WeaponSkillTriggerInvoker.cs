using UnityEngine;
using System;


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
    public override void OnGUI(UnityEditor.SerializedObject ruleObject, string path)
    {
        UnityEditor.EditorGUILayout.LabelField("트리거 시");
        CustomGUILayout.BeginTab();
        {
            CustomGUILayout.ArrayField(
                ref onTrigger, 
                i =>
                {
                    onTrigger[i].OnGUI(ruleObject, $"{path}.{nameof(onTrigger)}.Array.data[{i}]");
                    return false;
                },
                WeaponTriggerSkill.CreateDefault
            );
        }
        CustomGUILayout.EndTab();
    }
#endif
}