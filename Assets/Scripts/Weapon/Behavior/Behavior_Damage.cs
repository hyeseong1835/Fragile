using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Behavior_Damage : WeaponBehavior
{
    public WeaponOperator<Entity> targetGetter = WeaponOperator<Entity>.GetDefault();
    public WeaponOperator<float> damageGetter = WeaponOperator<float>.GetDefault();

    public Behavior_Damage()
    {
        Debug.Log("Behavior_Damage is Created!");
    }
    ~Behavior_Damage()
    {
        Debug.Log("Behavior_Damage is Disposed!");
    }
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
    public override void OnGUI(UnityEditor.SerializedObject ruleObject, string path)
    {
        CustomGUILayout.TitleHeaderLabel("데미지");
        CustomGUILayout.BeginTab();
        {
            CustomGUILayout.TitleHeaderLabel("타깃");
            targetGetter.OnGUI(ruleObject, $"{path}.{nameof(targetGetter)}");

            CustomGUILayout.TitleHeaderLabel("값");
            damageGetter.OnGUI(ruleObject, $"{path}.{nameof(damageGetter)}");
        }
        CustomGUILayout.EndTab();
    }
#endif
}