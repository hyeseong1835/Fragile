using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : Entity, IMaterialLink
{
    public Weapon curWeapon;

    [NonSerialized] public Vector2 moveInput = Vector2.zero;

    protected void Update()
    {
        Move();
    }
    protected virtual void Move()
    { 
        transform.position += (Vector3)(moveInput.normalized * Time.deltaTime);
    }
}
