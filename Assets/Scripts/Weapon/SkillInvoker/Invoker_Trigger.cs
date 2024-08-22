using UnityEngine;
using System;


[Serializable]
#if UNITY_EDITOR
[@ComponentInfo("트리거", "버튼을 누르는 즉시 단 한번 실행합니다.")]
#endif
public class Invoker_Trigger : WeaponSkillInvoker
{
    [SerializeReference] public WeaponTriggerSkill[] onTrigger = new WeaponTriggerSkill[0];
    public bool canInvoke = true;

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
    protected override void DrawField()
    {
        CustomGUILayout.BeginTab();
        {
            CustomGUILayout.ArrayField(
                ref onTrigger, 
                i =>
                {
                    onTrigger[i].OnGUI(ref onTrigger[i], i.ToString());
                    return false;
                },
                WeaponTriggerSkill.CreateDefault
            );
        }
        CustomGUILayout.EndTab();
    }
#endif
}