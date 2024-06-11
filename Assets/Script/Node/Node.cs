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
        inputPort = ControlInputPort.Define(this, "In" ,(flow) => 
        {
            reference = GraphReference.New(flow.stack.machine, true);

            Act(flow); 
            return outputPort.port; 
        }); 
        
        outputPort = ControlOutputPort.Define(this, "Out");

    }
    protected abstract void Act(Flow flow);
    protected Flow NewFlow() => Flow.New(reference);
}
public abstract class WeaponNode : ActNode
{
    protected Weapon weapon;

    protected override void Act(Flow flow)
    {
        if (weapon == null) weapon = flow.stack.gameObject.GetComponent<Weapon>();
    }
}

public abstract class CoroutineSkillNode : WeaponNode
{
    [DoNotSerialize] protected ControlOutputPort updatePort;
    [DoNotSerialize] protected ControlOutputPort endPort;

    protected override void Definition()
    {
        base.Definition();

        updatePort = ControlOutputPort.Define(this, "Update");
        endPort = ControlOutputPort.Define(this, "End");
    }
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        weapon.StartCoroutine(UseCoroutine(flow));
    }
    protected abstract IEnumerator UseCoroutine(Flow flow);
    protected void Update() => updatePort.Run(reference);
    protected void End() => endPort.Run(reference);
}