using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        Act(flow);

        return null;
    }
    protected virtual void Act(Flow flow) { }
}
public abstract class Node : CloseNode
{
    public ControlOutput Out;
    public virtual string OutArgumentName => string.Empty;


    protected override void Definition()
    {
        base.Definition();
        
        Out = ControlOutput(string.Empty);
    }
    protected override ControlOutput Control(Flow flow)
    {
        base.Control(flow);

        return Out;
    }
}

public abstract class Node<T> : Node
{
    public ValueInput value;
    public virtual string ValueArgumentName => typeof(T).Name;
    public virtual bool ShowField => true;
    public virtual T DefaultValue => default;

    protected override void Definition()
    {
        base.Definition();

        if (ShowField) value = ValueInput<T>(ValueArgumentName, DefaultValue);
        else value = ValueInput<T>(ValueArgumentName);
    }
}
