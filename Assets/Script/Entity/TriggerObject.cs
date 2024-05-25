using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;
using static UnityEngine.Tilemaps.Tile;
using Unity.Burst.CompilerServices;

public enum TriggerType
{
    Self, Friend, Enemy, Object
}
public class TriggerObject : MonoBehaviour
{
    public static TriggerObject Spawn(GameObject obj)
    {
        TriggerObject triggerObject = new GameObject().AddComponent<TriggerObject>();
        triggerObject.returnObject = obj;
        triggerObject.returnObjectCon = obj.GetComponent<Controller>();

        return triggerObject;
    }
    GameObject returnObject;
    Controller returnObjectCon;
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        EventBus.Trigger("TriggerObjectEnter", returnObject, GetTriggerType(coll));
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        EventBus.Trigger("TriggerObjectStay", returnObject, GetTriggerType(coll));
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        EventBus.Trigger("TriggerObjectExit", returnObject, GetTriggerType(coll));
    }
    TriggerType GetTriggerType(Collider2D _coll)
    {
        Controller hitCon = _coll.GetComponent<Controller>();
        if (hitCon == null)
        {
            return TriggerType.Object;
        }
        else if (hitCon == returnObjectCon)
        {
            return TriggerType.Self;
        }
        else if (hitCon.gameObject.layer == hitCon.gameObject.layer)
        {
            return TriggerType.Friend;
        }
        else return TriggerType.Enemy;
    }
}

[UnitTitle("TriggerObjectEnter")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectEnterEvent : DefiniteEventNode<TriggerType>
{
    public override string eventName => "TriggerObjectEnter";
}

[UnitTitle("TriggerObjectStay")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectStayEvent : DefiniteEventNode<TriggerType>
{
    public override string eventName => "TriggerObjectStay";
}

[UnitTitle("TriggerObjectExit")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectExitEvent : DefiniteEventNode<TriggerType>
{
    public override string eventName => "TriggerObjectExit";
}