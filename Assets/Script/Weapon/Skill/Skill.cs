using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill: MonoBehaviour
{
    protected Weapon weapon;
    protected Controller con { get { return weapon.con; } }

    void Awake()
    {
        weapon = gameObject.GetComponent<Weapon>();
        Init();
    }
    protected abstract void Init();
    public abstract void Execute();
    public virtual void OnUseUpdate() { }
    public virtual void DeUseUpdate() { }
    public virtual void OnUse() { }
    public virtual void DeUse() { }
    public abstract void Break();
    public abstract void Removed();
    public abstract void Destroyed();
}
