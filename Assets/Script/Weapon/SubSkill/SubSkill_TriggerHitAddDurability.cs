using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSkill_TriggerHitAddDurability : MonoBehaviour
{
    Weapon weapon;
    Controller con { get { return weapon.con; } }

    [SerializeField] int durability;

    void Awake()
    {
        weapon = GetComponent<Weapon>();
    }
    public void OnHit(TriggerObject triggerObject, Collider2D coll)
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
