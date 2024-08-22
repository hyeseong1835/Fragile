using System;
using UnityEngine;

[Serializable]
public abstract class WeaponSkill : UnityEngine.Object
{
    public Weapon weapon;

    public abstract void Execute();

#if UNITY_EDITOR
    public abstract void OnGUI(UnityEditor.SerializedObject ruleObject, string path);
#endif
}