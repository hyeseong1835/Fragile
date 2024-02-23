using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_WoodenSword : Weapon
{
    [SerializeField] Skill_Swing swing;
    [SerializeField] Skill_Spin spin;
    public override void AttackDown()
    {
        StartCoroutine(swing.Swing());
    }
    public override void Mouse1Down() 
    {
        StartCoroutine (spin.Spin());
    }
    public void SwingHitEvent(GameObject obj, Collider2D coll)
    {
        if (coll.gameObject.layer == 20)
        {
            AddDurability(-1);
            coll.GetComponent<Stat>().OnDamage(swing.swingDamage * damage);
        }
        else if (coll.gameObject.layer == 21)
        {
            AddDurability(-1);
            coll.GetComponent<Stat>().OnDamage(swing.swingDamage * damage);
        }
    }
    public void SpinHitEvent(GameObject obj, Collider2D coll)
    {
        if (coll.gameObject.layer == 20)
        {
            AddDurability(-1);
            coll.GetComponent<Stat>().OnDamage(spin.spinDamage * damage);
        }
        else if (coll.gameObject.layer == 21)
        {
            AddDurability(-1);
            coll.GetComponent<Stat>().OnDamage(swing.swingDamage * damage);
        }
    }
}