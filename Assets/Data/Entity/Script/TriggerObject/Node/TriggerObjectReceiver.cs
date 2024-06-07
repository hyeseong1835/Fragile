using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("TriggerObject Enter")]
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

[UnitTitle("TriggerObject Stay")]
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

[UnitTitle("TriggerObject Exit")]
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
