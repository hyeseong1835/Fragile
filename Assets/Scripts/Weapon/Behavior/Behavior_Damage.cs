using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class BehaviorData_Damage : WeaponBehaviorData
{
    public override WeaponBehavior CreateWeaponBehaviorInstance(WeaponSkill skill)
        => new Behavior_Damage(this);

    public WeaponOperator<Entity> targetGetter;
    public WeaponOperator<float> damageGetter;

#if UNITY_EDITOR
    public override void OnGUI()
    {
        CustomGUILayout.TitleHeaderLabel("µ¥¹ÌÁö");
    }
#endif
}
public class Behavior_Damage : WeaponBehavior
{
    BehaviorData_Damage data;

    public Behavior_Damage(BehaviorData_Damage data)
    { 
        this.data = data;
    }

    protected override void Initialize()
    {
        Debug.Log("DamageBehavior");
    }
    public override void Invoke()
    {
        Entity target = data.targetGetter.GetValue(this);
        float damage = data.damageGetter.GetValue(this);

        target.TakeDamage(damage);
    }
}
