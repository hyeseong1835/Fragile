using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Weapon_Hand : Weapon
{
    [SerializeField] TriggerObject swing_obj;
    [SerializeField] float swing_damage;
    [SerializeField] float swing_spread;
    [SerializeField] float swing_duration;
    UnityEvent<GameObject, Collider2D> swing_enterEvent = new UnityEvent<GameObject, Collider2D>();
    UnityEvent<GameObject> swing_endEvent = new UnityEvent<GameObject>();

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
        yield return new WaitForSeconds(attackBackDelay);

        if (state == WeaponState.REMOVED) Destroy();

        swing_obj.gameObject.SetActive(false);

        con.hand.HandLink(null);
    }
    public void SwingHitEvent(GameObject triggerObj, Collider2D coll)
    {
        if (coll.gameObject.layer == 20)
        {
            coll.GetComponent<Controller>().TakeDamage(swing_damage * damage);
        }
        else if (coll.gameObject.layer == 21)
        {
            coll.GetComponent<Controller>().TakeDamage(swing_damage * damage);
        }
    }

    #endregion

    public override void Special()
    {

    }
}
