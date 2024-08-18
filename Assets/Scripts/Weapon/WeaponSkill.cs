using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSkillBase : IWeaponEventHandler
{
    public abstract WeaponSkillDataBase Data { get; set; }
    public abstract Weapon Weapon { get; set; }
    public abstract void WeaponSkillUpdate();
}
public abstract class WeaponSkill<TSkill, TSkillData> : WeaponSkillBase
    where TSkill : WeaponSkill<TSkill, TSkillData>
    where TSkillData : WeaponSkillData<TSkill, TSkillData>
{
    
}
