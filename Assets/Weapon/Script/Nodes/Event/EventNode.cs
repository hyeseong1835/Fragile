using Unity.VisualScripting;
using System;

public abstract class BaseEventNode<T> : EventUnit<T>
{
    [DoNotSerialize] public ControlOutput Out;
    protected override bool register => true;
    public abstract string eventName { get; }
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

    static T v { get { return default; } }
    public virtual string argumentName { get { return v.GetType().Name; } }

    protected override void Definition()
    {
        Out = ControlOutput(String.Empty);
        OutValue = ValueOutput<T>(argumentName);
    }
    protected override void AssignArguments(Flow flow, T value)
    {
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

