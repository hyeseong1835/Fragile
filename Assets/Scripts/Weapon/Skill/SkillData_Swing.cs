using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillData_Swing : WeaponTriggerSkillData
{
    public override WeaponSkill CreateWeaponSkillInstance(Weapon weapon)
        => new Skill_Swing(this, weapon);
#if UNITY_EDITOR
    public override void OnGUI()
    {
        CustomGUILayout.TitleHeaderLabel("ÈÖµÎ¸£±â");
        
        base.OnGUI();
    }
#endif
}
public class Skill_Swing : WeaponTriggerSkill
{
    public override WeaponSkillData Data
    {
        get => data;
        set => data = (SkillData_Swing)value;
    }
    protected SkillData_Swing data;

    public Skill_Swing(SkillData_Swing data, Weapon weapon)
    {
        this.data = data;
        this.weapon = weapon;
        this.executeBehaviors = new WeaponBehavior[data.executeBehaviorData.Length];
        {
            for (int i = 0; i < executeBehaviors.Length; i++)
            {
                executeBehaviors[i] = data.executeBehaviorData[i].CreateWeaponBehaviorInstance(this);
            }
        }
    }
}