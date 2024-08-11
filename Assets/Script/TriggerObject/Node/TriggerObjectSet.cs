using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[UnitTitle("TriggerObject Set")]
[UnitCategory("Weapon/TriggerObject")]
public class TriggerObjectSet : GetComponentActNode<Weapon>
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
        TriggerObject obj = triggerObjectValueInputPort.GetValue(flow);

        if(setValueInput == SetType.Enable)
        {
            obj.gameObject.SetActive(true);

            if (linkHandValueInputPort.GetValue(flow))
            {
                component.hand_obj.gameObject.SetActive(false);
                component.con.hand.HandLink(obj.transform, HandMode.ToTarget);
            }
        }
        else
        {
            obj.gameObject.SetActive(false);

            if (linkHandValueInputPort.GetValue(flow))
            {
                component.hand_obj.gameObject.SetActive(true);
                component.con.hand.HandLink(component.hand_obj, HandMode.ToHand);
            }
        }
    }
}