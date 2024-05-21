using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Skill : Module
{
    public Weapon weapon;
    public Controller con { get { return weapon.con; } }

    protected override void InitModule()
    {

        InitSkill();
    }
    protected virtual void InitSkill() { }

    void Update()
    {
        SkillUpdate();
    }
    public virtual void SkillInit() { }
    public virtual void SkillUpdate() { }
    public virtual void OnUseUpdate() { }
    public virtual void DeUseUpdate() { }
    public virtual void OnUse() { }
    public virtual void DeUse() { }
    public virtual void Break() { }
    public virtual void Removed() { }
    public virtual void Destroyed() { }
}
