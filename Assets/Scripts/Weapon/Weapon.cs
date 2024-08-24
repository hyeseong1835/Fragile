using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEventHandler
{

}
public abstract class Weapon : MonoBehaviour
{
    public WeaponMaterial[] materials;

    [NonSerialized] public Controller owner;

    protected void Awake()
    {
        gameObject.SetActive(false);
    }
    protected void Update()
    {
        foreach (WeaponMaterial mat in materials)
        {
            mat.Update();
        }
    }
}
