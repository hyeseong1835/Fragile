using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("TriggerObject Set")]
[UnitCategory("Weapon/TriggerObject")]
public class TriggerObjectSet : WeaponNode
{
    public enum SetType
    {
        Enable, Disable
    }
    [Serialize, Inspectable, UnitHeaderInspectable]
    public SetType setValueInput;

    ValueInputPort<TriggerObject> triggerObjectValueInputPort;
    ValueInputPort<bool> linkHandValueInputPort;

    protected override void Definition()
    {
        base.Definition();
        
        triggerObjectValueInputPort = ValueInputPort<TriggerObject>.Define(this, "TriggerObject");
        linkHandValueInputPort = ValueInputPort<bool>.Define(this, "HandLink", true);
    }
    protected override void Act(Flow flow)
    {
        base.Act(flow);

        TriggerObject obj = triggerObjectValueInputPort.GetValue(flow);

        if(setValueInput == SetType.Enable)
        {
            obj.gameObject.SetActive(true);

            if (linkHandValueInputPort.GetValue(flow))
            {
                weapon.hand_obj.gameObject.SetActive(false);
                weapon.con.hand.HandLink(obj.transform, HandMode.ToTarget);
            }
        }
        else
        {
            obj.gameObject.SetActive(false);

            if (linkHandValueInputPort.GetValue(flow))
            {
                weapon.hand_obj.gameObject.SetActive(true);
                weapon.con.hand.HandLink(weapon.hand_obj, HandMode.ToTarget);
            }
        }
        
    }
}