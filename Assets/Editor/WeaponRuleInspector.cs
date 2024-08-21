using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(WeaponRule))]
public class WeaponRuleInspector : Editor
{
    protected static FloatingAreaManager floatingManager = new FloatingAreaManager();
    
    WeaponRule weaponRule;

    void OnEnable()
    {
        weaponRule = (WeaponRule)target;
    }
    public override void OnInspectorGUI()
    {
        floatingManager.EventListen();

        CustomGUILayout.TitleHeaderLabel("�⺻ ����");
        CustomGUILayout.BeginTab();
        {
            weaponRule.attackInvoker.OnGUI();
            DrawInvokerHeader(OnClickedAttackInvokerDropdown);
        }
        CustomGUILayout.EndTab();

        CustomGUILayout.TitleHeaderLabel("Ư�� ����");
        CustomGUILayout.BeginTab();
        {
            weaponRule.specialInvoker.OnGUI();
            DrawInvokerHeader(OnClickedSpecialInvokerDropdown);
        }
        CustomGUILayout.EndTab();

        floatingManager.Draw();

        void DrawInvokerHeader(Func<int, bool> onClickedInvokerDropDown)
        {
            Rect titleRect = GUILayoutUtility.GetRect(1, CustomGUILayout.TitleHeaderLabelHeight);
            CustomGUI.TitleHeaderLabel(titleRect, $"����� ({1})");
            
            if (GUI.Button(
                titleRect.SetSize(50, titleRect.height - 1, Anchor.TopRight),
                "����")
            )
            {
                TextPopupFloatingArea.labelStyle.normal.textColor = Color.white;
                TextPopupFloatingArea.labelStyle.alignment = TextAnchor.MiddleCenter;
                TextPopupFloatingArea.labelStyle.fontSize = 15;
                floatingManager.Create(
                    new TextPopupFloatingArea(
                        WeaponSkillInvokerData.names, 
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
        Debug.Log($"�⺻ �κ�Ŀ Ŭ��({index})");
        weaponRule.attackInvoker = (WeaponSkillInvokerData)Activator.CreateInstance(
            WeaponSkillInvokerData.weaponSkillInvokerDataTypes[index]
        );
        return true;
    }
    bool OnClickedSpecialInvokerDropdown(int index)
    {
        Debug.Log($"Ư�� �κ�Ŀ Ŭ��({index})");
        weaponRule.specialInvoker = (WeaponSkillInvokerData)Activator.CreateInstance(
            WeaponSkillInvokerData.weaponSkillInvokerDataTypes[index]
        );
        return true;
    }
}