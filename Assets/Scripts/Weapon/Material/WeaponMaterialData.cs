using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/무기/재료", fileName = "New Material Data")]
public class WeaponMaterialData : ScriptableObject
{
    public Sprite icon;
    public string displayedName;
    public string description;

    public float maxDurability = 100;
    public float heatConductivity = 0.25f;
    public float deltaHeatRate = 0.5f;
    //public float specificHeat;

    [SerializeReference] public MaterialUsage[] usage;
}
