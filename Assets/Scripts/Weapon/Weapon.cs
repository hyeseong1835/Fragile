using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEventHandler
{

}
public abstract class Weapon : MonoBehaviour
{
    public struct InputData
    {
        public InputType type;
        public bool input;
        public float holdTime;
        public bool canUse;
        public bool isUsing;

        public InputData(InputType type, bool input = false, float holdTime = 0, bool canUse = true, bool isUsing = false)
        {
            this.type = type;
            this.input = input;
            this.holdTime = holdTime;
            this.canUse = canUse;
            this.isUsing = isUsing;
        }
    }
    [Header("데이터")]
    public WeaponData data;
    public WeaponRule rule;

    [Header("정보")]
    [NonSerialized] public Controller owner;

    //좌클릭 스킬
    [NonSerialized] public WeaponSkillBase[] attackSkills;
    [NonSerialized] public bool attackInput = false;
    [NonSerialized] public float attackHoldTime = 0;
    [NonSerialized] public bool canUseAttack = true;
    [NonSerialized] public bool isUsingAttack = false;

    //우클릭 스킬
    [NonSerialized] public WeaponSkillBase[] specialSkills;
    [NonSerialized] public bool specialInput = false;
    [NonSerialized] public float specialHoldTime = 0;
    [NonSerialized] bool canUseSpecial = true;
    [NonSerialized] bool isUsingSpecial = false;

    void Awake()
    {
        gameObject.SetActive(false);

        Initialize();
    }
    public virtual void Initialize()
    {

    }
    void Update()
    {
        UpdateActiveSkill(ref attackSkills, ref attackInput, ref attackHoldTime, ref canUseAttack, ref isUsingAttack);
        UpdateActiveSkill(ref specialSkills, ref specialInput, ref specialHoldTime, ref canUseSpecial, ref isUsingSpecial);
    }
    void UpdateActiveSkill(ref WeaponSkillBase[] activeSkills, ref bool input, ref float holdTime, ref bool canUse, ref bool isUsing)
    {
        if (input)
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
                canUse = true;//임시
            }
        }

        if (isUsing)
        {
            holdTime += Time.deltaTime;
        }
    }
}
