using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WeaponSystem.Component.Behavior;

namespace WeaponSystem
{
    [CreateAssetMenu(fileName = "WeaponRule", menuName = "Data/무기/근접 무기/규칙")]
    public class MeleeRule : WeaponRule
    {
        [SerializeReference] public WeaponBehavior[] attackExecuteBehaviors;
        [SerializeReference] public WeaponBehavior[] attackHitBehavior;
        [SerializeReference] public WeaponBehavior[] attackRechargeBehaviors;

        [SerializeReference] public WeaponBehavior[] specialExecuteBehaviors;
        [SerializeReference] public WeaponBehavior[] specialHitBehavior;
        [SerializeReference] public WeaponBehavior[] specialRechargeBehaviors;

#if UNITY_EDITOR
        public override void OnGUI()
        {
            CustomGUILayout.TitleHeaderLabel("기본 공격");
            CustomGUILayout.BeginTab();
            {
                CustomGUILayout.WeaponBehaviorArrayField(ref attackExecuteBehaviors, "실행 효과");
                CustomGUILayout.WeaponBehaviorArrayField(ref attackHitBehavior, "적중시 효과");
                CustomGUILayout.WeaponBehaviorArrayField(ref attackRechargeBehaviors, "재충전 효과");
            }
            CustomGUILayout.EndTab();

            CustomGUILayout.TitleHeaderLabel("기본 공격");
            CustomGUILayout.BeginTab();
            {
                CustomGUILayout.WeaponBehaviorArrayField(ref specialExecuteBehaviors, "실행 효과");
                CustomGUILayout.WeaponBehaviorArrayField(ref specialHitBehavior, "적중시 효과");
                CustomGUILayout.WeaponBehaviorArrayField(ref specialRechargeBehaviors, "재충전 효과");
            }
            CustomGUILayout.EndTab();
        }
#endif
    }
}