using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(WeaponRule))]
public class WeaponRuleInspector : Editor
{
    WeaponRule weaponRule;

    SerializedProperty attackInvoker;
    SerializedProperty specialInvoker;

    void OnEnable()
    {
        weaponRule = (WeaponRule)target;
        
        if (weaponRule.attackInvoker == null) weaponRule.attackInvoker = WeaponSkillInvokerData.Default;
        attackInvoker = serializedObject.FindProperty("attackInvoker");

        if (weaponRule.specialInvoker == null) weaponRule.specialInvoker = WeaponSkillInvokerData.Default;
        specialInvoker = serializedObject.FindProperty("specialInvoker");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        {
            EditorGUILayout.PropertyField(attackInvoker, label: new GUIContent("기본 공격"));
            EditorGUILayout.PropertyField(specialInvoker, label: new GUIContent("특수 공격"));
        }
        serializedObject.ApplyModifiedProperties();
    }
}