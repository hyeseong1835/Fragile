using System;
using UnityEngine;

[Serializable]
#if UNITY_EDITOR
[ComponentInfo("트리거", "시전된 즉시 사용되는 스킬입니다.")]
#endif
public abstract class WeaponTriggerSkill : WeaponSkill
{
    [SerializeReference] public WeaponBehavior[] executeBehavior = new WeaponBehavior[0];

#if UNITY_EDITOR
    public static WeaponTriggerSkill CreateDefault() => new Skill_Swing();

    protected override void DrawField()
    {
        CustomGUILayout.ArrayField(
            ref executeBehavior,
            i => {
                executeBehavior[i].WeaponComponentOnGUI(ref executeBehavior[i], "실행");
                return false;
            },
            WeaponBehavior.GetDefault
        );
    }
#endif
}
