using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMaterialLink
{

}
public class WeaponMaterial : IMaterialLink
{
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

        public bool Recovery(WeaponMaterial material)
        {
            float delta = recovery * rate + value;
            recovery -= delta;

            if (recovery < 0)
            {
                delta += recovery;
                recovery = 0;
            }

            material.durability += delta;

            return (recovery == 0);
        }
    }

    public WeaponMaterialData data;

    public List<IMaterialLink> link = new List<IMaterialLink>();

    public float durability;
    List<DurabilityRecovery> recovery = new List<DurabilityRecovery>();

    public float heat = 273.15f;
    public float deltaHeat = 0;
    public float deltaHeatRate = 0.5f;

    public void Update()
    {
        deltaHeat *= deltaHeatRate;
        for (int i = recovery.Count - 1; i >= 0; i--)
        {
            if (recovery[i].Recovery(this))
            {
                recovery.RemoveAt(i);
            }
        }
    }

    public void Heat(float intensity)
    {
        heat += intensity;
        deltaHeat += intensity;
    }
    public void Damage(float intensity, DurabilityRecovery recovery)
    {
        durability -= intensity;
        this.recovery.Add(recovery);
    }
}
