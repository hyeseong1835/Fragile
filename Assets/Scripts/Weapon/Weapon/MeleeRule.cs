using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WeaponSystem.Component.Behavior;

namespace WeaponSystem
{
    [CreateAssetMenu(fileName = "WeaponRule", menuName = "Data/����/���� ����/��Ģ")]
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
            CustomGUILayout.TitleHeaderLabel("�⺻ ����");
            CustomGUILayout.BeginTab();
            {
                CustomGUILayout.WeaponBehaviorArrayField(ref attackExecuteBehaviors, "���� ȿ��");
                CustomGUILayout.WeaponBehaviorArrayField(ref attackHitBehavior, "���߽� ȿ��");
                CustomGUILayout.WeaponBehaviorArrayField(ref attackRechargeBehaviors, "������ ȿ��");
            }
            CustomGUILayout.EndTab();

            CustomGUILayout.TitleHeaderLabel("�⺻ ����");
            CustomGUILayout.BeginTab();
            {
                CustomGUILayout.WeaponBehaviorArrayField(ref specialExecuteBehaviors, "���� ȿ��");
                CustomGUILayout.WeaponBehaviorArrayField(ref specialHitBehavior, "���߽� ȿ��");
                CustomGUILayout.WeaponBehaviorArrayField(ref specialRechargeBehaviors, "������ ȿ��");
            }
            CustomGUILayout.EndTab();
        }
#endif
    }
}