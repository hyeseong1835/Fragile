using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill_Swing : WeaponTriggerSkill
{
    public Skill_Swing()
    {
        Debug.Log("Skill_Swing is Created!");
    }
    ~Skill_Swing()
    {
        Debug.Log("Skill_Swing is Disposed!");
    }
    public override void Execute()
    {
        foreach (WeaponBehavior behavior in executeBehavior)
        {
            behavior.Execute();
        }
    }

#if UNITY_EDITOR
    public override void OnGUI(UnityEditor.SerializedObject ruleObject, string path)
    {
        CustomGUILayout.TitleHeaderLabel("»÷µŒ∏£±‚");
        
        base.OnGUI(ruleObject, path);
    }
#endif
}