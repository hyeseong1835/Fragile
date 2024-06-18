using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

[UnitTitle("TriggerObject Enter Event")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectEnterEvent : DefiniteEventNode<TriggerType>
{
    public static string name = "TriggerObjectEnter";
    public override string eventName => name;
    public static void Trigger(GameObject gameObject, string id, TriggerType type)
    {
        EventBus.Trigger(DefiniteEventNode.GetEventName(name, id), gameObject, type);
    }
}

[UnitTitle("TriggerObject Stay Event")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectStayEvent : DefiniteEventNode<TriggerType>
{
    public static string name = "TriggerObjectStay";
    public override string eventName => name;
    public static void Trigger(GameObject gameObject, string id, TriggerType type)
    {
        EventBus.Trigger(DefiniteEventNode.GetEventName(name, id), gameObject, type);
    }
}

[UnitTitle("TriggerObject Exit Event")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectExitEvent : DefiniteEventNode<TriggerType>
{
    public static string name = "TriggerObjectExit";
    public override string eventName => name;
    public static void Trigger(GameObject gameObject, string id, TriggerType type)
    {
        EventBus.Trigger(DefiniteEventNode.GetEventName(name, id), gameObject, type);
    }
}

public abstract class  TriggerObjectReceiveNodeBase : ActNode
{
    ValueOutputPort<TriggerType> valueOutport;
    protected override void Definition()
    {
        base.Definition();

        outputPort = ControlOutputPort.Define(this, string.Empty);
        valueOutport = ValueOutputPort<TriggerType>.Define(this);
    }
    protected override void Act(Flow flow) { }
    public void Trigger(TriggerType type)
    {
        Flow flow = Flow.New(reference);
        valueOutport.SetValue(flow, type);
        outputPort.Run(flow);
    }
}