using UnityEngine;

public abstract class WeaponSkillDataBase
{
    public abstract WeaponSkillBase CreateWeaponSkillInstance(Weapon weapon);
}
public abstract class WeaponSkillData<TSkill, TSkillData> : WeaponSkillDataBase
    where TSkill : WeaponSkill<TSkill, TSkillData>
    where TSkillData : WeaponSkillData<TSkill, TSkillData>
{

}