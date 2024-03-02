using UnityEngine;
using UnityEngine.Events;

public class Weapon_Hand : Weapon
{
    Skill_Swing swing;

    [SerializeField] TriggerObject swing_obj;
    [SerializeField] float swing_damage;
    [SerializeField] float swing_spread;
    [SerializeField] float swing_duration;
    [SerializeField] UnityEvent<Transform, Collider2D> swing_enterEvent;

    void Awake()
    {
        swing = GetComponent<Skill_Swing>();
    }
    public override void AttackDown()
    {
        StartCoroutine(swing.Swing(swing_obj, swing_spread, swing_duration, Skill_Swing.Curve.Quadratic, swing_enterEvent));
    }
    public void SwingHitEvent(Transform transform, Collider2D coll)
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
}
