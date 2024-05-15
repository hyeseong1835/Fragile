using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Skill: MonoBehaviour
{
    protected Weapon weapon;
    protected Controller con { get { return weapon.con; } }

    void Awake()
    {
        weapon = gameObject.GetComponent<Weapon>();
        Init();
    }
    void Update()
    {
        SkillUpdate();
    }
    protected abstract void Init();
    public abstract void Execute();
    public virtual void SkillUpdate() { }
    public virtual void OnUseUpdate() { }
    public virtual void DeUseUpdate() { }
    public virtual void OnUse() { }
    public virtual void DeUse() { }
    public abstract void Break();
    public abstract void Removed();
    public abstract void Destroyed();
}

public abstract class TriggerHit
{
    public Weapon weapon;
    Controller con { get { return weapon.con; } }

    public void Hit()
    {

    }
    public abstract void OnHit(TriggerObject triggerObject, Collider2D coll);

    public class TakeDamage : TriggerHit
    {
        [SerializeField] float damage;
        public override void OnHit(TriggerObject triggerObject, Collider2D coll)
        {
            if (coll.gameObject.layer == 19)
            {
                if (con is PlayerController) return;

                coll.GetComponent<Controller>().TakeDamage(damage);
            }
            if (coll.gameObject.layer == 20)
            {
                if (con is EnemyController) return;

                coll.GetComponent<Controller>().TakeDamage(damage);
            }
            else if (coll.gameObject.layer == 21)
            {
                coll.GetComponent<Controller>().TakeDamage(damage);
            }
        }
    }
}
