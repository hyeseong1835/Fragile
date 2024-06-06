using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseEventNodeTrigger<T> : Node
{
    public ValueInput value;
    public GameObject target;
    public abstract string eventName { get; }

    protected override void Definition()
    {
        base.Definition();

        value = ValueInput<T>("Value");
    }
    protected virtual void SetTarget(Flow flow)
    {
        target = flow.stack.gameObject;
    }
    protected override void Act(Flow flow)
    {
        if (target == null) SetTarget(flow);
    }
}
public abstract class EventNodeTrigger : BaseEventNodeTrigger<int>
{
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        EventBus.Trigger(eventName, target, -1);
    }
}
public abstract class EventNodeTrigger<T> : BaseEventNodeTrigger<T>
{
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        EventBus.Trigger(eventName, target, flow.GetValue<T>(value));
    }
}