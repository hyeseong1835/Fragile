using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Behavior", menuName = "Data/Weapon/Behavior")]
public class BehaviorData_Damage : WeaponBehaviorData<Behavior_Damage, BehaviorData_Damage>
{
    public override WeaponBehaviorBase CreateWeaponBehaviorInstance(WeaponSkillBase skill)
        => new Behavior_Damage(this);
}
public class Behavior_Damage : WeaponBehavior<Behavior_Damage, BehaviorData_Damage>
{
    public Behavior_Damage(BehaviorData_Damage data)
    { 
        
    }

    protected override void Initialize()
    {
        Debug.Log("DamageBehavior");
    }
    public override void Invoke()
    {
        Debug.Log("DamageBehavior");
    }
}
