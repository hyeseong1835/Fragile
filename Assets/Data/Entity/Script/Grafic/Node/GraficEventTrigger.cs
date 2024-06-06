using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GraficEventTrigger<T> : EventNodeTrigger<T>
{
    protected override void SetTarget(Flow flow)
    {
        target = flow.stack.gameObject.transform.Find("Grafic").gameObject;
    }
}

[UnitTitle("Grafic Move Trigger")]
[UnitCategory("Events/Animation")]
public class GraficMoveEventTrigger : GraficEventTrigger<AnimationType>
{
    public override string eventName => "GraficMove";
}

[UnitTitle("Grafic Attack Trigger")]
[UnitCategory("Events/Animation")]
public class GraficAttackEventTrigger : GraficEventTrigger<GraficSkillStyle>
{
    public override string eventName => "GraficAttack";
}

[UnitTitle("Grafic Special Trigger")]
[UnitCategory("Events/Animation")]
public class GraficSpecialEventTrigger : GraficEventTrigger<GraficSkillStyle>
{
    public override string eventName => "GraficSpecial";
}
