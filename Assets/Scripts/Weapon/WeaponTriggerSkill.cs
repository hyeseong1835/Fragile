using System;
using UnityEngine;

[Serializable]
public abstract class WeaponTriggerSkill : WeaponSkill
{
    [SerializeReference] public WeaponBehavior[] executeBehavior = new WeaponBehavior[0];

#if UNITY_EDITOR
    public static WeaponTriggerSkill CreateDefault() => new Skill_Swing();

    public override void OnGUI(UnityEditor.SerializedObject ruleObject, string path)
    {
        UnityEditor.EditorGUILayout.LabelField("-Áï½Ã");
        CustomGUILayout.BeginTab();
        {
            CustomGUILayout.ArrayField(
                ref executeBehavior, 
                i => {
                    executeBehavior[i].OnGUI(ruleObject, $"{path}.{nameof(executeBehavior)}.Array.data[{i}]");
                    return false;
                }, 
                WeaponBehavior.GetDefault
            );
        }
        CustomGUILayout.EndTab();}
#endif
}
