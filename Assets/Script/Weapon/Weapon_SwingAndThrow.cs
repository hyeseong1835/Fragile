using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Weapon_SwingAndThrow : Weapon
{
    [Space(Utility.propertySpace)]
    [FoldoutGroup("Attack")]
    #region Override Foldout Attack - - - - - - - - - - - - - -|

        [SerializeField] 
        [LabelWidth(Utility.propertyLabelWidth)]
        TriggerObject swing_obj;
                                                                [FoldoutGroup("Attack")]
        [SerializeField] 
        [LabelWidth(Utility.propertyLabelWidth)]
        float swing_damage;
                                                                [FoldoutGroup("Attack")]
        [SerializeField] 
        [LabelWidth(Utility.propertyLabelWidth)]
        float swing_spread;
                                                                [FoldoutGroup("Attack")]
        [SerializeField] 
        [LabelWidth(Utility.propertyLabelWidth)]
        Skill.SwingCurve swing_curve;

        UnityEvent<GameObject, Collider2D> swing_enterEvent;//-|

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - -|

    [Space(Utility.propertySpace)]
    [FoldoutGroup("Special")]
    #region Override Foldout Special  - - - - - - - - - - - - -|

        [SerializeField] 
        [LabelWidth(Utility.propertyLabelWidth)]
        TriggerObject throw_obj;
                                                                [FoldoutGroup("Special")]
        [SerializeField] 
        [LabelWidth(Utility.propertyLabelWidth)]
        float throw_damage;
                                                                [FoldoutGroup("Special")]
        [SerializeField] 
        [LabelWidth(Utility.propertyLabelWidth)]
        float throw_throwSpeed;
                                                                [FoldoutGroup("Special")]
        [LabelWidth(Utility.propertyLabelWidth)]
        [SerializeField] 
        float throw_spinSpeed;

        UnityEvent<GameObject, Collider2D> throw_enterEvent;//-|

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - -|


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
        
        swing_obj.gameObject.SetActive(false);

        hand_obj.gameObject.SetActive(true);

        if (state == WeaponState.REMOVED) Destroy();

        swing_obj.gameObject.SetActive(false);

        hand_obj.gameObject.SetActive(true);
        con.hand.HandLink(hand_obj, HandMode.ToHand);
    }
    public void SwingHitEvent(GameObject triggerObj, Collider2D coll)
    {
        if (coll.gameObject.layer == 19)
        {
            if (con is PlayerController) return;

            coll.GetComponent<Controller>().TakeDamage(swing_damage * damage);
            AddDurability(-1);
        }
        if (coll.gameObject.layer == 20)
        {
            if (con is EnemyController) return;

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
            StartCoroutine(SpecialCoroutine());
        }
    IEnumerator SpecialCoroutine()
    {
        con.RemoveWeapon(this);
        StartCoroutine(Skill.Throw(con, throw_obj,
            throw_spinSpeed, throw_throwSpeed, specialDelay,
            enterEvent: throw_enterEvent)
            );
        yield return new WaitForSeconds(specialDelay);

        Destroy(throw_obj.gameObject);
        Destroy();
    }
        public void ThrowHitEvent(GameObject triggerObj, Collider2D coll)
        {
        if (coll.gameObject.layer == 19)
        {
            if (con is PlayerController) return;

            coll.GetComponent<Controller>().TakeDamage(throw_damage * damage);
            AddDurability(-1);
        }
        if (coll.gameObject.layer == 20)
        {
            if (con is EnemyController) return;

            coll.GetComponent<Controller>().TakeDamage(throw_damage * damage);
            AddDurability(-1);
        }
        else if (coll.gameObject.layer == 21)
        {
            coll.GetComponent<Controller>().TakeDamage(throw_damage * damage);
            AddDurability(-1);
        }
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