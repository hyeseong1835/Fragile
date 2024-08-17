using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : Entity
{
    public Weapon curWeapon;

    public Vector2 moveInput;

    protected void Update()
    {
        Move();
    }
    public virtual void Move()
    { 
        transform.position += (Vector3)(moveInput.normalized * Time.deltaTime);
    }
}
