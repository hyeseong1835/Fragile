using Unity.VisualScripting;
using System;

public abstract class ControllerSwichOn<TEnum> : CloseSwitchOnEnum<TEnum> where TEnum : Enum
{
    public Controller con;

    protected override void Act(Flow flow)
    {
        if(con == null) con = flow.stack.gameObject.GetComponent<Controller>();
    }
}

[UnitTitle("Animation")]
[UnitCategory("Animation/Control")]
public class SwitchOnAnimationType : ControllerSwichOn<AnimationType>
{
    public override AnimationType Value => con.animationType;
}

[UnitTitle("Move")]
[UnitCategory("Animation/Control")]
public class SwitchOnMoveAnimationType : ControllerSwichOn<MoveAnimationType>
{
    public override MoveAnimationType Value => con.moveAnimationType;
}

[UnitTitle("Skill")]
[UnitCategory("Animation/Control")]
public class SwitchOnSkillAnimationType : ControllerSwichOn<SkillAnimationType>
{
    public override SkillAnimationType Value => con.skillAnimationType;
}