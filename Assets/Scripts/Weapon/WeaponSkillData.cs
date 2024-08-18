using System;
using UnityEngine;

[Serializable]
public abstract class WeaponSkillData
{
    public abstract WeaponSkill CreateWeaponSkillInstance(Weapon weapon);
}