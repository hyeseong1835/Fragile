using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/����/���", fileName = "New Material Data")]
public class WeaponMaterialData : ScriptableObject
{
    public Sprite icon;
    public string displayedName;
    public string description;

    public float maxDurability = 100;
}
