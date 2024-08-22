using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
#if UNITY_EDITOR
[BehaviorInfo("������", "��󿡰� ���ظ� ���մϴ�")]
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
            targetGetter.OnGUI(ref targetGetter, "���");

            damageGetter.OnGUI(ref damageGetter, "���ط�");
        }
        CustomGUILayout.EndTab();
    }
#endif
}