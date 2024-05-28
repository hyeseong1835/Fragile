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
public abstract class Node<T> : Node
{
    public ValueInput value;
    public virtual string argumentName { get { return typeof(T).Name; } }

    protected override void Definition()
    {
        base.Definition();

        value = ValueInput<T>(argumentName);
    }
}
