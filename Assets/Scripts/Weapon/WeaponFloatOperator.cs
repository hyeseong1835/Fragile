using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponFloatOperator : WeaponOperator<float>
{
#if UNITY_EDITOR
    protected override Type[] ComponentTypes => operatorTypes;
    protected override string[] Names => names;
    protected override string[] Descriptions => descriptions;

    public static Type[] operatorTypes;
    public static string[] names;
    public static string[] descriptions;

    [UnityEditor.InitializeOnLoadMethod]
    static void OnLoad() => LoadChildType<WeaponFloatOperator, ComponentInfo>(ref operatorTypes, ref names, ref descriptions);
#endif
}
#if UNITY_EDITOR
public class FloatOperatorInfoAttribute : ComponentInfo
{
    public FloatOperatorInfoAttribute(string name, string description)
    {
        this.path = name;
        this.description = description;
    }
}
#endif
