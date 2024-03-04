using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Skill_Swing))]
[RequireComponent(typeof(Skill_Throw))]
public class Weapon_SwingAndThrow : Weapon
{
    [Space(25)]
    [Title("Swing")]
    [SerializeField] TriggerObject swing_obj;
    [SerializeField] float swing_damage;
    [SerializeField] float swing_spread;
    [SerializeField] float swing_duration;
    [SerializeField] UnityEvent<GameObject, Collider2D> swing_enterEvent;
    Skill_Swing swing;

    [Title("Spin")]
    [SerializeField] TriggerObject spin_obj;
    [SerializeField] float spin_damage;
    [SerializeField] float spin_throwSpeed;
    [SerializeField] float spin_spinSpeed;
    [SerializeField] float spin_duration;
    [SerializeField] UnityEvent<GameObject, Collider2D> spin_enterEvent;
    Skill_Throw spin;

    void Awake()
    {
        swing = GetComponent<Skill_Swing>();
        spin = GetComponent<Skill_Throw>();
    }
    public override void Attack()
    {
        StartCoroutine(swing.Swing(swing_obj, swing_spread, swing_duration, Skill_Swing.Curve.Quadratic, enterEvent: swing_enterEvent));
    }
    public override void Mouse1Down() 
    {
        StartCoroutine (spin.Throw(spin_obj, spin_spinSpeed, spin_throwSpeed, spin_duration, enterEvent: spin_enterEvent));
    }
    public void SwingHitEvent(GameObject triggerObj, Collider2D coll)
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
    public void SpinHitEvent(GameObject triggerObj, Collider2D coll)
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
    protected override void WeaponBreak()
    {
        if (swing_obj.gameObject.activeInHierarchy)
        {
            breakParticle.SpawnParticle(swing_obj.transform.position, swing_obj.transform.rotation);
            Remove();
            Destroy();
            return;
        }
        if (spin_obj.gameObject.activeInHierarchy)
        {
            breakParticle.SpawnParticle(spin_obj.transform.position, spin_obj.transform.rotation);
            Remove();
            Destroy();
            return;
        }
    }
    protected override void OnWeaponRemoved()
    {

    }
    protected override void OnWeaponDestroyed()
    {

    }
}