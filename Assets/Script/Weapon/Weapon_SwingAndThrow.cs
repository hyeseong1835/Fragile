using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Weapon_SwingAndThrow : Weapon
{
    [Space(25)]
    [Title("Swing")]
    [SerializeField] TriggerObject swing_obj;
    [SerializeField] float swing_damage;
    [SerializeField] float swing_spread;
    [SerializeField] float swing_duration;
    UnityEvent<GameObject, Collider2D> swing_enterEvent = new UnityEvent<GameObject, Collider2D>();
    UnityEvent<GameObject> swing_endEvent = new UnityEvent<GameObject>();

    [Title("Throw")]
    [SerializeField] TriggerObject throw_obj;
    [SerializeField] float throw_damage;
    [SerializeField] float throw_throwSpeed;
    [SerializeField] float throw_spinSpeed;
    [SerializeField] float throw_duration;
    UnityEvent<GameObject, Collider2D> throw_enterEvent = new UnityEvent<GameObject, Collider2D>();
    UnityEvent<GameObject> throw_endEvent = new UnityEvent<GameObject>();

    public override void WeaponAwake()
    {
        //Swing
        swing_enterEvent.AddListener(SwingHitEvent);
        swing_endEvent.AddListener(SwingEndEvent);

        //Throw
        throw_enterEvent.AddListener(ThrowHitEvent);
        throw_endEvent.AddListener(ThrowEndEvent);
    }
    #region Attack
    public override void Attack()
    {
        Player.grafic.HandLink(swing_obj.transform, true); //공격 시작: 손 -> 무기
        handWeapon.gameObject.SetActive(false);
        swing_obj.gameObject.SetActive(true);

        StartCoroutine(Skill.Swing(con.transform, swing_obj, Utility.GetTargetAngle(transform.position, con.targetPos), swing_spread, swing_duration, Skill.Curve.Quadratic, 
            enterEvent: swing_enterEvent, endEvent: swing_endEvent));
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
    public void SwingEndEvent(GameObject triggerObj)
    {
        swing_obj.gameObject.SetActive(false);
        handWeapon.gameObject.SetActive(true);
        Player.grafic.HandLink(handWeapon, false);
    }
    #endregion
    public override void Mouse1Down() 
    {
        Player.grafic.HandLink(null);
        StartCoroutine(Skill.Throw(this, throw_obj, throw_spinSpeed, throw_throwSpeed, throw_duration, 
            enterEvent: throw_enterEvent, endEvent: throw_endEvent));
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
    public void ThrowEndEvent(GameObject triggerObj)
    {
        Destroy();
    }
    protected override void Break()
    {
        if (swing_obj.gameObject.activeInHierarchy)
        {
            Player.grafic.stateHandAnimation = true;

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
    /*
    private void OnDrawGizmos()
    {
        if (swing_obj.gameObject.activeInHierarchy)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(swing_obj.transform.position);
            Gizmos.
        }
    }
*/
}