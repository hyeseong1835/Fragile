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
        [SerializeReference] public WeaponBehavior[] executeBehaviors;
        [SerializeReference] public WeaponBehavior[] hitBehavior;

#if UNITY_EDITOR
        public override void OnGUI()
        {
            CustomGUILayout.TitleHeaderLabel("실행 효과");
            CustomGUILayout.BeginTab();
            {
                CustomGUILayout.ArrayField(
                    ref executeBehaviors,
                    i =>
                    {
                        ref WeaponBehavior behavior = ref executeBehaviors[i];
                        behavior.WeaponComponentOnGUI(ref behavior, $"{i}");
                        return false;
                    },
                    WeaponBehavior.GetDefault
                );
            }
            CustomGUILayout.EndTab();

            CustomGUILayout.TitleHeaderLabel("적중시 효과");
            CustomGUILayout.BeginTab();
            {
                CustomGUILayout.ArrayField(
                    ref hitBehavior,
                    i =>
                    {
                        ref WeaponBehavior behavior = ref hitBehavior[i];
                        behavior.WeaponComponentOnGUI(ref behavior, $"{i}");
                        return false;
                    },
                    WeaponBehavior.GetDefault
                );
            }
            CustomGUILayout.EndTab();
        }
#endif
    }
}