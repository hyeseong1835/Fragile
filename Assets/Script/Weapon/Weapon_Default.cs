using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Weapon_Default : Weapon
{
    [HorizontalGroup("AttackSkill")]
    #region AttackSkill  - - - - - - - - - - - - - - - - - - - - -|                

        public Skill[] attackSkills;
                                                                   [HorizontalGroup("AttackSkill")]
        [VerticalGroup("AttackSkill/Vertical", PaddingTop = 1)]//-|
        [ShowInInspector][HideIf("noAttackSkills")]
        [TableMatrix(RowHeight = 25, HideRowIndices = true)]
        public bool[,] attackSet = new bool[1, 1];

        #if UNITY_EDITOR
        bool noAttackSkills { get { return attackSkills.Length == 0; } }

        #endif

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - -|            

    [HorizontalGroup("SpecialSkill")]
    #region AttackSkill - - - - - - - - - - - - - - - - - - - - - -|                

        public Skill[] specialSkills;
                                                                    [HorizontalGroup("SpecialSkill")]
        [VerticalGroup("SpecialSkill/Vertical", PaddingTop = 1)]//-|
        [ShowInInspector][HideIf("noSpecialSkills")]
        [TableMatrix(RowHeight = 25, HideRowIndices = true)]
        public bool[,] specialSet = new bool[1, 1];

        #if UNITY_EDITOR
        bool noSpecialSkills { get { return specialSkills.Length == 0; } }

        #endif

    #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - -|     

    protected override void WeaponAwake()
    {

    }
    protected override void WeaponUpdate()
    {
        if (Editor.GetType(Editor.StateType.IsEditor))
        {
            //AttackSet Resize
            if (attackSet.GetLength(1) != attackSkills.Length)
            {
                bool[,] temp = attackSet;
                attackSet = new bool[attackSet.GetLength(0), attackSkills.Length];

                for (int x = 0; x < attackSet.GetLength(0); x++)
                {
                    for (int y = 0; y < Utility.Smaller(attackSet.GetLength(1), temp.GetLength(1)); y++)
                    {
                        attackSet[x, y] = temp[x, y];
                    }
                }
            }

            //SpecialSet Resize
            if (specialSet.GetLength(1) != specialSkills.Length)
            {
                bool[,] temp = specialSet;
                specialSet = new bool[specialSet.GetLength(0), specialSkills.Length];

                for (int x = 0; x < specialSet.GetLength(0); x++)
                {
                    for (int y = 0; y < Utility.Smaller(specialSet.GetLength(1), temp.GetLength(1)); y++)
                    {
                        specialSet[x, y] = temp[x, y];
                    }
                }
            }
        }
    }
    protected override void OnUseUpdate()
    {
        foreach (Skill skill in attackSkills)
        {
            skill.OnUseUpdate();
        }
        foreach (Skill skill in specialSkills)
        {
            skill.OnUseUpdate();
        }
    }
    protected override void DeUseUpdate()
    {
        foreach (Skill skill in attackSkills)
        {
            skill.DeUseUpdate();
        }
        foreach (Skill skill in specialSkills)
        {
            skill.DeUseUpdate();
        }
    }
    protected override void OnUse()
    {
        if(hand_obj != null)
        {
            hand_obj.gameObject.SetActive(true);
            con.hand.HandLink(hand_obj, HandMode.ToHand);
        }
        foreach (Skill skill in attackSkills)
        {
            skill.OnUse();
        }
        foreach (Skill skill in specialSkills)
        {
            skill.OnUse();
        }
    }
    public override void Attack()
    {
        
    }
    protected override void OnDeUse()
    {
        if (hand_obj != null)
        {
            hand_obj.gameObject.SetActive(false);
            con.hand.HandLink(null);
        }
        foreach (Skill skill in attackSkills)
        {
            skill.DeUse();
        }
        foreach (Skill skill in specialSkills)
        {
            skill.DeUse();
        }
    }
    public override void OnWeaponRemoved()
    {
        foreach (Skill skill in attackSkills)
        {
            skill.Removed();
        }
        foreach (Skill skill in specialSkills)
        {
            skill.Removed();
        }

        Destroy(hand_obj.gameObject);
        con.hand.HandLink(null);
    }
    protected override void Break()
    {
        foreach (Skill skill in attackSkills)
        {
            skill.Break();
        }
        foreach (Skill skill in specialSkills)
        {
            skill.Break();
        }
        WeaponDestroy();
    }
    protected override void OnWeaponDestroyed()
    {
        foreach (Skill skill in attackSkills)
        {
            skill.Destroyed();
        }
        foreach (Skill skill in specialSkills)
        {
            skill.Destroyed();
        }
    }
}