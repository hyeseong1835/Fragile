using System.Reflection;
using UnityEngine;
using UnityEngine.Windows;

public class WeaponSkillTriggerInvoker : WeaponSkillInvoker
{
    [SerializeReference] public WeaponTriggerSkill[] onTrigger;
    public bool canInvoke = true;

    public WeaponSkillTriggerInvoker(WeaponSkillTriggerInvokerData data)
    {

    }
    public override void WeaponInvokeUpdate()
    {
        if (input)
        {
            if (canInvoke)
            {
                foreach (WeaponTriggerSkill skill in onTrigger)
                {
                    skill.WeaponSkillExecute();
                }
                canInvoke = false;
            }
        }
    }
}
public class WeaponSkillTriggerInvokerData : WeaponSkillInvokerData
{
    [SerializeReference] public WeaponTriggerSkillData[] onTrigger = new WeaponTriggerSkillData[0];

    public override WeaponSkillInvoker CreateInvoker() => new WeaponSkillTriggerInvoker(this);
}