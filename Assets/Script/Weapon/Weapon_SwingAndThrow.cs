using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Weapon_SwingAndThrow : Weapon
{
    [Space(Editor.overrideSpace)]
    [BoxGroup("Object")]
    #region Override Box Object  - - - - - - - - - - - - - - - - - - - - - - - - - -|

        [SerializeField][ChildGameObjectsOnly]
        [LabelWidth(Editor.propertyLabelWidth - Editor.childGameObjectOnlyWidth)]//-|
        BreakParticle breakParticle;
                                                                                     [BoxGroup("Object")]
        [SerializeField][Required][ChildGameObjectsOnly]
        [LabelWidth(Editor.propertyLabelWidth - Editor.childGameObjectOnlyWidth)]
        TriggerObject swing_obj;
                                                                                     [BoxGroup("Object")]
        [SerializeField][Required][ChildGameObjectsOnly]
        [LabelWidth(Editor.propertyLabelWidth - Editor.childGameObjectOnlyWidth)]
        TriggerObject throw_obj;

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [Space(Editor.overrideSpace)]
    [FoldoutGroup("Attack")]
    #region Override Foldout Attack  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [SerializeField] 
        [LabelWidth(Editor.propertyLabelWidth)]
        float swing_damage;
                                                                                                           [FoldoutGroup("Attack")]
        [SerializeField]
        [LabelWidth(Editor.propertyLabelWidth)]
        float swing_spread;
                                                                                                           [FoldoutGroup("Attack")]
        [SerializeField] 
        [LabelWidth(Editor.propertyLabelWidth)]
        Skill.SwingCurve swing_curve;

        UnityEvent<GameObject, Collider2D> swing_enterEvent = new UnityEvent<GameObject, Collider2D>();//-|

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [Space(Editor.overrideSpace)]
    [FoldoutGroup("Special")]
    #region Override Foldout Special - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

    [SerializeField] 
        [LabelWidth(Editor.propertyLabelWidth)]
        float throw_damage;
                                                                                                           [FoldoutGroup("Special")]
        [SerializeField] 
        [LabelWidth(Editor.propertyLabelWidth)]
        float throw_throwSpeed;
                                                                                                           [FoldoutGroup("Special")]
        [LabelWidth(Editor.propertyLabelWidth)]
        [SerializeField] 
        float throw_spinSpeed;

        UnityEvent<GameObject, Collider2D> throw_enterEvent = new UnityEvent<GameObject, Collider2D>();//-|

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|


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
        
        if (state == WeaponState.Removed) yield break;

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
            WeaponDestroy();
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

    public override void OnWeaponRemoved()
    {
        con.hand.HandLink(null);
        Destroy(hand_obj.gameObject);
        Destroy(swing_obj.gameObject);
    }
    protected override void Break()
    {
        if (swing_obj != null && swing_obj.gameObject.activeInHierarchy)
        {
            if (swing_spread == 0) breakParticle.SpawnParticle(swing_obj.transform.position, swing_obj.transform.rotation);
            else if (swing_spread > 0) breakParticle.SpawnParticle(swing_obj.transform.position, Quaternion.Euler(0, 0, (swing_obj.transform.rotation.z + 90) % 360));
            else breakParticle.SpawnParticle(swing_obj.transform.position, Quaternion.Euler(0, 0, (swing_obj.transform.rotation.z - 90) % 360));
        }
        else if (throw_obj != null && throw_obj.gameObject.activeInHierarchy)
        {
            breakParticle.SpawnParticle(throw_obj.transform.position, throw_obj.transform.rotation);
        }
        WeaponDestroy();
    }
    protected override void OnWeaponDestroyed()
    {
        Destroy(throw_obj.gameObject);
    }
}