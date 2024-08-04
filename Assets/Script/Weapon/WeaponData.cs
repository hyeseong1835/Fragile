using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Melee, Range
}
[CreateAssetMenu(fileName = "Weapon Data", menuName = "Weapon Data")]
public class WeaponData : ScriptableObject
{
    public int maxDurability;

    public float attackDamage;
    public float attackCooltime;

    public float specialDamage;
    public float specialCooltime;

    [Header("AI Guide")]
    public WeaponType weaponType;
    public float attackRange;
}
