using UnityEngine;
using WeaponSystem;

public class Skill_TakeDamage : Skill, IController
{
    [SerializeField] float damageMultiply = 1;

    public void InputController(Controller con)
    {
        con.TakeDamage(weapon.damage * damageMultiply);
    }
}
