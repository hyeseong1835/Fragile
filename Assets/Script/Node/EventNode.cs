using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public abstract class EventNode : EventUnit<int>
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
public abstract class EventNode<T> : EventUnit<T>
{
    public GameObject target;
    public Type MessageListenerType => GetType();
    protected override bool register => true;
    public abstract string eventName { get; }
    
    protected ValueOutputPort<T> valueOutputPort;
    public virtual string argumentName { get { return typeof(T).Name; } }

    public override EventHook GetHook(GraphReference reference)
    {
        if (target == null) SetTarget(reference);

        return new EventHook(eventName, target);
    }
    protected virtual void SetTarget(GraphReference reference)
    {
        target = reference.gameObject;
    }

    protected override void Definition()
    {
        base.Definition();

        valueOutputPort = ValueOutputPort<T>.Define(this, argumentName);
    }
    protected override void AssignArguments(Flow flow, T arg)
    {
        valueOutputPort.SetValue(flow, arg);
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
    protected ValueInputPort<string> idPort;

    protected override void Definition()
    {
        base.Definition();

        idPort = ValueInputPort<string>.Define(this, "ID");
    }
    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook(DefiniteEventNode.GetEventName(eventName, idPort.GetValue(Flow.New(reference))), reference.gameObject);
    }
}