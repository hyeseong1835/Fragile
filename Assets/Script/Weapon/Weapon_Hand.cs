using UnityEngine;
using UnityEngine.Events;

public class Weapon_Hand : Weapon
{
    [SerializeField] Transform hand_obj;
    [SerializeField] TriggerObject swing_obj;
    [SerializeField] float swing_damage;
    [SerializeField] float swing_spread;
    [SerializeField] float swing_duration;
    UnityEvent<GameObject, Collider2D> swing_enterEvent = new UnityEvent<GameObject, Collider2D>();
    UnityEvent<GameObject> swing_endEvent = new UnityEvent<GameObject>();

    void Awake()
    {
        swing_enterEvent.AddListener(SwingHitEvent);
        swing_endEvent.AddListener(SwingEndEvent);
    }
    public override void SetData(string[] data)
    {
        
    }
    public override string[] GetData()
    {
        return new string[]
        {

        };
    }
    public override void Attack()
    {
        grafic.HandLink(HandMode.ToTarget, swing_obj.transform);
        handWeapon.gameObject.SetActive(false);

        StartCoroutine(Skill.Swing(con.transform, swing_obj, Utility.GetTargetAngle(con.transform.position, con.targetPos), 
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
        handWeapon.gameObject.SetActive(true);

        grafic.HandLink(HandMode.ToHand, handWeapon);
    }
    protected override void Break()
    {
        Debug.LogError("이게 왜 없어져 미친");
    }
    protected override void OnWeaponRemoved()
    {
        Debug.LogError("이게 왜 없어져 미친");
    }
    protected override void OnWeaponDestroyed()
    {
        Debug.LogError("이게 왜 없어져 미친");
    }
}
