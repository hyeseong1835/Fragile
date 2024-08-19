using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BehaviorData_Damage : WeaponBehaviorData
{
    public override WeaponBehavior CreateWeaponBehaviorInstance(WeaponSkill skill)
        => new Behavior_Damage(this);
}
public class Behavior_Damage : WeaponBehavior
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
