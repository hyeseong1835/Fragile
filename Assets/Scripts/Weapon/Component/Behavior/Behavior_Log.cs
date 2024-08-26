using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Component.Operator;

namespace WeaponSystem.Component.Behavior
{
    [Serializable]
#if UNITY_EDITOR
    [ComponentInfo("�α�", "�ֿܼ� �α׸� ����ϴ�.")]
#endif
    public class Behavior_Log : WeaponBehavior
    {
        public WeaponStringOperator messageOperator = WeaponStringOperator.GetDefault();

        protected override void Initialize()
        {

        }
        public override void Execute(Weapon weapon)
        {
            Debug.Log(messageOperator.GetValue(this));
        }
        protected override void DrawField()
        {
            messageOperator.WeaponComponentOnGUI(ref messageOperator, "�޽���");
        }
    }
}