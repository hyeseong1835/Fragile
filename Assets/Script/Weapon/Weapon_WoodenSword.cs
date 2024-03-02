using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Weapon_WoodenSword : Weapon
{
    [Space(25)]
    [Title("Swing")]
    [SerializeField] TriggerObject swing_obj;
    [SerializeField] float swing_damage;
    [SerializeField] float swing_spread;
    [SerializeField] float swing_duration;
    [SerializeField] UnityEvent<Transform, Collider2D> swing_enterEvent;
    Skill_Swing swing;

    [Title("Spin")]
    [SerializeField] TriggerObject spin_obj;
    [SerializeField] float spin_damage;
    [SerializeField] float spin_throwSpeed;
    [SerializeField] float spin_spinSpeed;
    [SerializeField] float spin_duration;
    [SerializeField] UnityEvent<Transform, Collider2D> spin_enterEvent;
    Skill_Spin spin;

    void Awake()
    {
        swing = GetComponent<Skill_Swing>();
        spin = GetComponent<Skill_Spin>();
    }
    public override void AttackDown()
    {
        StartCoroutine(swing.Swing(swing_obj, swing_spread, swing_duration, Skill_Swing.Curve.Quadratic, swing_enterEvent));
    }
    public override void Mouse1Down() 
    {
        StartCoroutine (spin.Spin(spin_obj, spin_spinSpeed, spin_throwSpeed, spin_duration, spin_enterEvent));
    }
    public void SwingHitEvent(Transform transform, Collider2D coll)
    {
        if (coll.gameObject.layer == 20)
        {
            coll.GetComponent<Stat>().OnDamage(swing_damage * damage);
            AddDurability(-1);
        }
        else if (coll.gameObject.layer == 21)
        {
            coll.GetComponent<Stat>().OnDamage(swing_damage * damage);
            AddDurability(-1);
        }
    }
    public void SpinHitEvent(Transform transform, Collider2D coll)
    {
        if (coll.gameObject.layer == 20)
        {
            coll.GetComponent<Stat>().OnDamage(spin_damage * damage);
            AddDurability(-1);
        }
        else if (coll.gameObject.layer == 21)
        {
            coll.GetComponent<Stat>().OnDamage(spin_damage * damage);
            AddDurability(-1);
        }
    }
}