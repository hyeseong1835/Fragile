using System;

[Serializable]
public abstract class WeaponBehaviorData
{
    public abstract WeaponBehavior CreateWeaponBehaviorInstance(WeaponSkill skill);
}