using System;
using UnityEngine;

[Serializable]
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
                executeBehavior[i].OnGUI(ref executeBehavior[i], "½ÇÇà");
                return false;
            },
            WeaponBehavior.GetDefault
        );
    }
#endif
}
