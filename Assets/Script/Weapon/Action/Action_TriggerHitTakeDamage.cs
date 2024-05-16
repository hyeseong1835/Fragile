using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_TriggerHitTakeDamage : Action
{
    protected override string GetModuleName() { return "TakeDamage"; }


    [SerializeField] float damageMultiply;
    public void OnHit(TriggerObject triggerObject, Collider2D coll)
    {
        if (coll.gameObject.layer == 19)
        {
            if (con is PlayerController) return;

            coll.GetComponent<Controller>().TakeDamage(weapon.damage * damageMultiply);
        }
        if (coll.gameObject.layer == 20)
        {
            if (con is EnemyController) return;

            coll.GetComponent<Controller>().TakeDamage(weapon.damage * damageMultiply);
        }
        else if (coll.gameObject.layer == 21)
        {
            coll.GetComponent<Controller>().TakeDamage(weapon.damage * damageMultiply);
        }
    }
}
