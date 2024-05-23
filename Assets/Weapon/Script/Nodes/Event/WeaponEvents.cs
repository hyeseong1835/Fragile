using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("Update")]
[UnitCategory("Events/Weapon")]
public class UpdateEventNode : EventNode<WeaponState>
{
    public override string eventName => "Update";
}

#region Attack Input

    [UnitTitle("Attack Down")]
    [UnitCategory("Events/Weapon/Action")]
    public class AttackDownEventNode : EventNode
    {
        public override string eventName => "AttackDown";
    }

    [UnitTitle("Attack Hold")]
    [UnitCategory("Events/Weapon/Action")]
    public class AttackHoldEventNode : EventNode<float>
    {
        public override string eventName => "AttackHold";
        public override string argumentName => "Time";
    }

    [UnitTitle("Attack Up")]
    [UnitCategory("Events/Weapon/Action")]
    public class AttackUpEventNode : EventNode<float>
    {
        public override string eventName => "AttackUp";
        public override string argumentName => "Time";
    }

#endregion

#region Special Input

    [UnitTitle("Special Down")]
    [UnitCategory("Events/Weapon/Action")]
    public class SpecialDownEventNode : EventNode
    {
        public override string eventName => "SpecialDown";
    }

    [UnitTitle("Special Hold")]
    [UnitCategory("Events/Weapon/Action")]
    public class SpecialHoldEventNode : EventNode<float>
    {
        public override string eventName => "SpecialHold";
        public override string argumentName => "Time";
    }

    [UnitTitle("Special Up")]
    [UnitCategory("Events/Weapon/Action")]
    public class SpecialUpEventNode : EventNode<float>
    {
        public override string eventName => "SpecialUp";
        public override string argumentName => "Time";
    }

#endregion