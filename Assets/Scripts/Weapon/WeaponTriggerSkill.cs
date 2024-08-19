using System;
using UnityEngine;

public abstract class WeaponTriggerSkill : WeaponSkill
{
    public WeaponBehavior[] executeBehaviors;
    
    public void WeaponSkillExecute()
    {

    }
}

[Serializable]
public abstract class WeaponTriggerSkillData : WeaponSkillData
{
    [SerializeReference] public WeaponBehaviorData[] executeBehaviorData = new WeaponBehaviorData[0];
}