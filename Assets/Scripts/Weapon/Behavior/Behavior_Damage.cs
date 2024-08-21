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
    public override void OnGUI(UnityEditor.SerializedProperty property)
    {
        CustomGUILayout.TitleHeaderLabel("Å¸±ê");
        targetGetter.OnGUI(property.FindPropertyRelative(nameof(targetGetter)));
        
        CustomGUILayout.TitleHeaderLabel("µ¥¹ÌÁö");
        damageGetter.OnGUI(property.FindPropertyRelative(nameof(damageGetter)));
    }
#endif
}