using System;
using System.Reflection;
using UnityEngine;
using WeaponSystem.Component.Behavior;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WeaponSystem.Component.Operator
{
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
    [ComponentInfo("연산자(Float)", "무기의 연산자입니다.")]
    public abstract class WeaponFloatOperator : WeaponOperator<float>
    {

#if UNITY_EDITOR
        public static WeaponFloatOperator GetDefault() => new Operator_AssignFloatValue();
#endif
    }
    [ComponentInfo("연산자(Int)", "무기의 연산자입니다.")]
    public abstract class WeaponIntOperator : WeaponOperator<int>
    {

#if UNITY_EDITOR
        public static WeaponIntOperator GetDefault() => new Operator_AssignIntValue();
#endif
    }
    [ComponentInfo("연산자(Object)", "무기의 연산자입니다.")]
    public abstract class WeaponObjectOperator : WeaponOperator<UnityEngine.Object>
    {

#if UNITY_EDITOR
        public static WeaponObjectOperator GetDefault() => new Operator_AssignObjectValue();
#endif
    }
    [ComponentInfo("연산자(Entity)", "무기의 연산자입니다.")]
    public abstract class WeaponEntityOperator : WeaponOperator<Entity>
    {

#if UNITY_EDITOR
        public static WeaponEntityOperator GetDefault() => new Operator_AssignEntityValue();
#endif
    }
}