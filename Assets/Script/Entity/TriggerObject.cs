using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;

public class TriggerObject : MonoBehaviour
{
    public static TriggerObject Spawn()
    {
        GameObject obj = new GameObject();
        TriggerObject triggerObject = obj.AddComponent<TriggerObject>();

        return triggerObject;
    }
    TriggerObjectUnit unit;

    public enum TriggerType
    {
        None, Enter, Stay, Exit
    }
    public enum ColliderType
    {
        None, Self, Friend, Enemy, Object
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        unit.OnTriggerEnter(coll);
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        unit.OnTriggerStay(coll);
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        unit.OnTriggerExit(coll);
    }
}
[UnitTitle("TriggerObjectOut")]
[UnitCategory("Weapon")]
public class TriggerObjectUnit : Unit
{
    Flow flow;
    GameObject gameObject;
    Weapon weapon;
    Controller con;

    public ControlInput In;
    public ValueInput triggerObject;

    public ControlOutput Out;
    public ControlOutput Hit;

    public ValueOutput coll;
    public ValueOutput triggerType;
    public ValueOutput colliderType;

    protected override void Definition()
    {
        In = ControlInput(String.Empty, (_flow) =>
        {
            flow = _flow;
            gameObject = _flow.stack.gameObject;
            if (weapon == null) weapon = gameObject.GetComponent<Weapon>();
            return Out;
        });
        triggerObject = ValueInput<TriggerObject>("Trigger");
        
        Out = ControlOutput(String.Empty);
        Hit = ControlOutput("Hit");

        coll = ValueOutput<Collider2D>("Collider");
        triggerType = ValueOutput<TriggerObject.TriggerType>("TriggerType");
        colliderType = ValueOutput<TriggerObject.ColliderType>("ColliderType");
    }
    public void OnTriggerEnter(Collider2D coll) { SendEvent(coll, TriggerObject.TriggerType.Enter); }
    public void OnTriggerStay(Collider2D coll) { SendEvent(coll, TriggerObject.TriggerType.Stay); }
    public void OnTriggerExit(Collider2D coll) { SendEvent(coll,TriggerObject.TriggerType.Exit); }
    void SendEvent(Collider2D _coll, TriggerObject.TriggerType _triggerType)
    {
        flow.SetValue(coll, _coll);

        Controller hitCon = _coll.GetComponent<Controller>();
        if (hitCon == null)
        {
            flow.SetValue(colliderType, TriggerObject.ColliderType.Object);
        }
        else if (hitCon == con)
        {
            flow.SetValue(colliderType, TriggerObject.ColliderType.Self);
        }
        else if (hitCon.gameObject.layer == hitCon.gameObject.layer)
        {
            flow.SetValue(colliderType, TriggerObject.ColliderType.Friend);
        }
        else flow.SetValue(colliderType, TriggerObject.ColliderType.Enemy);

        flow.SetValue(triggerType, _triggerType);

        flow.Run(Hit);

        flow.SetValue(colliderType, TriggerObject.ColliderType.None);
        flow.SetValue(triggerType, TriggerObject.TriggerType.None);
    }
}