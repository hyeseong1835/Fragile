using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Skill_Swing))]
public class Weapon_Hand : Weapon
{
    Skill_Swing swing;

    [SerializeField] Transform hand_obj;
    [SerializeField] TriggerObject swing_obj;
    [SerializeField] float swing_damage;
    [SerializeField] float swing_spread;
    [SerializeField] float swing_duration;
    UnityEvent<GameObject, Collider2D> swing_enterEvent = new UnityEvent<GameObject, Collider2D>();
    UnityEvent<GameObject> swing_endEvent = new UnityEvent<GameObject>();


    void Awake()
    {
        swing = GetComponent<Skill_Swing>();

        swing_enterEvent.AddListener(SwingHitEvent);
        swing_endEvent.AddListener(SwingEndEvent);
    }
    public override void Attack()
    {
        Player.grafic.HandLink(swing_obj.transform, true);
        handWeapon.gameObject.SetActive(false);
        StartCoroutine(swing.Swing(swing_obj, swing_spread, swing_duration, Skill_Swing.Curve.Quadratic,
            enterEvent: swing_enterEvent, endEvent: swing_endEvent));
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
        Player.grafic.HandLink(handWeapon, false);
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
