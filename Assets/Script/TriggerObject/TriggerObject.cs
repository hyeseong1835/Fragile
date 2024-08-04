using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

public enum TriggerType
{
    Self, Friend, Enemy, Object
}
public struct TriggerInfo
{
    public Entity entity;
    public TriggerType type;

    public TriggerInfo(Entity entity, TriggerType type)
    {
        this.entity = entity;
        this.type = type;
    }
}
public class TriggerObject : MonoBehaviour
{
    public static implicit operator GameObject(TriggerObject triggerObject) => triggerObject.gameObject;
    public static implicit operator Transform(TriggerObject triggerObject) => triggerObject.transform;
    public static implicit operator string(TriggerObject triggerObject) => triggerObject.ID;

    [ReadOnly] public GameObject returnObject;
    [ReadOnly] public Controller returnObjectCon;
    [ReadOnly] public string ID;
    //[ReadOnly] public bool eventTrigger = true;

    public void Set(GameObject obj, string id)
    {
        returnObject = obj;
        returnObjectCon = obj.GetComponent<Weapon>().con;
        ID = id;
        //this.eventTrigger = eventTrigger;
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        TriggerObjectEnterEvent.Trigger(returnObject, ID, new TriggerInfo(coll.GetComponent<Entity>(), GetTriggerType(coll)));
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        TriggerObjectStayEvent.Trigger(returnObject, ID, new TriggerInfo(coll.GetComponent<Entity>(), GetTriggerType(coll)));
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        TriggerObjectExitEvent.Trigger(returnObject, ID, new TriggerInfo(coll.GetComponent<Entity>(), GetTriggerType(coll)));
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