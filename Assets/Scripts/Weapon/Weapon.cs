using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponSkill[] attack = new WeaponSkill[0];
    public WeaponSkill[] skill = new WeaponSkill[0];

    void Awake()
    {
        foreach (WeaponSkill s in attack)
        {
            s.SkillInitialize(this);
        }
        foreach (WeaponSkill s in skill)
        {
            s.SkillInitialize(this);
        }
    }
}
