using System;


public abstract class WeaponSkillInvoker
{
    public WeaponSkillInvokerData data;
    public bool input = false;

    public abstract void WeaponInvokeUpdate();
}

[Serializable]
public abstract class WeaponSkillInvokerData
{
    public static WeaponSkillInvokerData Default => new WeaponSkillTriggerInvokerData();

    public abstract WeaponSkillInvoker CreateInvoker();
}