using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_TriggerHitAddDurability : Action, Input_TriggerHit
{
    [SerializeField] int durability;

    public void TriggerHit(TriggerObject triggerObject, Collider2D coll)
    {
        if (coll.gameObject.layer == 19)
        {
            if (con is PlayerController) return;

            weapon.AddDurability(durability);
        }
        if (coll.gameObject.layer == 20)
        {
            if (con is EnemyController) return;

            weapon.AddDurability(durability);
        }
        else if (coll.gameObject.layer == 21)
        {
            weapon.AddDurability(durability);
        }
    }
}
