using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Weapon_SwingAndThrow : Weapon
{
    [SerializeField] Transform hand_obj;

    [Space(25)]
    [Title("Swing")]
    [SerializeField] TriggerObject swing_obj;
    [SerializeField] float swing_damage;
    [SerializeField] float swing_spread;

    [SerializeField] Skill.SwingCurve swing_curve;
    UnityEvent<GameObject, Collider2D> swing_enterEvent = new UnityEvent<GameObject, Collider2D>();

    [Title("Throw")]
    [SerializeField] TriggerObject throw_obj;
    [SerializeField] float throw_damage;
    [SerializeField] float throw_throwSpeed;
    [SerializeField] float throw_spinSpeed;
    UnityEvent<GameObject, Collider2D> throw_enterEvent = new UnityEvent<GameObject, Collider2D>();


    protected override void WeaponAwake()
    {
        //Swing
        swing_enterEvent.AddListener(SwingHitEvent);

        //Throw
        throw_enterEvent.AddListener(ThrowHitEvent);
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

    public override void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }
    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackFrontDelay);
        hand_obj.gameObject.SetActive(false);

        swing_obj.gameObject.SetActive(true);
        con.hand.HandLink(swing_obj.transform, HandMode.ToTarget);

        StartCoroutine(Skill.Swing(con, swing_obj,
            swing_spread, attackDelay, swing_curve,
            enterEvent: swing_enterEvent)
            );
        yield return new WaitForSeconds(attackBackDelay);

        if (state == WeaponState.REMOVED) Destroy();

        swing_obj.gameObject.SetActive(false);

        hand_obj.gameObject.SetActive(true);
        con.hand.HandLink(hand_obj, HandMode.ToHand);
    }
    public void SwingHitEvent(GameObject triggerObj, Collider2D coll)
    {
        if (coll.gameObject.layer == 20)
        {
            coll.GetComponent<Controller>().TakeDamage(swing_damage * damage);
            AddDurability(-1);
        }
        else if (coll.gameObject.layer == 21)
        {
            coll.GetComponent<Controller>().TakeDamage(swing_damage * damage);
            AddDurability(-1);
        }
    }

    #endregion

    #region Special

    public override void Special() 
        {
            con.RemoveWeapon(this);
            StartCoroutine(Skill.Throw(con, throw_obj, 
                throw_spinSpeed, throw_throwSpeed, specialDelay, 
                enterEvent: throw_enterEvent)
                );
        }
        public void ThrowHitEvent(GameObject triggerObj, Collider2D coll)
        {
            if (coll.gameObject.layer == 20)
            {
                coll.GetComponent<Controller>().TakeDamage(throw_damage * damage);
            }
            else if (coll.gameObject.layer == 21)
            {
                coll.GetComponent<Controller>().TakeDamage(throw_damage * damage);
            }
        }
        public void ThrowEndEvent(GameObject triggerObj)
        {
            Destroy(throw_obj.gameObject);

            Destroy();
        }

    #endregion

    protected override void Break()
    {
        if (swing_obj.gameObject.activeInHierarchy)
        {
            con.hand.HandLink(null);

            breakParticle.SpawnParticle(swing_obj.transform.position, swing_obj.transform.rotation);
            con.RemoveWeapon(this);
            Destroy();
            return;
        }
        if (throw_obj.gameObject.activeInHierarchy)
        {
            breakParticle.SpawnParticle(throw_obj.transform.position, throw_obj.transform.rotation);
            con.RemoveWeapon(this);
            Destroy();
            return;
        }
    }
}