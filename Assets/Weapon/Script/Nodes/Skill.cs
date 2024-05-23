using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Skill : Unit
{
    GameObject gameObject;
    Weapon weapon;

    [DoNotSerialize][NullMeansSelf] public ValueInput target;
    [DoNotSerialize] public ControlInput In;
    [DoNotSerialize] public ControlOutput Out;

    protected override void Definition()
    {
        target = ValueInput<Weapon>("", null).NullMeansSelf();
        In = ControlInput("Time", (flow) => {
            gameObject = flow.stack.gameObject;
            if (weapon == null) weapon = gameObject.GetComponent<Weapon>();
            return default;
        });
    }
}
public abstract class Skill<TData> : Skill
{

}