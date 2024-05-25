using Unity.VisualScripting;
using System;

public abstract class BaseGameObjectEventNode<T> : GameObjectEventUnit<T>
{
    [DoNotSerialize] public ControlOutput Out;
    [DoNotSerialize] public abstract string eventName { get; }
    protected override string hookName => eventName;
    [DoNotSerialize] public override Type MessageListenerType => null;
    public override EventHook GetHook(GraphReference reference)
    {
        return new EventHook(eventName);
    }
}
public abstract class GameObjectEventNode : BaseGameObjectEventNode<int>
{

}
public abstract class GameObjectEventNode<T> : BaseGameObjectEventNode<T>
{
    [DoNotSerialize] public ValueOutput OutValue;

    public virtual string argumentName { get { return typeof(T).Name; } }

    protected override void Definition()
    {
        base.Definition();

        Out = ControlOutput(String.Empty);
        OutValue = ValueOutput<T>(argumentName);
    }
    protected override void AssignArguments(Flow flow, T value)
    {
        base.AssignArguments(flow, value);

        flow.SetValue(OutValue, value);
    }
}
public abstract class DefiniteEventNode<T> : GameObjectEventNode<T>
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


