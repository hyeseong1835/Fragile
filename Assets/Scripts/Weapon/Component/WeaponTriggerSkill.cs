using System;
using UnityEngine;

[Serializable]
#if UNITY_EDITOR
[ComponentInfo("Ʈ����", "������ ��� ���Ǵ� ��ų�Դϴ�.")]
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
                executeBehavior[i].WeaponComponentOnGUI(ref executeBehavior[i], "����");
                return false;
            },
            WeaponBehavior.GetDefault
        );
    }
#endif
}
