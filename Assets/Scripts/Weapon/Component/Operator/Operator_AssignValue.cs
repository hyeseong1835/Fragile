using System;
using UnityEngine;
using WeaponSystem.Component.Behavior;

namespace WeaponSystem.Component.Operator
{
    [Serializable]
#if UNITY_EDITOR
    [ComponentInfo("할당", "기본적인 연산자입니다.")]
#endif
    public class Operator_AssignFloatValue : WeaponFloatOperator
    {
        public float value;

        public override float GetValue(WeaponBehavior context)
        {
            return value;
        }

#if UNITY_EDITOR
        protected override void DrawHeaderProperty(Rect rect)
        {
            value = UnityEditor.EditorGUI.FloatField(rect, value);
        }
#endif
    }
    [Serializable]
#if UNITY_EDITOR
    [ComponentInfo("할당", "기본적인 연산자입니다.")]
#endif
    public class Operator_AssignIntValue : WeaponIntOperator
    {
        public int value;

        public override int GetValue(WeaponBehavior context)
        {
            return value;
        }

#if UNITY_EDITOR
        protected override void DrawHeaderProperty(Rect rect)
        {
            value = UnityEditor.EditorGUI.IntField(rect, value);
        }
#endif
    }
    [Serializable]
#if UNITY_EDITOR
    [ComponentInfo("할당", "기본적인 연산자입니다.")]
#endif
    public class Operator_AssignStringValue : WeaponStringOperator
    {
        public string value;

        public override string GetValue(WeaponBehavior context)
        {
            return value;
        }

#if UNITY_EDITOR
        protected override void DrawHeaderProperty(Rect rect)
        {
            value = UnityEditor.EditorGUI.TextField(rect, value);
        }
#endif
    }
    [Serializable]
#if UNITY_EDITOR
    [ComponentInfo("할당", "기본적인 연산자입니다.")]
#endif
    public class Operator_AssignObjectValue : WeaponObjectOperator
    {
        public UnityEngine.Object value;

        public override UnityEngine.Object GetValue(WeaponBehavior context)
        {
            return value;
        }

#if UNITY_EDITOR
        protected override void DrawHeaderProperty(Rect rect)
        {
            value = UnityEditor.EditorGUI.ObjectField(
                rect,
                value,
                typeof(UnityEngine.Object),
                true
            );
        }
#endif
    }

    [Serializable]
#if UNITY_EDITOR
    [ComponentInfo("할당", "기본적인 연산자입니다.")]
#endif
    public class Operator_AssignEntityValue : WeaponEntityOperator
    {
        public Entity value;

        public override Entity GetValue(WeaponBehavior context)
        {
            return value;
        }

#if UNITY_EDITOR
        protected override void DrawHeaderProperty(Rect rect)
        {
            value = (Entity)UnityEditor.EditorGUI.ObjectField(
                rect,
                value,
                typeof(Entity),
                true
            );
        }
#endif
    }
}