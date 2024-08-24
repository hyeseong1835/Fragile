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
}
