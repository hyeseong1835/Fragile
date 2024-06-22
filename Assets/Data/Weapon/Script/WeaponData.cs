using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Weapon Data")]
public class WeaponData : ScriptableObject
{
    public int maxDurability;

    public float attackDamage;
    public float attackCooltime;

    public float specialDamage;
    public float specialCooltime;
}
