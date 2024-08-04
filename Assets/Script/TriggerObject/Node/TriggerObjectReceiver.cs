using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("TriggerObject Enter Event")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectEnterEvent : DefiniteEventNode<TriggerInfo>
{
    public static string name = "TriggerObjectEnter";
    public override string eventName => name;
    public static void Trigger(GameObject gameObject, string id, TriggerInfo info)
    {
        EventBus.Trigger(DefiniteEventNode.GetEventName(name, id), gameObject, info);
    }
}

[UnitTitle("TriggerObject Stay Event")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectStayEvent : DefiniteEventNode<TriggerInfo>
{
    public static string name = "TriggerObjectStay";
    public override string eventName => name;
    public static void Trigger(GameObject gameObject, string id, TriggerInfo info)
    {
        EventBus.Trigger(DefiniteEventNode.GetEventName(name, id), gameObject, info);
    }
}

[UnitTitle("TriggerObject Exit Event")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectExitEvent : DefiniteEventNode<TriggerInfo>
{
    public static string name = "TriggerObjectExit";
    public override string eventName => name;
    public static void Trigger(GameObject gameObject, string id, TriggerInfo info)
    {
        EventBus.Trigger(DefiniteEventNode.GetEventName(name, id), gameObject, info);
    }
}