using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(WeaponRule))]
public class WeaponRuleInspector : Editor
{
    WeaponRule weaponRule;

    void OnEnable()
    {
        weaponRule = (WeaponRule)target;
    }
    public override void OnInspectorGUI()
    {
        CustomGUILayout.TitleHeaderLabel("기본 공격");
        CustomGUILayout.BeginTab();
        {
            weaponRule.attackInvoker.OnGUI();
        }
        CustomGUILayout.EndTab();

        CustomGUILayout.TitleHeaderLabel("특수 공격");
        CustomGUILayout.BeginTab();
        {
            weaponRule.specialInvoker.OnGUI();
        }
        CustomGUILayout.EndTab();
    }
}