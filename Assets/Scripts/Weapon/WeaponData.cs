using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Data/Weapon/Weapon")]
public class WeaponData : ScriptableObject
{
    public WeaponRule rule;

    public Sprite icon;
    public string displayedWeaponName;
    public string description;
}