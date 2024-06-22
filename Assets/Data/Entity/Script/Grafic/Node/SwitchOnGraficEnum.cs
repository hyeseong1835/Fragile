using Unity.VisualScripting;

[UnitTitle("Animation")]
[UnitCategory("Animation/Control")]
public class SwitchOnAnimationType : GetComponentSwitchOnEnum<Controller, AnimationType>
{
    protected override AnimationType GetValue(Flow flow) => component.animationType;
}

[UnitTitle("Move")]
[UnitCategory("Animation/Control")]
public class SwitchOnMoveAnimationType : GetComponentSwitchOnEnum<Controller, MoveAnimationType>
{
    protected override MoveAnimationType GetValue(Flow flow) => component.moveAnimationType;
}

[UnitTitle("Skill")]
[UnitCategory("Animation/Control")]
public class SwitchOnSkillAnimationType : GetComponentSwitchOnEnum<Controller, SkillAnimationType>
{
    protected override SkillAnimationType GetValue(Flow flow) => component.skillAnimationType;
}