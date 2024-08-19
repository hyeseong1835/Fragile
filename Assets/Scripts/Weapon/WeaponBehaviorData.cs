using System;

[Serializable]
public abstract class WeaponBehaviorData
{
#if UNITY_EDITOR
    public static WeaponBehaviorData Default => new BehaviorData_Damage();
#endif
    public abstract WeaponBehavior CreateWeaponBehaviorInstance(WeaponSkill skill);

    public abstract void OnGUI();
}