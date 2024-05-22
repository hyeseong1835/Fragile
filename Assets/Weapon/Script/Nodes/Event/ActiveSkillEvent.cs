using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public abstract class ActiveSkillEvent : GameObjectEventUnit<int>
{
    [DoNotSerialize] public ValueOutput Ov_index;
    public override Type MessageListenerType => null;

    protected override void Definition()
    {
        base.Definition();
        Ov_index = ValueOutput<int>("Index");
        Debug.Log("Definition");
    }
    protected override void AssignArguments(Flow flow, int _count)
    {
        flow.SetValue(Ov_index, _count);
        Debug.Log("AssignArguments");
    }
}
[UnitTitle("Attack")]
[UnitCategory("Events/WeaponEvents")]
public class AttackEventNode : ActiveSkillEvent
{
    protected override string hookName => "Attack";
}
[UnitTitle("Special")]
[UnitCategory("Events/WeaponEvents")]
public class SpecialEventNode : ActiveSkillEvent
{
    protected override string hookName => "Special";
}