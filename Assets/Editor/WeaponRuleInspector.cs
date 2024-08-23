using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponRule))]
public class WeaponRuleInspector : Editor
{
    WeaponRule weaponRule;

    void OnEnable()
    {
        weaponRule = (WeaponRule)target;

        if (weaponRule.attackInvoker == null)
        {
            weaponRule.attackInvoker = WeaponSkillInvoker.CreateDefault();
        }
        if (weaponRule.specialInvoker == null)
        {
            weaponRule.specialInvoker = WeaponSkillInvoker.CreateDefault();
        }
    }
    public override void OnInspectorGUI()
    {
        WeaponComponent.floatingManager.EventListen();

        weaponRule.controllerValueContainerLength = EditorGUILayout.IntField(weaponRule.controllerValueContainerLength);
        weaponRule.intValueContainerLength = EditorGUILayout.IntField(weaponRule.intValueContainerLength);
        weaponRule.floatValueContainerLength = EditorGUILayout.IntField(weaponRule.floatValueContainerLength);

        weaponRule.attackInvoker.WeaponComponentOnGUI(ref weaponRule.attackInvoker, "기본 공격");
        weaponRule.specialInvoker.WeaponComponentOnGUI(ref weaponRule.specialInvoker, "특수 공격");

        WeaponComponent.floatingManager.Draw();
    }
}