using UnityEngine;
using UnityEngine.Events;

public class Weapon_Hand : Weapon
{
    Skill_Swing swing;

    [SerializeField] TriggerObject swing_obj;
    [SerializeField] float swing_damage;
    [SerializeField] float swing_spread;
    [SerializeField] float swing_duration;
    [SerializeField] UnityEvent<GameObject, Collider2D> swing_enterEvent;

    void Awake()
    {
        swing = GetComponent<Skill_Swing>();
    }
    public override void Attack()
    {
        StartCoroutine(swing.Swing(swing_obj, swing_spread, swing_duration, Skill_Swing.Curve.Quadratic, enterEvent: swing_enterEvent));
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
    protected override void WeaponBreak()
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
