using UnityEngine;

public abstract class WeaponTriggerSkillData<TSkill, TData> : WeaponSkillData<TSkill, TData>
    where TSkill : WeaponTriggerSkill<TSkill, TData>
    where TData : WeaponTriggerSkillData<TSkill, TData>
{
    public WeaponBehaviorDataBase[] executeBehaviorData;
}