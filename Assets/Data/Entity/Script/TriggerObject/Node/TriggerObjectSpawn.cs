using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("TriggerObject Spawn")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectSpawn : Node
{
    public ValueInputPort<GameObject> prefabValueInputPort;
    public ValueInputPort<string> IDValueInputPort;

    [DoNotSerialize] public ValueOutput Ov_triggerObject;

    protected override void Definition()
    {
        base.Definition();
        
        prefabValueInputPort = new ValueInputPort<GameObject>(this, "Prefab", true);

        Ov_triggerObject = ValueOutput<TriggerObject>("TriggerObject");
    }
    protected override ControlOutput Act(Flow flow)
    {
        TriggerObject triggerObject = GameObject.Instantiate(
                    prefabValueInputPort.GetValue(flow),
                    flow.stack.gameObject.transform
                    ).AddComponent<TriggerObject>();
        flow.SetValue(Ov_triggerObject, triggerObject);
        triggerObject.Set(flow.stack.gameObject, IDValueInputPort.GetValue(flow));

        return Out;
    }
}