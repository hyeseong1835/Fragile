using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Windows;

public abstract class WeaponTriggerSkill<TSkill, TData> : WeaponSkill<TSkill, TData>
    where TSkill : WeaponTriggerSkill<TSkill, TData>
    where TData : WeaponTriggerSkillData<TSkill, TData>
{
    public override WeaponSkillDataBase Data
    {
        get => data;
        set => data = (TData)value;
    }
    protected TData data;
    public WeaponBehaviorBase[] executeBehaviors;
    
    public void WeaponSkillExecute()
    {

    }
}
