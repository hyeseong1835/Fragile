using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("Add Durability")]
[UnitCategory("Weapon")]
public class AddDurability : GetComponentActNode<Weapon>
{
    ValueInputPort<int> durabilityIn;

    ControlOutputPort breakOut;

    protected override void Definition()
    {
        base.Definition();

        durabilityIn = ValueInputPort<int>.Define(this, "Durability");
        breakOut = ControlOutputPort.Define(this, "Break");
    }
    protected override void Act(Flow flow)
    {
        int durability = durabilityIn.GetValue(flow);

        if (component.AddDurability(durability))
        {
            breakOut.Run(reference);
        }
    }
}