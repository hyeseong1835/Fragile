using UnityEngine;

public abstract class WeaponSkillInvokerData
{
    public abstract WeaponSkillInvoker CreateInvoker();
}

public abstract class WeaponSkillInvoker
{
    public WeaponSkillInvokerData data;
    public bool input;

    public abstract void WeaponInvokeUpdate();
}