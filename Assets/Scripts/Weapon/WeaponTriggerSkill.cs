using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Windows;

public class WeaponTriggerSkill<TSkill, TSkillData> : WeaponSkill<TSkill, TSkillData>
    where TSkill : WeaponTriggerSkill<TSkill, TSkillData>
    where TSkillData : WeaponTriggerSkillData<TSkill, TSkillData>
{
    public override WeaponSkillDataBase Data
    {
        get => data;
        set => data = (TSkillData)value;
    }
    protected WeaponTriggerSkillData<TSkill, TSkillData> data;

    public override Weapon Weapon
    {
        get => weapon;
        set => weapon = value;
    }
    protected Weapon weapon;

    public WeaponBehaviorBase[] executeBehaviors;

    public WeaponTriggerSkill(WeaponTriggerSkillData<TSkill, TSkillData> data, Weapon weapon)
    {
        this.data = data;
        this.weapon = weapon;
        this.executeBehaviors = new WeaponBehaviorBase[data.executeBehaviorData.Length];
        {
            for (int i = 0; i < executeBehaviors.Length; i++)
            {
                executeBehaviors[i] = data.executeBehaviorData[i].CreateWeaponBehaviorInstance(this);
            }
        }
    }
    public override void WeaponSkillUpdate()
    {
        if (weapon.input)
        {
            if (!isUsing && canUse)
            {
                foreach (WeaponSkillBase s in activeSkills)
                {
                    s.WeaponSkillExecute();
                }
                isUsing = true;
                canUse = false;
                holdTime = 0;
            }
        }

        if (isUsing)
        {
            foreach (WeaponSkillBase s in activeSkills)
            {
                s.Execute(holdTime);
            }
        }

        if (!input)
        {
            if (isUsing)
            {
                foreach (WeaponSkillBase s in activeSkills)
                {
                    s.End(holdTime);
                }
                isUsing = false;
                holdTime = -1;
                canUse = true;//юс╫ц
            }
        }

        if (isUsing)
        {
            holdTime += Time.deltaTime;
        }
    }

    
    public void WeaponSkillExecute()
    {

    }
}
