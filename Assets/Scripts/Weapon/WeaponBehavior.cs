using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehavior : IWeaponEventHandler
{
    public Weapon weapon;
    public WeaponSkill skill;
    protected abstract void Initialize();
    public abstract void Invoke();
}

