using System;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
#if UNITY_EDITOR
[ComponentInfo("스킬", "무기의 스킬입니다.")]
#endif
public abstract class WeaponSkill : WeaponComponent
{
    public Weapon weapon;

    public abstract void Execute();


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
        CustomGUI.TitleHeaderLabel(titleRect, $"{label} ({info.name})");

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