using System.Collections;
using Unity.VisualScripting;

[UnitTitle("Is Flow")]
[UnitCategory("Logic")]
public class IsFlowNode : Unit
{
    DetectedControlInputPort inputPort;

    ControlOutputPort outputPort;
    ValueOutputPort<bool> isFlowPort;

    protected override void Definition()
    {
        inputPort = DetectedControlInputPort.Define(this, "In", (flow) => outputPort.port);

        outputPort = ControlOutputPort.Define(this, "Out");
        isFlowPort = ValueOutputPort<bool>.Define(this, "Is Flow", (flow) => inputPort.isFlow);
    }
}
