using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public interface IMaterialLink
{
    [ShowInInspector]
    public int Temperature { get; }
    public void ConductHeat(IMaterialLink material);
    public void Heat(int intensity);
}
[Serializable]
public struct DurabilityRecovery
{
    public float recovery;
    public readonly float value;
    public readonly float rate;

    public DurabilityRecovery(float recovery, float value, float rate)
    {
        this.recovery = recovery;
        this.value = value;
        this.rate = rate;
    }

    public bool Recovery(MaterialLinkInfo info)
    {
        float delta = recovery * rate + value;
        recovery -= delta;

        if (recovery < 0)
        {
            delta += recovery;
            recovery = 0;
        }

        info.durability += delta;

        return (recovery == 0);
    }
}

[Serializable]
public abstract class MaterialUsage
{

}

[Serializable]
public class WeaponMaterial : IMaterialLink, IInventoryItem
{
    public Sprite Icon => data.icon;
    public string DisplayedName => data.displayedName;

    public int Temperature => (int)(heat / mass);

    public WeaponMaterialData data;

    /// <summary>
    /// [K/100]
    /// </summary>
    public uint heat = 27315;

    [ShowInInspector]
    public float RealCHeat { 
        get => (heat * 0.01f) / mass - 273.15f;
        set => heat = (uint)(((value * 100) + 27315) * mass);
    }

    [ReadOnly]
    public float deltaHeat = 0;

    /// <summary>
    /// [g]
    /// </summary>
    public float mass = 100;
    public WeaponMaterial(WeaponMaterialData data)
    {
        this.data = data;
    }
    public void FixedUpdate()
    {
        deltaHeat *= data.deltaHeatRate;
    }
    public void ConductHeat(IMaterialLink material)
    {
        int delta = Mathf.RoundToInt((Temperature - material.Temperature) * data.heatConductivity);
        material.Heat(delta);
        Heat(-delta);
    }

    public void Heat(int intensity)
    {
        if (intensity > 0)
        {
            heat += (uint)intensity;
            deltaHeat += intensity;
        }
        else
        {
            heat -= (uint)-intensity;
            deltaHeat += intensity;
        }
    }
}
