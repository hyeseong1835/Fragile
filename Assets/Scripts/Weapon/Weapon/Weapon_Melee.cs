using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[AddComponentMenu("무기/근접 무기")]
public class Weapon_Melee : Weapon
{
    new protected void Awake()
    {
        base.Awake();
    }
    protected void OnEnable()
    {
        
    }
    [Button("FixedUpdate")]
    new protected void FixedUpdate()
    {
        base.FixedUpdate();
    }
}