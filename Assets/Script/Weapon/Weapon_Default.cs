using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Weapon_Default : Weapon
{
    protected override void WeaponAwake()
    {

    }
    protected override void OnUse()
    {
        if(hand_obj != null)
        {
            hand_obj.gameObject.SetActive(true);
            con.hand.HandLink(hand_obj, HandMode.ToHand);
        }
    }
    protected override void OnDeUse()
    {
        if (hand_obj != null)
        {
            hand_obj.gameObject.SetActive(false);
            con.hand.HandLink(null);
        }
    }
    public override void OnWeaponRemoved()
    {
        Destroy(hand_obj.gameObject);
        con.hand.HandLink(null);
    }
    protected override void Break()
    {
        WeaponDestroy();
    }
    protected override void OnWeaponDestroyed()
    {

    }
}