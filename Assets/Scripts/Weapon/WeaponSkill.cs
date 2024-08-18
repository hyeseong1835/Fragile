using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSkill : IWeaponEventHandler
{
    public abstract WeaponSkillData Data { get; set; }
    public Weapon weapon;
}
