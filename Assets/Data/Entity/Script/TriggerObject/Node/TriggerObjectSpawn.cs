using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("TriggerObject Spawn")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectSpawn : Unit
{
    public ControlInputPort inputPort;
    public ControlOutputPort outPort;
    public ValueInputPort<GameObject> prefabPort;
    public ValueInputPort<string> IDPort;

    [DoNotSerialize] public ValueOutputPort<TriggerObject> triggerObjectPort;

    protected override void Definition()
    {
        inputPort.Define(this, Act);
        prefabPort.Define(this, "Prefab");

        triggerObjectPort.Define(this);
    }
    ControlOutput Act(Flow flow)
    {
        TriggerObject triggerObject = GameObject.Instantiate(
                    prefabPort.GetValue(flow),
                    flow.stack.gameObject.transform
                    ).AddComponent<TriggerObject>();
        triggerObjectPort.SetValue(flow, triggerObject);
        triggerObject.Set(flow.stack.gameObject, IDPort.GetValue(flow));

        return outPort.port;
    }
}