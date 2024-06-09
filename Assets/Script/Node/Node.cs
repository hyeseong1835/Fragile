using System;
using Unity.VisualScripting;

public abstract class ActNode : Unit
{
    public ControlInputPort inputPort;
    public ControlOutputPort outputPort;

    protected override void Definition()
    {
        inputPort.Define(this, (flow) => { Act(flow); return outputPort.port; });
        outputPort.Define(this);
    }
    protected abstract void Act(Flow flow);
}