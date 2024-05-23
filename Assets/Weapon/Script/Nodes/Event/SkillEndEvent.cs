using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;


public abstract class SkillEndEvent : Unit
{
    [DoNotSerialize][NullMeansSelf] public ValueInput target;
    [DoNotSerialize] public ControlInput In;

    public Weapon weapon { get; private set; }
    protected abstract Action End { get; }

    protected override void Definition()
    {
        target = ValueInput<Weapon>("", null).NullMeansSelf();
        In = ControlInput("Time", (flow) => {
            if (weapon == null) weapon = flow.GetValue<Weapon>(target);
            End();
            return default;
        });
    }
}
[UnitTitle("Attack")]
[UnitCategory("Events/WeaponEvents")]
public class AttackEndEvent : SkillEndEvent
{
    protected override Action End => weapon.con.EndAttack;
}
[UnitTitle("Special")]
[UnitCategory("Events/WeaponEvents")]
public class SpecialEndEvent : SkillEndEvent
{
    protected override Action End => weapon.con.EndAttack;
}