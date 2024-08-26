using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Component.Operator;

namespace WeaponSystem.Component.Behavior
{
    [Serializable]
#if UNITY_EDITOR
    [ComponentInfo("������", "��󿡰� ���ظ� ���մϴ�")]
#endif
    public class Behavior_Damage : WeaponBehavior
    {
        public WeaponEntityOperator targetGetter = WeaponEntityOperator.GetDefault();
        public WeaponFloatOperator damageGetter = WeaponFloatOperator.GetDefault();

        protected override void Initialize()
        {
            Debug.Log("DamageBehavior");
        }
        public override void Execute(Weapon weapon)
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
                targetGetter.WeaponComponentOnGUI(ref targetGetter, "���");

                damageGetter.WeaponComponentOnGUI(ref damageGetter, "���ط�");
            }
            CustomGUILayout.EndTab();
        }
#endif
    }
}