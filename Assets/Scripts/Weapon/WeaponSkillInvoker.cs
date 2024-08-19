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
#if UNITY_EDITOR
    public static WeaponSkillInvokerData Default => new WeaponSkillTriggerInvokerData();
#endif
    public abstract WeaponSkillInvoker CreateInvoker();

    public abstract void OnGUI();
}