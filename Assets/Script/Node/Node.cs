using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Node : Unit
{
    public ControlInput In;

    public ControlOutput Out;

    protected override void Definition()
    {
        In = ControlInput(string.Empty, (flow) =>
        {
            Act(flow);
            return Out;
        });
        Out = ControlOutput(string.Empty);
    }
    protected abstract void Act(Flow flow);
}
