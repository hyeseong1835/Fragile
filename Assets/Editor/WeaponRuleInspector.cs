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
        WeaponRuleComponent.floatingManager.EventListen();

        weaponRule.controllerValueContainerLength = EditorGUILayout.IntField(weaponRule.controllerValueContainerLength);
        weaponRule.intValueContainerLength = EditorGUILayout.IntField(weaponRule.intValueContainerLength);
        weaponRule.floatValueContainerLength = EditorGUILayout.IntField(weaponRule.floatValueContainerLength);

        weaponRule.attackInvoker.OnGUI(ref weaponRule.attackInvoker, "기본 공격");
        weaponRule.specialInvoker.OnGUI(ref weaponRule.specialInvoker, "특수 공격");

        WeaponRuleComponent.floatingManager.Draw();
    }
}