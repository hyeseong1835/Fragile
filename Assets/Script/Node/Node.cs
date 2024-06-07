using System;
using Unity.VisualScripting;

public abstract class Node : Unit
{
    public ControlInput In;
    public virtual string InArgumentName => string.Empty;

    public ControlOutput Out;
    public virtual string OutArgumentName => string.Empty;

    protected override void Definition()
    {
        In = ControlInput(InArgumentName, Control);

        Out = ControlOutput(OutArgumentName);
    }
    protected virtual ControlOutput Control(Flow flow)
    {
        return Act(flow);
    }
    protected abstract ControlOutput Act(Flow flow);
}

public abstract class CloseNode : Unit
{
    public ControlInput In;
    public virtual string InArgumentName => string.Empty;

    protected override void Definition()
    {
        In = ControlInput(InArgumentName, Control);
    }
    protected virtual ControlOutput Control(Flow flow)
    {
        return Act(flow);
    }
    protected abstract ControlOutput Act(Flow flow);
}