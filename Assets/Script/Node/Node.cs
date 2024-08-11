using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ActNode : Unit
{
    protected GraphReference reference;

    protected ControlInputPort inputPort;
    protected ControlOutputPort outputPort;

    protected override void Definition()
    {
        inputPort = ControlInputPort.Define(this, "In" ,(Func<Flow, ControlOutput>)((flow) => 
        {
            reference = GraphReference.New(flow.stack.machine, true);

            Execute(flow);
            Act(flow); 
            return outputPort.port; 
        })); 
        
        outputPort = ControlOutputPort.Define(this, "Out");
    }
    protected virtual void Execute(Flow flow) { }
    protected abstract void Act(Flow flow);
    protected Flow NewFlow() => Flow.New(reference);
}
public abstract class GetComponentActNode<ComponentT> : ActNode where ComponentT : Component
{
    protected ComponentT component;

    protected override void Execute(Flow flow)
    {
        if (component == null) component = flow.stack.gameObject.GetComponent<ComponentT>();
    }
}

public abstract class CoroutineSkillNode : GetComponentActNode<Weapon>
{
    [Inspectable] public bool coroutine = true;

    [DoNotSerialize] protected ControlOutputPort updatePort;
    [DoNotSerialize] protected ControlOutputPort endPort;

    protected override void Definition()
    {
        base.Definition();

        updatePort = ControlOutputPort.Define(this, "Update");
        endPort = ControlOutputPort.Define(this, "End");
    }
    protected override void Execute(Flow flow)
    {
        base.Execute(flow);

        component.StartCoroutine(UseCoroutine(flow));
    }
    protected override void Act(Flow flow) { }

    protected abstract IEnumerator UseCoroutine(Flow flow);
    protected void Update()
    {
        if(coroutine) updatePort.CoroutineRun(reference);
        else updatePort.Run(reference);
    }
    protected void End()
    {
        if (coroutine) endPort.CoroutineRun(reference);
        else endPort.Run(reference);
    }
}