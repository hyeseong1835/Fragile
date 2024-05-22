using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public abstract class ActiveSkillEvent : GameObjectEventUnit<int>
{
    [DoNotSerialize] public ValueOutput Ov_index;

    public override Type MessageListenerType => null;
    protected override string hookName => eventName;

    public abstract string eventName { get; }

    protected override void Definition()
    {
        base.Definition();
        Ov_index = ValueOutput<int>("Index");
    }
    protected override void AssignArguments(Flow flow, int _count)
    {
        flow.SetValue(Ov_index, _count);
    }
}
[UnitTitle("Attack")]
[UnitCategory("Events/WeaponEvents")]
public class AttackEventNode : ActiveSkillEvent
{
    public override string eventName => "Attack";
}
[UnitTitle("Special")]
[UnitCategory("Events/WeaponEvents")]
public class SpecialEventNode : ActiveSkillEvent
{
    public override string eventName => "Special";
}