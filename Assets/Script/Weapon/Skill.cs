using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Skill: Module
{
    protected Weapon weapon;
    protected Controller con { get { return weapon.con; } }

    public float damage = 1;

    void Awake()
    {
        weapon = gameObject.GetComponent<Weapon>();
        Init();
    }
    protected virtual void Init() { }
    void Update()
    {
        SkillUpdate();
    }
    public virtual void SkillInit() { }
    public abstract void Execute();
    public virtual void SkillUpdate() { }
    public virtual void OnUseUpdate() { }
    public virtual void DeUseUpdate() { }
    public virtual void OnUse() { }
    public virtual void DeUse() { }
    public abstract void Break();
    public abstract void Removed();
    public abstract void Destroyed();
    public abstract void OnWeaponMakerGUI();
    public virtual void OnGUI() { }
}

public abstract class TriggerHit
{
    public abstract void OnHit(TriggerObject triggerObject, Collider2D coll);
}
