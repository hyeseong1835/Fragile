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
        swing_endEvent.AddListener(SwingEndEvent);
    }
    public override void Attack()
    {
        swing_obj.gameObject.SetActive(true);
        con.grafic.HandLink(HandMode.ToTarget, swing_obj.transform);

        StartCoroutine(Skill.Swing(con, swing_obj, 
            Utility.GetTargetAngle(con.transform.position, con.targetPos), 
            swing_spread, swing_duration, Skill.Curve.Quadratic,
            enterEvent: swing_enterEvent, endEvent: swing_endEvent)
            );
    }
    public void SwingHitEvent(GameObject triggerObj, Collider2D coll)
    {
        if (coll.gameObject.layer == 20)
        {
            coll.GetComponent<Stat>().OnDamage(swing_damage * damage);
        }
        else if (coll.gameObject.layer == 21)
        {
            coll.GetComponent<Stat>().OnDamage(swing_damage * damage);
        }
    }
    public void SwingEndEvent(GameObject triggerObj) 
    {
        swing_obj.gameObject.SetActive(false);

        hand_obj.gameObject.SetActive(true);
        con.grafic.HandLink(HandMode.ToHand, hand_obj);
    }
}
