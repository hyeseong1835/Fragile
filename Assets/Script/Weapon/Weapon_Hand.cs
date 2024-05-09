using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Weapon_Hand : Weapon
{
    [Space(Editor.overrideSpace)]
    [BoxGroup("Object")]
    #region Override Box Object - - - - - - - - - - - - - -|

        [SerializeField][Required][ChildGameObjectsOnly]//-|
        [LabelWidth(Editor.propertyLabelWidth)]
        TriggerObject swing_obj;

    #endregion  - - - - - - - - - - - - - - - - - - - - - -|

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

    protected override void WeaponAwake()
    {
        swing_enterEvent.AddListener(SwingHitEvent);
    }
    protected override void OnUse()
    {
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

        swing_obj.gameObject.SetActive(true);
        con.hand.HandLink(swing_obj.transform, HandMode.ToTarget);

        StartCoroutine(Skill.Swing(con, swing_obj,
            swing_spread, attackDelay, Skill.SwingCurve.Linear,
            enterEvent: swing_enterEvent)
            );
        yield return new WaitForSeconds(attackDelay);

        yield return new WaitForSeconds(attackBackDelay);

        if (state == WeaponState.REMOVED) Destroy();

        swing_obj.gameObject.SetActive(false);

        con.hand.HandLink(null);
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

    public override void Special()
    {

    }
}
