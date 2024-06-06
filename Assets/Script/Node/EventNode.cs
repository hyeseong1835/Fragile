using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseEventNode<T> : EventUnit<T>
{
    public GameObject target;
    public Type MessageListenerType => GetType(); 
    protected override bool register => true;
    public abstract string eventName { get; }

    public override EventHook GetHook(GraphReference reference)
    {
        if (target == null) SetTarget(reference);

        return new EventHook(eventName, target);
    }
    protected virtual void SetTarget(GraphReference reference)
    {
        target = reference.gameObject;
    }
}
public abstract class EventNode : BaseEventNode<int>
{

}
public abstract class EventNode<T> : BaseEventNode<T>
{
    [DoNotSerialize] public ValueOutput OutValue;

    public virtual string argumentName { get { return typeof(T).Name; } }

    protected override void Definition()
    {
        base.Definition();

        OutValue = ValueOutput<T>(argumentName);
    }
    protected override void AssignArguments(Flow flow, T args)
    {
        flow.SetValue(OutValue, args);
    }
}
public abstract class DefiniteEventNode : EventNode
{
    public static string GetEventName(string eventName, string id)
    {
        return $"{eventName}({id})";
    }
    [DoNotSerialize] public ValueInput Iv_id;
    public string id;

    protected override void Definition()
    {
        base.Definition();

        Iv_id = ValueInput<string>("ID");
    }
    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook(GetEventName(eventName, Flow.New(reference).GetValue<string>(Iv_id)), reference.gameObject);
    }
}
public abstract class DefiniteEventNode<T> : EventNode<T>
{
    [DoNotSerialize] public ValueInput Iv_id;

    protected override void Definition()
    {
        base.Definition();

        Iv_id = ValueInput<string>("ID");
    }
    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook(DefiniteEventNode.GetEventName(eventName, Flow.New(reference).GetValue<string>(Iv_id)), reference.gameObject);
    }
}


