using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
#if UNITY_EDITOR
[ComponentInfo("»÷µŒ∏£±‚", "»÷µŒ∏®¥œ¥Ÿ.")]
#endif
public class Skill_Swing : WeaponTriggerSkill
{
    public override void Execute()
    {
        foreach (WeaponBehavior behavior in executeBehavior)
        {
            behavior.Execute();
        }
    }

#if UNITY_EDITOR
    
#endif
}