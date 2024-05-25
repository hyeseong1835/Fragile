using System;
using Unity.VisualScripting;

public abstract class BaseEventNode<T> : EventUnit<T>, IGameObjectEventUnit
{
    public Type MessageListenerType => null;
    protected override bool register => true;
    
    [DoNotSerialize] public abstract string eventName { get; }
    
    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook(eventName);
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
    protected override void AssignArguments(Flow flow, T value)
    {
        base.AssignArguments(flow, value);

        flow.SetValue(OutValue, value);
    }
}
public abstract class DefiniteEventNode<T> : EventNode<T>
{
    [DoNotSerialize] public ValueInput Iv_definite;

    protected override void Definition()
    {
        base.Definition();
        Iv_definite = ValueInput<string>("definite");
    }
    public override EventHook GetHook(GraphReference reference)
    {
        if (Iv_definite == null) { UnityEngine.Debug.Log("Fail: " + eventName); return new EventHook(eventName); }

        UnityEngine.Debug.Log(eventName + Iv_definite);
        return new EventHook(eventName + Iv_definite);
    }
}


