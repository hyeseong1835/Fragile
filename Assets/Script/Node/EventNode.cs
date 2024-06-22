using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public abstract class EventNode : EventUnit<int>
{
    public Type MessageListenerType => GetType();
    protected override bool register => true;
    public abstract string eventName { get; }

    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook(eventName, reference.gameObject);
    }
}
public abstract class EventNode<T> : EventUnit<T>
{
    public Type MessageListenerType => GetType();
    protected override bool register => true;
    public abstract string eventName { get; }
    
    protected ValueOutputPort<T> valueOutputPort;
    public virtual string argumentName { get { return typeof(T).Name; } }

    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook(eventName, reference.gameObject);
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
    [Serialize, Inspectable] public string id;

    /// <summary>
    /// {eventName}({id})
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetEventName(string eventName, string id) => $"{eventName}({id})";

    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook(GetEventName(eventName, id), reference.gameObject);
    }
}
public abstract class DefiniteEventNode<T> : EventNode<T>
{
    [Serialize, Inspectable] public string id;

    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook(DefiniteEventNode.GetEventName(eventName, id), reference.gameObject);
    }
}