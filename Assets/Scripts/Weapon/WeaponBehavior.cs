using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class WeaponBehavior
{
    public WeaponSkill skill;

    protected abstract void Initialize();
    public abstract void Execute();
    public abstract void OnGUI(UnityEditor.SerializedProperty property);


#if UNITY_EDITOR
    public static WeaponBehavior GetDefault() => new Behavior_Damage();
#endif
}