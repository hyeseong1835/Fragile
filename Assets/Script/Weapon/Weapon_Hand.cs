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
    protected override void OnUse()
    {
        con.hand.HandLink(null);
    }
    public override void Attack()
    {
        swing_obj.gameObject.SetActive(true);
        con.hand.HandLink(swing_obj.transform, HandMode.ToTarget);

        StartCoroutine(Skill.Swing(con, swing_obj, 
            swing_spread, swing_duration, Skill.SwingCurve.Quadratic,
            enterEvent: swing_enterEvent)
            );
    }
    public void SwingHitEvent(GameObject triggerObj, Collider2D coll)
    {
        if (coll.gameObject.layer == 20)
        {
            coll.GetComponent<Controller>().OnDamage(swing_damage * damage);
        }
        else if (coll.gameObject.layer == 21)
        {
            coll.GetComponent<Controller>().OnDamage(swing_damage * damage);
        }
    }
    public void SwingEndEvent(GameObject triggerObj) 
    {
        swing_obj.gameObject.SetActive(false);
        con.hand.HandLink(null);
    }
    public override void Special()
    {

    }
}
