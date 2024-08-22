using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class BehaviorInfoAttribute : WeaponRuleComponentInfoAttribute
{
    public BehaviorInfoAttribute(string name, string description)
    {
        this.path = name;
        this.description = description;
    }
}
#endif
[Serializable]
public abstract class WeaponBehavior : WeaponRuleComponent
{
    public WeaponSkill skill;

    protected abstract void Initialize();
    public abstract void Execute();

#if UNITY_EDITOR
    public static WeaponBehavior GetDefault() => new Behavior_Damage();
#endif
    public override void Dispose()
    {

#if UNITY_EDITOR

#endif
    }

#if UNITY_EDITOR
    protected override Type[] ComponentTypes => behaviorTypes;
    protected override string[] Names => names;
    protected override string[] Descriptions => descriptions;

    public static Type[] behaviorTypes;
    public static string[] names;
    public static string[] descriptions;

    [InitializeOnLoadMethod]
    static void OnLoad()
        => LoadChildType<WeaponBehavior, BehaviorInfoAttribute>(ref behaviorTypes, ref names, ref descriptions);

    protected virtual float HeaderHeight => 50;
    protected virtual float HeaderLabelWidth => 150;
    protected virtual float ChangeFlotingElementHeight => 20;
    protected Rect titleRect;
    protected int changeOrigin = -1;
    [NonSerialized] public int selectedIndex = -1;

    public override void OnGUI<TComponent>(ref TComponent origin, string label)
    {
        if (behaviorTypes == null) return;
        if (selectedIndex == -1) selectedIndex = Array.IndexOf(behaviorTypes, GetType());
        DrawHeader(label);
        DrawField();

        if (changeOrigin != -1)
        {
            Dispose();

            origin = (TComponent)Activator.CreateInstance(
                behaviorTypes[changeOrigin]
            );
        }
    }
    protected virtual void CreateMenu(GenericMenu menu)
    {
        menu.AddItem(
            new GUIContent("º¯°æ"),
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
        GUI.Label(titleRect, $"{label} ({names[selectedIndex]})");
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