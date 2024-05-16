using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : Module
{
    protected Weapon weapon;
    protected Controller con { get { return weapon.con; } }
    private void Awake()
    {
        weapon = GetComponent<Weapon>();
        Init();
    }
    protected virtual void Init() { }
}
