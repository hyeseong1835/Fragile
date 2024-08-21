using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponRule))]
public class WeaponRuleInspector : Editor
{
    protected static FloatingAreaManager floatingManager = new FloatingAreaManager();
    
    WeaponRule weaponRule;

    SerializedProperty attackInvokerProperty;
    SerializedProperty specialInvokerProperty;

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
        attackInvokerProperty = serializedObject.FindProperty("attackInvoker");
        specialInvokerProperty = serializedObject.FindProperty("specialInvoker");
    }
    public override void OnInspectorGUI()
    {
        if (attackInvokerProperty == null) attackInvokerProperty = serializedObject.FindProperty(nameof(weaponRule.attackInvoker));
        if (specialInvokerProperty == null) specialInvokerProperty = serializedObject.FindProperty(nameof(weaponRule.specialInvoker));
        
        floatingManager.EventListen();

        CustomGUILayout.TitleHeaderLabel("기본 공격");
        CustomGUILayout.BeginTab();
        {
            DrawInvokerHeader(OnClickedAttackInvokerDropdown, weaponRule.selectedAttackInvokerIndex);
            weaponRule.attackInvoker.OnGUI(attackInvokerProperty);
        }
        CustomGUILayout.EndTab();

        CustomGUILayout.TitleHeaderLabel("특수 공격");
        CustomGUILayout.BeginTab();
        {
            DrawInvokerHeader(OnClickedSpecialInvokerDropdown, weaponRule.selectedSpecialInvokerIndex);
            weaponRule.specialInvoker.OnGUI(specialInvokerProperty);
        }
        CustomGUILayout.EndTab();

        floatingManager.Draw();

        void DrawInvokerHeader(Func<int, bool> onClickedInvokerDropDown, int selectedInvokerIndex)
        {
            Rect titleRect = GUILayoutUtility.GetRect(1, CustomGUILayout.TitleHeaderLabelHeight);

            CustomGUI.TitleHeaderLabel(titleRect, $"실행기 ({WeaponSkillInvoker.names[selectedInvokerIndex]})");
            if (GUI.Button(
                titleRect.SetSize(
                    75, 
                    titleRect.height - 1, 
                    Anchor.TopRight
                ).SetHeight(titleRect.height - 5, 1),
                "변경")
            )
            {
                TextPopupFloatingArea.labelStyle.normal.textColor = Color.white;
                TextPopupFloatingArea.labelStyle.alignment = TextAnchor.MiddleLeft;
                TextPopupFloatingArea.labelStyle.fontSize = 15;
                floatingManager.Create(
                    new TextPopupFloatingArea(
                        WeaponSkillInvoker.names, 
                        onClickedInvokerDropDown,
                        height: 20
                    )
                );
                floatingManager.SetRect(titleRect);
            }
        }
    }
    bool OnClickedAttackInvokerDropdown(int index)
    {
        weaponRule.attackInvoker = (WeaponSkillInvoker)Activator.CreateInstance(
            WeaponSkillInvoker.weaponSkillInvokerTypes[index]
        );
        return true;
    }
    bool OnClickedSpecialInvokerDropdown(int index)
    {
        weaponRule.specialInvoker = (WeaponSkillInvoker)Activator.CreateInstance(
            WeaponSkillInvoker.weaponSkillInvokerTypes[index]
        );
        return true;
    }
}