using Unity.VisualScripting;
using UnityEngine;
using System;

[UnitTitle("Update")]
[UnitCategory("Events/Weapon")]
public class UpdateEventNode : EventNode<WeaponState>
{
    public static string name = "Update";
    public override string eventName => name;
    
    public static void Trigger(GameObject gameObject, WeaponState state)
    {
        EventBus.Trigger(name, gameObject, state);
    }
}

[UnitTitle("DeUse")]
[UnitCategory("Events/Weapon")]
public class DeUseEventNode : EventNode
{
    public static string name = "Break";
    public override string eventName => name;
 
    public static void Trigger(GameObject gameObject)
    {
        EventBus.Trigger(name, gameObject, -1);
    }
}