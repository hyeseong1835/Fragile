using UnityEngine;
using WeaponSystem;

public class Skill_AddDurability : Skill, IVoid
{
    [SerializeField] int durability;
    public void InputVoid()
    {
        weapon.AddDurability(durability);
    }
}
