using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Behavior", menuName = "Data/Weapon/Behavior")]
public class BehaviorData_Damage : WeaponBehaviorDataBase
{
    public override void Initialize(WeaponBehavior behavior)
    {

    }
    public override void Invoke(WeaponBehavior skill)
    {
        Debug.Log("DamageBehaviorData");
    }
}
