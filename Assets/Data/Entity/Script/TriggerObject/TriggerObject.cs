using UnityEngine;
using System;

public enum TriggerType
{
    Self, Friend, Enemy, Object
}
public class TriggerObject : Entity
{
    public override EntityData EntityData
    {
        get => data;
        set { data = (TriggerObjectData)value; }
    }
    public TriggerObjectData data;
    public override Type DataType => typeof(TriggerObjectData);
    

    public GameObject returnObject;
    public Controller returnObjectCon;
    public string ID;

    public void Set(GameObject obj, string id)
    {
        returnObject = obj;
        returnObjectCon = obj.GetComponent<Weapon>().con;
        ID = id;
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        TriggerObjectEnterEvent.Trigger(returnObject, ID, GetTriggerType(coll));
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        TriggerObjectStayEvent.Trigger(returnObject, ID, GetTriggerType(coll));
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        TriggerObjectExitEvent.Trigger(returnObject, ID, GetTriggerType(coll));
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