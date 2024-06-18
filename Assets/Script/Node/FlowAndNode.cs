using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("Flow And")]
[UnitCategory("Control")]
public class FlowAndNode : Unit
{
    ControlInputPortHasBool inputPortA;
    ControlInputPortHasBool inputPortB;

    ControlOutputPort truePort;
    ControlOutputPort outputPortA;
    ControlOutputPort outputPortB;

    protected override void Definition()
    {
        inputPortA = ControlInputPortHasBool.Define(this, "In A", (flow) => { if (inputPortA.isFlow) truePort.Run(flow); return outputPortA.port; });
        inputPortB = ControlInputPortHasBool.Define(this, "In B", (flow) => { if (inputPortB.isFlow) truePort.Run(flow); return outputPortB.port; });

        truePort = ControlOutputPort.Define(this, "True");
        outputPortA = ControlOutputPort.Define(this, "Out A");
        outputPortB = ControlOutputPort.Define(this, "Out B");
    }
}
