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
        floatingManager.EventListen();

        weaponRule.controllerValueContainerLength = EditorGUILayout.IntField(weaponRule.controllerValueContainerLength);
        weaponRule.intValueContainerLength = EditorGUILayout.IntField(weaponRule.intValueContainerLength);
        weaponRule.floatValueContainerLength = EditorGUILayout.IntField(weaponRule.floatValueContainerLength);

        CustomGUILayout.TitleHeaderLabel("기본 공격");
        CustomGUILayout.BeginTab();
        {
            DrawInvokerHeader(OnClickedAttackInvokerDropdown, weaponRule.selectedAttackInvokerIndex);
            weaponRule.attackInvoker.OnGUI(serializedObject, nameof(weaponRule.attackInvoker));
        }
        CustomGUILayout.EndTab();

        CustomGUILayout.TitleHeaderLabel("특수 공격");
        CustomGUILayout.BeginTab();
        {
            DrawInvokerHeader(OnClickedSpecialInvokerDropdown, weaponRule.selectedSpecialInvokerIndex);
            weaponRule.specialInvoker.OnGUI(serializedObject, nameof(weaponRule.specialInvoker));
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