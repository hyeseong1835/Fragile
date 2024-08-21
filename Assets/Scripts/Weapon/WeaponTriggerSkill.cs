using System;
using UnityEditor;
using UnityEngine;

public abstract class WeaponTriggerSkill : WeaponSkill
{
    public WeaponBehavior[] executeBehaviors;
    
    public void WeaponSkillExecute()
    {

    }
}

[Serializable]
public abstract class WeaponTriggerSkillData : WeaponSkillData
{

    [SerializeReference] public WeaponBehaviorData[] executeBehaviorData = new WeaponBehaviorData[0];

#if UNITY_EDITOR
    public static WeaponTriggerSkillData Default => new SkillData_Swing();

    public override void OnGUI()
    {
        EditorGUILayout.LabelField("-즉시");
        CustomGUILayout.BeginTab();
        {
            foreach (WeaponBehaviorData behaviorData in executeBehaviorData)
            {
                behaviorData.OnGUI();
            }
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("추가"))
                {
                    Array.Resize(ref executeBehaviorData, executeBehaviorData.Length + 1);
                    executeBehaviorData[executeBehaviorData.Length - 1] = WeaponBehaviorData.Default;
                }
                if (GUILayout.Button("삭제"))
                {
                    if (executeBehaviorData.Length > 0)
                    {
                        Array.Resize(ref executeBehaviorData, executeBehaviorData.Length - 1);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        CustomGUILayout.EndTab();}
#endif
}
