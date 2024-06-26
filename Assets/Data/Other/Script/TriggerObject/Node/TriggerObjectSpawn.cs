using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("TriggerObject Spawn")]
[UnitCategory("Events/Weapon")]
public class TriggerObjectSpawn : ActNode
{
    public ValueInputPort<GameObject> prefabPort;
    public ValueInputPort<string> IDPort;
    
    [DoNotSerialize] public ValueOutputPort<TriggerObject> triggerObjectPort;

    protected override void Definition()
    {
        base.Definition();

        prefabPort = ValueInputPort<GameObject>.Define(this, "Prefab");
        IDPort = ValueInputPort<string>.Define(this, "ID");

        triggerObjectPort = ValueOutputPort<TriggerObject>.Define(this);
    }
    protected override void Act(Flow flow)
    {
        GameObject gameObject = flow.stack.gameObject;
        GameObject prefab = prefabPort.GetValue(flow);
        string ID = IDPort.GetValue(flow);

        TriggerObject triggerObject = GameObject.Instantiate(
                    prefab,
                    gameObject.transform
                    ).GetComponent<TriggerObject>();
        triggerObject.gameObject.name = ID;
        triggerObject.gameObject.SetActive(false);

        triggerObjectPort.SetValue(flow, triggerObject);
        triggerObject.Set(gameObject, ID);
    }
}