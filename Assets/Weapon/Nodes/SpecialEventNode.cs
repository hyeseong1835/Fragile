using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;


[UnitTitle("Special")]//The Custom Scripting Event node to receive the Event. Add "On" to the node title as an Event naming convention.
[UnitCategory("WeaponEvents")]//Set the path to find the node in the fuzzy finder as Events > My Events.
public class SpecialEventNode : GameObjectEventUnit<int>
{
    public static string eventName = "SpecialEvent";
    public Weapon weapon;
    [DoNotSerialize]// No need to serialize ports.
    public ValueOutput result { get; private set; }// The Event output data to return when the Event is triggered.
    protected override string hookName => eventName;
    public override Type MessageListenerType => null;

    protected override void Definition()
    {
        base.Definition();
        // Setting the value on our port.
        result = ValueOutput<int>(nameof(result));
    }
    // Setting the value on our port.
    protected override void AssignArguments(Flow flow, int data)
    {
        flow.SetValue(result, data);
    }
}
