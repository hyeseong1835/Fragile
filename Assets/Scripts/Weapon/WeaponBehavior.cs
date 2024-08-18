using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehaviorBase
{
    public abstract WeaponBehaviorDataBase Data { get; set; }
    public abstract Weapon Weapon { get; set; }

    protected abstract void Initialize();
    public abstract void Invoke();
}
public abstract class WeaponBehavior<TBehavior, TBehaviorData> : WeaponBehaviorBase, IWeaponEventHandler
    where TBehavior : WeaponBehavior<TBehavior, TBehaviorData>
    where TBehaviorData : WeaponBehaviorData<TBehavior, TBehaviorData>
{
    public override WeaponBehaviorDataBase Data
    {
        get => data;
        set => data = (TBehaviorData)value;
    }
    protected TBehaviorData data;
    public override Weapon Weapon
    {
        get => weapon;
        set => weapon = value;
    }
    protected Weapon weapon;
    protected WeaponSkillBase skill;
}

