using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EventNodeTrigger : ActNode
{
    protected GameObject target;
    protected abstract string eventName { get; }

    protected override void Act(Flow flow)
    {
        if (target == null) target = flow.stack.gameObject;

        EventBus.Trigger(eventName, target, -1);
    }
}
public abstract class EventNodeTrigger<TArg> : ActNode
{
    protected GameObject target;

    protected ValueInputPort<TArg> valuePort;
    protected abstract string eventName { get; }

    protected override void Definition()
    {
        base.Definition();

        valuePort = ValueInputPort<TArg>.Define(this);
    }
    protected override void Act(Flow flow)
    {
        if (target == null) target = flow.stack.gameObject;

        EventBus.Trigger(eventName, target, valuePort.GetValue(flow));
    }
}