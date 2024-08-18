using UnityEngine;

public enum InputType
{
    Trigger, Hold
}
public class WeaponRule : ScriptableObject
{
    [Header("좌클릭 공격")]
    [SerializeReference] public WeaponSkillDataBase[] attackActiveSkill = new WeaponSkillDataBase[0];

    [Header("우클릭 공격")]
    [SerializeReference] public WeaponSkillDataBase[] specialActiveSkill = new WeaponSkillDataBase[0];
}