using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public struct MaterialLinkInfo
{
    public int aMaterialIndex, bMaterialIndex;

    [NonSerialized] public WeaponMaterial a, b;
    public void SetA(WeaponMaterial material) => a = material;
    public void SetB(WeaponMaterial material) => b = material;

    public float durability;
    public List<DurabilityRecovery> recovery;

    public void Damage(float intensity, DurabilityRecovery recovery)
    {
        durability -= intensity;
        this.recovery.Add(recovery);
    }
}
public abstract class Weapon : MonoBehaviour
{
    [ShowInInspector]
    public Controller owner;

    public WeaponMaterial[] material;
    public MaterialLinkInfo[] linkInfo;

    protected void Awake()
    {
        for (int i = 0; i < linkInfo.Length; i++)
        {
            ref MaterialLinkInfo info = ref linkInfo[i];
            linkInfo[i].SetA(material[linkInfo[i].aMaterialIndex]);
            linkInfo[i].SetB(material[linkInfo[i].bMaterialIndex]);
        }
    }

    protected void FixedUpdate()
    {
        material[0].ConductHeat(owner);
        owner.ConductHeat(material[0]);

        foreach (WeaponMaterial material in material)
        {
            material.FixedUpdate();
        }
        for (int i = 0; i < linkInfo.Length; i++)
        {
            ref MaterialLinkInfo info = ref linkInfo[i];
            info.a.ConductHeat(info.b);
            info.b.ConductHeat(info.a);

            for (int recoveryIndex = linkInfo[i].recovery.Count - 1; recoveryIndex >= 0; recoveryIndex--)
            {
                if (info.recovery[recoveryIndex].Recovery(info))
                {
                    info.recovery.RemoveAt(recoveryIndex);
                }
            }
        }
    }
}
