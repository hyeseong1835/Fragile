using System;
using UnityEngine;

[Serializable]
public abstract class WeaponSkillInvokerData
{
    [SerializeReference] public WeaponSkillData[] attackActiveSkill = new WeaponSkillData[0];
    public abstract WeaponSkillInvoker CreateInvoker();
}

public abstract class WeaponSkillInvoker
{
    public WeaponSkillInvokerData data;
    public bool input = false;

    public abstract void WeaponInvokeUpdate();
}