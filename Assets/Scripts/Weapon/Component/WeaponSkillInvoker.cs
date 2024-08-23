using System;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
#if UNITY_EDITOR
[ComponentInfo("실행기", "스킬을 실행합니다.")]
#endif
public abstract class WeaponSkillInvoker : WeaponComponent
{
    public static WeaponSkillInvoker CreateDefault() => new Invoker_Trigger();

    public bool input = false;

    public abstract void OnWeaponUpdate();

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
    protected override Rect HeaderRect => titleRect;

    protected override void OnGUI(string label)
    {
        titleRect = GUILayoutUtility.GetRect(
            0,
            HeaderHeight,
            GUILayout.ExpandHeight(false)
        );
        CustomGUI.TitleHeaderLabel(titleRect, $"{label} ({info.name})");
        
        DrawHeaderProperty(titleRect.AddHeight(-1).AddWidth(-HeaderLabelWidth, 1));
        DrawField();
    }
    protected virtual void DrawHeaderProperty(Rect rect)
    {

    }
    protected virtual void DrawField()
    {

    }
#endif
}