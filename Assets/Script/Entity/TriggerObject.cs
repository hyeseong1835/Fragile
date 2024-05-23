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
        Self, Friend, Enemy, Object
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
[UnitTitle("TriggerObjectEvent")]
[UnitCategory("Events/Weapon/Action")]
public class TriggerObjectUnit : Unit
{
    GraphReference reference;

    GameObject gameObject;
    Weapon weapon;

    public ControlInput In;
    public ValueInput triggerObject;

    public ControlOutput Out;
    public ControlOutput Enter;
    public ControlOutput Stay;
    public ControlOutput Exit;
    public ValueOutput coll;
    public ValueOutput triggerType;

    protected override void Definition()
    {
        In = ControlInput("In", (flow) =>
        {
            gameObject = flow.stack.gameObject;
            reference = GraphReference.New(flow.stack.machine, true);
            if (weapon == null) weapon = gameObject.GetComponent<Weapon>();
            return Out;
        });
        triggerObject = ValueInput<TriggerObject>("Trigger");
        
        Out = ControlOutput("Out");
        Enter = ControlOutput("Enter");
        Stay = ControlOutput("Stay");
        Exit = ControlOutput("Exit");
        coll = ValueOutput<Collider2D>("Collider");
        triggerType = ValueOutput<TriggerObject.TriggerType>("Type");
    }
    public void OnTriggerEnter(Collider2D coll)
    {
        Flow flow = Flow.New(reference);

        flow.Run(Enter);
    }
    public void OnTriggerStay(Collider2D coll)
    {
        Flow flow = Flow.New(reference);

        flow.Run(Stay);
    }
    public void OnTriggerExit(Collider2D coll)
    {
        Flow flow = Flow.New(reference);

        flow.Run(Exit);
    }
}