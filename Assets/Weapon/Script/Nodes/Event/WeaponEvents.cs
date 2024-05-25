using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("Update")]
[UnitCategory("Events/Weapon")]
public class UpdateEventNode : GameObjectEventNode<WeaponState>
{
    public static string name = "Update";
    public static void Trigger(GameObject gameObject, WeaponState state)
    {
        EventBus.Trigger(name, gameObject, state);
    }

    public override string eventName => name;
}

#region Attack Input

[UnitTitle("Attack Down")]
[UnitCategory("Events/Weapon/Action")]
public class AttackDownEventNode : GameObjectEventNode
{
    public static string name = "AttackDown";
    public static void Trigger(GameObject gameObject)
    {
        EventBus.Trigger(name, gameObject, -1);
    }

    public override string eventName => name;
}

[UnitTitle("Attack Hold")]
[UnitCategory("Events/Weapon/Action")]
public class AttackHoldEventNode : GameObjectEventNode<float>
{
    public static string name = "AttackHold";
    public static void Trigger(GameObject gameObject, float time)
    {
        EventBus.Trigger(name, gameObject, time);
    }

    public override string eventName => name;
    public override string argumentName => "Time";
}

[UnitTitle("Attack Up")]
[UnitCategory("Events/Weapon/Action")]
public class AttackUpEventNode : GameObjectEventNode<float>
{
    public static string name = "AttackUp";
    public static void Trigger(GameObject gameObject, float time)
    {
        EventBus.Trigger(name, gameObject, time);
    }

    public override string eventName => name;
    public override string argumentName => "Time";
}

#endregion

#region Special Input

[UnitTitle("Special Down")]
[UnitCategory("Events/Weapon/Action")]
public class SpecialDownEventNode : GameObjectEventNode
{
    public static string name = "SpecialDown";
    public static void Trigger(GameObject gameObject)
    {
        EventBus.Trigger(name, gameObject, -1);
    }

    public override string eventName => name;
}

[UnitTitle("Special Hold")]
[UnitCategory("Events/Weapon/Action")]
public class SpecialHoldEventNode : GameObjectEventNode<float>
{
    public static string name = "SpecialHold";
    public static void Trigger(GameObject gameObject, float time)
    {
        EventBus.Trigger(name, gameObject, time);
    }
    
    public override string eventName => name;
    public override string argumentName => "Time";
}

[UnitTitle("Special Up")]
[UnitCategory("Events/Weapon/Action")]
public class SpecialUpEventNode : GameObjectEventNode<float>
{
    public static string name = "SpecialUp";
    public override string eventName => name;
    public static void Trigger(GameObject gameObject, float time)
    {
        EventBus.Trigger(name, gameObject, time);
    }

    public override string argumentName => "Time";
}

#endregion