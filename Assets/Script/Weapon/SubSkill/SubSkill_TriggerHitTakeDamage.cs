using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSkill_TriggerHitTakeDamage : MonoBehaviour
{
    Weapon weapon;
    Controller con { get { return weapon.con; } }

    [SerializeField] float damageMultiply;

    void Awake()
    {
        weapon = GetComponent<Weapon>();
    }
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
