using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class CoroutineSkill : Unit
{
    [DoNotSerialize] public ControlInput In;
    [DoNotSerialize] public ControlOutput Out;
    [DoNotSerialize] public ControlOutput End;

    public GraphReference reference;

    protected override void Definition()
    {
        In = ControlInput(string.Empty, 
            (flow) => 
            {
                GameObject gameObject = null;
                Weapon weapon = null;

                gameObject = flow.stack.gameObject;
                weapon = gameObject.GetComponent<Weapon>();
                weapon.StartCoroutine(Use(flow, gameObject, weapon));
                reference = GraphReference.New(flow.stack.machine, true);

                return Out;
            }
        );
        Out = ControlOutput(string.Empty);
        End = ControlOutput("End");
    }
    protected abstract IEnumerator Use(Flow flow, GameObject gameObject, Weapon weapon);
    protected void UseEnd() => Flow.New(reference).Run(End);
}