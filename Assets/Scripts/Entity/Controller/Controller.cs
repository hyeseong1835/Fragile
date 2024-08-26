using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;

public abstract class Controller : Entity
{
    public Weapon curWeapon;

    [NonSerialized] public Vector2 moveInput = Vector2.zero;
}