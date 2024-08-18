using UnityEngine;

public abstract class WeaponBehaviorDataBase
{
    public abstract WeaponBehaviorBase CreateWeaponBehaviorInstance(WeaponSkill skill);
}
public abstract class WeaponBehaviorData<TBehavior, TBehaviorData> : WeaponBehaviorDataBase
    where TBehavior : WeaponBehavior<TBehavior, TBehaviorData>
    where TBehaviorData : WeaponBehaviorData<TBehavior, TBehaviorData>
{

}