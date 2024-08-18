using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Swing", menuName = "Data/Weapon/ActiveSkill/Swing")]
public class SkillData_Swing : WeaponTriggerSkillData<Skill_Swing, SkillData_Swing>
{
    public override WeaponSkillBase CreateWeaponSkillInstance(Weapon weapon)
        => new Skill_Swing(this, weapon);
}
public class Skill_Swing : WeaponTriggerSkill<Skill_Swing, SkillData_Swing>
{
    public Skill_Swing(SkillData_Swing data, Weapon weapon)
    {
        this.data = data;
        this.weapon = weapon;
        this.executeBehaviors = new WeaponBehaviorBase[data.executeBehaviorData.Length];
        {
            for (int i = 0; i < executeBehaviors.Length; i++)
            {
                executeBehaviors[i] = data.executeBehaviorData[i].CreateWeaponBehaviorInstance(this);
            }
        }
    }
}