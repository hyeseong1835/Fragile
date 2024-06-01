using UnityEngine;
using Unity.VisualScripting;
using System;
public enum TriggerType
{
    Self, Friend, Enemy, Object
}
public class TriggerObjectData : EntityData
{

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
[UnitTitle("TriggerObject Spawn")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectSpawn : Unit
{
    [DoNotSerialize] public ControlInput In;

    [DoNotSerialize] public ValueInput Iv_prefab;

    [DoNotSerialize] public ValueInput Iv_id;
    
    [DoNotSerialize] public ControlOutput Out;
    
    [DoNotSerialize] public ValueOutput Ov_triggerObject;

    protected override void Definition()
    {
        In = ControlInput(string.Empty, 
            (flow) => {
                TriggerObject triggerObject = GameObject.Instantiate(
                    flow.GetValue<GameObject>(Iv_prefab), 
                    flow.stack.gameObject.transform
                    ).AddComponent<TriggerObject>();
                flow.SetValue(Ov_triggerObject, triggerObject);
                triggerObject.Set(flow.stack.gameObject, flow.GetValue<string>(Iv_id));

                return Out;
            }
        );
        Iv_prefab = ValueInput<GameObject>("Prefab");
        Iv_id = ValueInput<string>("ID");
        
        Out = ControlOutput(string.Empty);
        Ov_triggerObject = ValueOutput<TriggerObject>("TriggerObject");
    }
}

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