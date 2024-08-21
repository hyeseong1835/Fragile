
using System;
using UnityEngine;

public abstract class WeaponOperator<ValueT>
{
    public static WeaponOperator<ValueT> GetDefault() => new Operator_AssignValue<ValueT>();
    public abstract ValueT GetValue(WeaponBehavior behavior);

    public abstract void OnGUI(UnityEditor.SerializedProperty property);
}

#if UNITY_EDITOR
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class OperatorInfoAttribute : PropertyAttribute
{
    public string name;
    public OperatorInfoAttribute(string name)
    {
        this.name = name;
    }
}
#endif