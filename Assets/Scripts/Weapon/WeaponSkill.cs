using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSkill : ScriptableObject
{
    public virtual void SkillInitialize(Weapon weapon)
    {
        Initialize(weapon);
    }
    protected abstract void Initialize(Weapon weapon);
    protected abstract void Execute();
}
