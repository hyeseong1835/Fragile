using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;


public abstract class SkillEndEvent : Unit
{
    [DoNotSerialize] public ControlInput In;
    [DoNotSerialize] public ControlOutput Out;

    protected Weapon weapon;

    protected abstract Action End { get; }

    protected override void Definition()
    {
        In = ControlInput(String.Empty, (flow) => {
            if (weapon == null) weapon = flow.stack.gameObject.GetComponent<Weapon>();
            End();
            return Out;
        });
        Out = ControlOutput(String.Empty);
    }
}
[UnitTitle("Attack End")]
[UnitCategory("Events/Weapon/Action")]
public class AttackEndEvent : SkillEndEvent
{
    protected override Action End => weapon.con.EndAttack;
}
[UnitTitle("Special End")]
[UnitCategory("Events/Weapon/Action")]
public class SpecialEndEvent : SkillEndEvent
{
    protected override Action End => weapon.con.EndAttack;
}