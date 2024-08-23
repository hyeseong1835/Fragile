﻿using System;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ComponentInfo("연산자", "무기의 연산자입니다.")]
public abstract class WeaponOperator<ValueT> : WeaponComponent
{
    public abstract ValueT GetValue(WeaponBehavior behavior);
    public override void Dispose()
    {

#if UNITY_EDITOR

#endif
    }

#if UNITY_EDITOR
    protected override Rect HeaderRect => titleRect;
    protected virtual float HeaderHeight => 25;
    protected virtual float HeaderLabelWidth => 100;
    protected virtual float ChangeFlotingElementHeight => 20;

    protected Rect titleRect;

    protected override void OnGUI(string label)
    {
        DrawHeader(label);
        DrawField();
    }
    protected virtual void DrawHeader(string label)
    {
        titleRect = GUILayoutUtility.GetRect(
            0,
            HeaderHeight,
            GUILayout.ExpandHeight(false)
        ); 
        GUI.Label(titleRect, $"{label} ({info.name})");
        DrawHeaderProperty(titleRect.AddHeight(-1).AddWidth(-HeaderLabelWidth, 1));
    }
    protected virtual void DrawHeaderProperty(Rect rect)
    {

    }
    protected virtual void DrawField()
    {

    }
#endif
}
public abstract class WeaponFloatOperator : WeaponOperator<float>
{

#if UNITY_EDITOR
    public static WeaponFloatOperator GetDefault() => new Operator_AssignFloatValue();
#endif
}
public abstract class WeaponIntOperator : WeaponOperator<int>
{

#if UNITY_EDITOR
    public static WeaponIntOperator GetDefault() => new Operator_AssignIntValue();
#endif
}
public abstract class WeaponObjectOperator : WeaponOperator<UnityEngine.Object>
{

#if UNITY_EDITOR
    public static WeaponObjectOperator GetDefault() => new Operator_AssignObjectValue();
#endif
}
public abstract class WeaponEntityOperator : WeaponOperator<Entity>
{

#if UNITY_EDITOR
    public static WeaponEntityOperator GetDefault() => new Operator_AssignEntityValue();
#endif
}