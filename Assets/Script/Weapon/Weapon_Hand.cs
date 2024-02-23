using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Hand : Weapon
{
    [SerializeField] Skill_Swing swing;
    
    public override void AttackDown()
    {
        StartCoroutine(swing.Swing());
    }
    public void SwingHitEvent(GameObject obj, Collider2D coll)
    {
        if (coll.gameObject.layer == 20)
        {
            coll.GetComponent<Stat>().OnDamage(swing.swingDamage * damage);
        }
        else if (coll.gameObject.layer == 21)
        {
            coll.GetComponent<Stat>().OnDamage(swing.swingDamage * damage);
        }
    }
}
