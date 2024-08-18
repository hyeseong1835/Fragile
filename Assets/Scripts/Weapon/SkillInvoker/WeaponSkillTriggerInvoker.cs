using UnityEngine;

public class WeaponSkillTriggerInvoker : WeaponSkillInvoker
{
    [SerializeReference] public WeaponSkillDataBase[] onTrigger = new WeaponSkillDataBase[0];

    public override void WeaponInvokeUpdate()
    {
        foreach (var skill in onTrigger)
        {
            skill.CreateWeaponSkillInstance(null).TriggerSkill();
        }
    }
}