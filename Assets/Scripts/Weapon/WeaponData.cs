using UnityEngine;

//[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Data/Weapon/Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("정보")]
    public Sprite icon;
    public string displayedWeaponName;
    public string description;
}