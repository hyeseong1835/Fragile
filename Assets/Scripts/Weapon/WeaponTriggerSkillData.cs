using UnityEngine;

public class WeaponTriggerSkillData<TSkill, TSkillData> : WeaponSkillData<TSkill, TSkillData>
    where TSkill : WeaponTriggerSkill<TSkill, TSkillData>
    where TSkillData : WeaponTriggerSkillData<TSkill, TSkillData>
{
    public override WeaponSkillBase CreateWeaponSkillInstance(Weapon weapon) => new WeaponTriggerSkill<TSkill, TSkillData>(this, weapon);
    public WeaponBehaviorDataBase[] executeBehaviorData;
}