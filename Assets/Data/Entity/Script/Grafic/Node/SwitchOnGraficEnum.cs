using Unity.VisualScripting;
using System;
using UnityEngine;
using UnityEditor;

public abstract class ControllerSwichOn<TEnum> : SwitchOnEnum<TEnum> where TEnum : Enum
{
    protected Controller con;
    protected abstract TEnum Value { get; }
    protected override TEnum GetValue(Flow flow)
    {
        if (con == null) con = flow.stack.gameObject.GetComponent<Controller>();
        return Value;
    }
}

[UnitTitle("Animation")]
[UnitCategory("Animation/Control")]
public class SwitchOnAnimationType : ControllerSwichOn<AnimationType>
{
    protected override AnimationType Value => con.animationType;
}

[UnitTitle("Move")]
[UnitCategory("Animation/Control")]
public class SwitchOnMoveAnimationType : ControllerSwichOn<MoveAnimationType>
{
    protected override MoveAnimationType Value => con.moveAnimationType;
}

[UnitTitle("Skill")]
[UnitCategory("Animation/Control")]
public class SwitchOnSkillAnimationType : ControllerSwichOn<SkillAnimationType>
{
    protected override SkillAnimationType Value => con.skillAnimationType;
}