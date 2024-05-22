using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public abstract class GameObjectEvent<T> : GameObjectEventUnit<T>
{
    [DoNotSerialize] public ValueOutput OutValue;
    static T v { get { return default; } }

    public override Type MessageListenerType => null;
    protected override string hookName => eventName;
    public abstract string eventName { get; }
    public virtual string argumentName { get { return v.GetType().Name; } }
    protected override void Definition()
    {
        base.Definition();

        OutValue = ValueOutput<T>(argumentName);
    }
    protected override void AssignArguments(Flow flow, T _count)
    {
        flow.SetValue(OutValue, _count);
    }
}
public abstract class VoidGameObjectEvent : GameObjectEvent<int>
{
    protected override void Definition()
    {

    }
    protected override void AssignArguments(Flow flow, int _count)
    {

    }
}
[UnitTitle("Update")]
[UnitCategory("Events/WeaponEvents")]
public class UpdateEventNode : GameObjectEvent<WeaponState>
{
    public override string eventName => "Update";
}
[UnitTitle("Attack")]
[UnitCategory("Events/WeaponEvents")]
public class AttackEventNode : GameObjectEvent<int>
{
    public override string eventName => "Attack";
    public override string argumentName => "Index";

}
[UnitTitle("Special")]
[UnitCategory("Events/WeaponEvents")]
public class SpecialEventNode : GameObjectEvent<int>
{
    public override string eventName => "Special";
    public override string argumentName => "Index";
}