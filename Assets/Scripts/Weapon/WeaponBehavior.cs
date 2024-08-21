using System;
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

[Serializable]
public abstract class WeaponBehaviorData
{
#if UNITY_EDITOR
    public static WeaponBehaviorData Default => new BehaviorData_Damage();
#endif
    public abstract WeaponBehavior CreateWeaponBehaviorInstance(WeaponSkill skill);

    public abstract void OnGUI();
}