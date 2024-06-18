using System.Collections;
using Unity.VisualScripting;

[UnitTitle("Is Flow")]
[UnitCategory("Logic")]
public class IsFlowNode : Unit
{
    ControlInputPortHasBool inputPort;

    ControlOutputPort outputPort;
    ValueOutputPort<bool> isFlowPort;

    protected override void Definition()
    {
        inputPort = ControlInputPortHasBool.Define(this, "In", (flow) => outputPort.port);

        outputPort = ControlOutputPort.Define(this, "Out");
        isFlowPort = ValueOutputPort<bool>.Define(this, "Is Flow", (flow) => inputPort.isFlow);
    }
}
