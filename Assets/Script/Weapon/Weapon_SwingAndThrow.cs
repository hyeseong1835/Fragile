using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;


public class Weapon_SwingAndThrow : Weapon
{
    [SerializeField] Transform hand_obj;

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


    protected override void WeaponAwake()
    {
        //Swing
        swing_enterEvent.AddListener(SwingHitEvent);
        swing_endEvent.AddListener(SwingEndEvent);

        //Throw
        throw_enterEvent.AddListener(ThrowHitEvent);
        throw_endEvent.AddListener(ThrowEndEvent);
    }
    protected override void OnUse()
    {
        hand_obj.gameObject.SetActive(true);
        con.grafic.hand.HandLink(hand_obj, HandMode.ToHand);
    }
    protected override void OnDeUse()
    {
        hand_obj.gameObject.SetActive(false);
        con.grafic.hand.HandLink(null);
    }

    #region Attack

    public override void Attack()
        {
            hand_obj.gameObject.SetActive(false);

            swing_obj.gameObject.SetActive(true);
            con.grafic.hand.HandLink(swing_obj.transform, HandMode.ToTarget);

            StartCoroutine(Skill.Swing(con, swing_obj, 
                swing_spread, swing_duration, Skill.Curve.Quadratic, 
                enterEvent: swing_enterEvent, endEvent: swing_endEvent)
                );
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
            if (state == WeaponState.REMOVED) Destroy();

            swing_obj.gameObject.SetActive(false);
        
            hand_obj.gameObject.SetActive(true);
            con.grafic.hand.HandLink(hand_obj, HandMode.ToHand);
        }
    
    #endregion

    #region Special

        public override void Mouse1Down() 
        {
            con.RemoveWeapon(this);
            StartCoroutine(Skill.Throw(con, throw_obj, 
                throw_spinSpeed, throw_throwSpeed, throw_duration, 
                enterEvent: throw_enterEvent, endEvent: throw_endEvent)
                );
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
            Destroy(throw_obj.gameObject);

            Destroy();
        }

    #endregion

    protected override void Break()
    {
        if (swing_obj.gameObject.activeInHierarchy)
        {
            con.grafic.hand.HandLink(null);

            breakParticle.SpawnParticle(swing_obj.transform.position, swing_obj.transform.rotation);
            con.RemoveWeapon(this);
            Destroy();
            return;
        }
        if (throw_obj.gameObject.activeInHierarchy)
        {
            breakParticle.SpawnParticle(throw_obj.transform.position, throw_obj.transform.rotation);
            con.RemoveWeapon(this);
            Destroy();
            return;
        }
    }
}