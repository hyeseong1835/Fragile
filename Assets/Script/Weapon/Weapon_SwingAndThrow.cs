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
    UnityEvent<GameObject, Collider2D> swing_enterEvent = new UnityEvent<GameObject, Collider2D>();
    Skill_Swing swing;

    [Title("Throw")]
    [SerializeField] TriggerObject throw_obj;
    [SerializeField] float throw_damage;
    [SerializeField] float throw_throwSpeed;
    [SerializeField] float throw_spinSpeed;
    [SerializeField] float throw_duration;
    UnityEvent<GameObject, Collider2D> throw_enterEvent = new UnityEvent<GameObject, Collider2D>();
    Skill_Throw @throw;

    UnityEvent<GameObject> throw_endEvent = new UnityEvent<GameObject>();
    void Awake()
    {
        swing = GetComponent<Skill_Swing>();
        @throw = GetComponent<Skill_Throw>();

        swing_enterEvent.AddListener(SwingHitEvent);
        throw_enterEvent.AddListener(ThrowHitEvent);
        throw_endEvent.AddListener(EndEvent);
    }
    public override void Attack()
    {
        StartCoroutine(swing.Swing(swing_obj, swing_spread, swing_duration, Skill_Swing.Curve.Quadratic, enterEvent: swing_enterEvent));
    }
    public override void Mouse1Down() 
    {
        StartCoroutine (@throw.Throw(throw_obj, throw_spinSpeed, throw_throwSpeed, throw_duration, enterEvent: throw_enterEvent, endEvent: throw_endEvent));
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
    public void ThrowHitEvent(GameObject triggerObj, Collider2D coll)
    {
        if (coll.gameObject.layer == 20)
        {
            coll.GetComponent<Stat>().OnDamage(throw_damage * damage);
        }
        else if (coll.gameObject.layer == 21)
        {
            coll.GetComponent<Stat>().OnDamage(throw_damage * damage);
        }
    }
    public void EndEvent(GameObject triggerObj)
    {
        Destroy();
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
        if (throw_obj.gameObject.activeInHierarchy)
        {
            breakParticle.SpawnParticle(throw_obj.transform.position, throw_obj.transform.rotation);
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