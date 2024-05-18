using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : Module
{
    protected Weapon weapon;
    protected Controller con { get { return weapon.con; } }
    protected override void InitModule()
    {
        weapon = GetComponent<Weapon>();
        InitAction();
    }
    protected virtual void InitAction() { }
}
