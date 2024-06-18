using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("Flow And")]
[UnitCategory("Control")]
public class FlowAndNode : Unit
{
    DetectedControlInputPort inputPortA;
    DetectedControlInputPort inputPortB;

    ControlOutputPort truePort;
    ControlOutputPort outputPortA;
    ControlOutputPort outputPortB;

    protected override void Definition()
    {
        inputPortA = DetectedControlInputPort.Define(this, "In A", (flow) => Act(flow, inputPortA, outputPortA));
        inputPortB = DetectedControlInputPort.Define(this, "In B", (flow) => Act(flow, inputPortB, outputPortB));

        truePort = ControlOutputPort.Define(this, "True");
        outputPortA = ControlOutputPort.Define(this, "Out A");
        outputPortB = ControlOutputPort.Define(this, "Out B");

        ControlOutput Act(Flow flow, DetectedControlInputPort input, ControlOutputPort output)
        {
            if (input.isFlow) 
            { 
                GraphReference reference = GraphReference.New(flow.stack.machine, true); 
                truePort.Run(reference); 
            }
            return output.port;
        }
    }
}
