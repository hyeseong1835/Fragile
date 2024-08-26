using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WeaponSystem.Component.Behavior
{
    [Serializable]
#if UNITY_EDITOR
    [ComponentInfo("행동", "행동합니다.")]
#endif
    public abstract class WeaponBehavior : WeaponComponent
    {
        protected abstract void Initialize();
        public abstract void Execute(Weapon weapon);

#if UNITY_EDITOR
        public static WeaponBehavior GetDefault() => new Behavior_Damage();
#endif
        public override void Dispose()
        {

#if UNITY_EDITOR

#endif
        }

#if UNITY_EDITOR
        protected override Rect HeaderRect => titleRect;
        protected virtual float HeaderHeight => 50;
        protected virtual float HeaderLabelWidth => 150;
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
}
