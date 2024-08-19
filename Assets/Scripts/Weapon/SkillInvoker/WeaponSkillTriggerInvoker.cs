using UnityEngine;
using System;
using JetBrains.Annotations;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeaponSkillTriggerInvoker : WeaponSkillInvoker
{
    [SerializeReference] public WeaponTriggerSkill[] onTrigger;
    public bool canInvoke = true;

    public WeaponSkillTriggerInvoker(WeaponSkillTriggerInvokerData data)
    {

    }
    public override void WeaponInvokeUpdate()
    {
        if (input)
        {
            if (canInvoke)
            {
                foreach (WeaponTriggerSkill skill in onTrigger)
                {
                    skill.WeaponSkillExecute();
                }
                canInvoke = false;
            }
        }
    }
}
[Serializable]
public class WeaponSkillTriggerInvokerData : WeaponSkillInvokerData
{
    [SerializeReference] public WeaponTriggerSkillData[] onTrigger = new WeaponTriggerSkillData[0];

    public override WeaponSkillInvoker CreateInvoker() => new WeaponSkillTriggerInvoker(this);

#if UNITY_EDITOR
    public override void OnGUI()
    {
        CustomGUILayout.TitleHeaderLabel("실행기 (트리거)");

        EditorGUILayout.LabelField("트리거 시");
        CustomGUILayout.BeginTab();
        {
            foreach (WeaponTriggerSkillData skillData in onTrigger)
            {
                skillData.OnGUI();
            }
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("추가"))
                {
                    Array.Resize(ref onTrigger, onTrigger.Length + 1);
                    onTrigger[onTrigger.Length - 1] = WeaponTriggerSkillData.Default;
                }
                if (GUILayout.Button("삭제"))
                {
                    if (onTrigger.Length > 0)
                    {
                        Array.Resize(ref onTrigger, onTrigger.Length - 1);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        CustomGUILayout.EndTab();
    }
#endif
}