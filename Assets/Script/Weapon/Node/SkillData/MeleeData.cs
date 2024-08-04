using static Skill.Melee;
using UnityEngine;

[CreateAssetMenu(fileName = "New MeleeData", menuName = "SkillData/Melee")]
public class MeleeData : ScriptableObject
{
    public float duration;
    public float spread;
    public float startSpear;
    public float spear;
    public SwingCurve curve;
}