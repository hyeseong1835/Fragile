using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
#if UNITY_EDITOR
[BehaviorInfo("데미지", "대상에게 피해를 가합니다")]
#endif
public class Behavior_Damage : WeaponBehavior
{
    public WeaponOperator<Entity> targetGetter = WeaponOperator<Entity>.GetDefault();
    public WeaponOperator<float> damageGetter = WeaponOperator<float>.GetDefault();

    protected override void Initialize()
    {
        Debug.Log("DamageBehavior");
    }
    public override void Execute()
    {
        Entity target = targetGetter.GetValue(this);
        float damage = damageGetter.GetValue(this);

        target.TakeDamage(damage);
    }

#if UNITY_EDITOR
    protected override void DrawField()
    {
        CustomGUILayout.BeginTab();
        {
            targetGetter.OnGUI(ref targetGetter, "대상");

            damageGetter.OnGUI(ref damageGetter, "피해량");
        }
        CustomGUILayout.EndTab();
    }
#endif
}