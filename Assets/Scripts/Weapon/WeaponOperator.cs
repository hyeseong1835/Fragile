using System;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class WeaponOperator<ValueT> : WeaponRuleComponent
{
    public static WeaponOperator<ValueT> GetDefault() => new Operator_AssignValue<ValueT>();
    public abstract ValueT GetValue(WeaponBehavior behavior);
    public override void Dispose()
    {

#if UNITY_EDITOR

#endif
    }

#if UNITY_EDITOR
    protected virtual float HeaderHeight => 25;
    protected virtual float HeaderLabelWidth => 100;
    protected virtual float ChangeFlotingElementHeight => 20;

    protected Rect titleRect;
    protected int changeOrigin = -1;
    [NonSerialized] public int selectedIndex = -1;

    public override void OnGUI<TComponent>(ref TComponent origin, string label)
    {
        if (ComponentTypes == null) return;
        if (selectedIndex == -1) selectedIndex = Array.IndexOf(ComponentTypes, GetType());
        DrawHeader(label);
        DrawField();

        if (changeOrigin != -1)
        {
            Dispose();

            origin = (TComponent)Activator.CreateInstance(
                ComponentTypes[changeOrigin]
            );
        }
    }
    protected virtual void CreateMenu(GenericMenu menu)
    {
        menu.AddItem(
            new GUIContent("변경"),
            false,
            ChangeOrigin
        );
    }
    protected virtual void DrawHeader(string label)
    {
        titleRect = GUILayoutUtility.GetRect(
            0,
            HeaderHeight,
            GUILayout.ExpandHeight(false)
        ); 
        GUI.Label(titleRect, $"{label} ({Names[selectedIndex]})");
        if (Event.current.type == EventType.MouseDown && Event.current.button == 1
            && titleRect.SetWidth(HeaderLabelWidth).Contains(Event.current.mousePosition))
        {
            menu = new GenericMenu();
            CreateMenu(menu);
            menu.ShowAsContext();
        }
        DrawHeaderProperty(titleRect.AddHeight(-1).AddWidth(-HeaderLabelWidth, 1));
    }
    protected virtual void DrawHeaderProperty(Rect rect)
    {

    }
    protected virtual void DrawField()
    {

    }
    protected virtual void ChangeOrigin()
    {
        TextPopupFloatingArea.labelStyle.normal.textColor = Color.white;
        TextPopupFloatingArea.labelStyle.alignment = TextAnchor.MiddleLeft;
        TextPopupFloatingArea.labelStyle.fontSize = 15;
        floatingManager.Create(
            new TextPopupFloatingArea(
                WeaponSkillInvoker.names,
                i => { changeOrigin = i; return true; },
                height: ChangeFlotingElementHeight
            )
        );
        floatingManager.SetRect(titleRect);
    }
#endif
}