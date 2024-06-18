using Unity.VisualScripting;
using UnityEngine;

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