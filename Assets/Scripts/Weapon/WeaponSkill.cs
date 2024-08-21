using System;
using UnityEngine;

[Serializable]
public abstract class WeaponSkill
{
    public Weapon weapon;

    public abstract void Execute();

#if UNITY_EDITOR
    public abstract void OnGUI(UnityEditor.SerializedProperty arrayProperty);
#endif
}