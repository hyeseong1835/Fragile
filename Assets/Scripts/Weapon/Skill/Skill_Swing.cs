using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Swing", menuName = "Data/Weapon/ActiveSkill/Swing")]
public class Skill_Swing : WeaponTriggerSkillData
{
    public override void SkillEnd(WeaponTriggerSkill skill, float time)
    {
        Debug.Log("Swing End");
    }

    public override void SkillExecute(WeaponTriggerSkill skill, float time)
    {
        Debug.Log("Swing");
    }

    public override void SkillInitialize(WeaponTriggerSkill skill)
    {
        throw new System.NotImplementedException();
    }

    public override void SkillStart(WeaponTriggerSkill skill)
    {
        Debug.Log("Swing Start");
    }
}