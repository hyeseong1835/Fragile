using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSkillBase : IWeaponEventHandler
{
    public abstract WeaponSkillDataBase Data { get; set; }
    public Weapon weapon;
}
public abstract class WeaponSkill<TSkill, TSkillData> : WeaponSkillBase
    where TSkill : WeaponSkill<TSkill, TSkillData>
    where TSkillData : WeaponSkillData<TSkill, TSkillData>
{
    
}
