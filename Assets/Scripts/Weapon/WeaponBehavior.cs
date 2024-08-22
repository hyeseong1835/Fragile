using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class WeaponBehavior : UnityEngine.Object
{
    public WeaponSkill skill;

    protected abstract void Initialize();
    public abstract void Execute();
    public abstract void OnGUI(UnityEditor.SerializedObject ruleObject, string path);


#if UNITY_EDITOR
    public static WeaponBehavior GetDefault() => new Behavior_Damage();
#endif
}