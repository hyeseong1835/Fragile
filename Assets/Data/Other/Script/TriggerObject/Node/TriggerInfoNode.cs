using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("Expose TriggerInfo")]
[UnitCategory("Weapon")]
public class TriggerInfoNode : Unit
{
    ValueInputPort<TriggerInfo> triggerInfoIn;

    ValueOutputPort<Entity> entityOut;
    ValueOutputPort<TriggerType> triggerTypeOut;

    protected override void Definition()
    {
        triggerInfoIn = ValueInputPort<TriggerInfo>.Define(this, "Info");
        entityOut = ValueOutputPort<Entity>.Define(this, "Entity", (flow) => triggerInfoIn.GetValue(flow).entity);
        triggerTypeOut = ValueOutputPort<TriggerType>.Define(this, "Type", (flow) => triggerInfoIn.GetValue(flow).type);
    }
}
