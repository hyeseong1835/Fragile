using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : Entity, IMaterialLink
{
    public int Temperature => 30315;

    public Weapon curWeapon;

    [NonSerialized] public Vector2 moveInput = Vector2.zero;
    public float heatConductivity = 0.1f;

    public void ConductHeat(IMaterialLink material)
    {
        int delta = Mathf.RoundToInt((Temperature - material.Temperature) * heatConductivity);
        material.Heat(delta);
        Heat(-delta);
    }
    public void Heat(int intensity)
    {
        if (intensity > 0) Debug.Log($"뜨거워요 {intensity}");
        else if (intensity < 0) Debug.Log($"차가워요 {intensity}");
    }

    public void Damage(float intensity, DurabilityRecovery recovery)
    {
        Debug.Log($"아파요 {intensity} [{recovery.recovery}, {recovery.value}, {recovery.rate}]");
    }
}
