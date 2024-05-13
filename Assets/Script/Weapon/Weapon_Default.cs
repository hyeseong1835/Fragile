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
        hand_obj.gameObject.SetActive(true);
        con.hand.HandLink(hand_obj, HandMode.ToHand);
    }
    protected override void OnDeUse()
    {
        hand_obj.gameObject.SetActive(false);
        con.hand.HandLink(null);
    }

    #region Attack

        public virtual void AttackHitEvent(GameObject triggerObj, Collider2D coll)
        {
            if (coll.gameObject.layer == 19)
            {
                if (con is PlayerController) return;

                coll.GetComponent<Controller>().TakeDamage(damage);
                AddDurability(attackUseDurability);
            }
            if (coll.gameObject.layer == 20)
            {
                if (con is EnemyController) return;

                coll.GetComponent<Controller>().TakeDamage(damage);
                AddDurability(attackUseDurability);
            }
            else if (coll.gameObject.layer == 21)
            {
                coll.GetComponent<Controller>().TakeDamage(damage);
                AddDurability(attackUseDurability);
            }
        }

    #endregion

    #region Special

        
        
        IEnumerator SpecialCoroutine()
        {
            con.RemoveWeapon(this);

            yield return new WaitForSeconds(specialDelay);

            WeaponDestroy();
        }
        public void SpecialHitEvent(GameObject triggerObj, Collider2D coll)
        {
            if (coll.gameObject.layer == 19)
            {
                if (con is PlayerController) return;

                coll.GetComponent<Controller>().TakeDamage(damage);
                AddDurability(-1);
            }
            if (coll.gameObject.layer == 20)
            {
                if (con is EnemyController) return;

                coll.GetComponent<Controller>().TakeDamage(damage);
                AddDurability(-1);
            }
            else if (coll.gameObject.layer == 21)
            {
                coll.GetComponent<Controller>().TakeDamage(damage);
                AddDurability(-1);
            }
        }

    #endregion

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