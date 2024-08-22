using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ComponentInfo : Attribute
{
    public string path;
    public string description;

    public ComponentInfo(string path, string description)
    {
        this.path = path;
        this.description = description;
    }
}
#endif

public abstract class WeaponRuleComponent : IDisposable
{
    public abstract void Dispose();

#if UNITY_EDITOR
    public static FloatingAreaManager floatingManager = new FloatingAreaManager();
    public static GenericMenu menu;

    protected abstract Type[] ComponentTypes { get; }
    protected abstract string[] Names { get; }
    protected abstract string[] Descriptions { get; }

    [InitializeOnLoadMethod]
    static void LoadChildType()
    {
        operatorTypes = TypeUtility.LoadChildType<TComponent>();
        names = new string[operatorTypes.Length];
        descriptions = new string[operatorTypes.Length];

        for (int i = 0; i < operatorTypes.Length; i++)
        {
            TAttribute attribute = operatorTypes[i].GetCustomAttribute<TAttribute>(false);
            names[i] = attribute.path;
            descriptions[i] = attribute.description;
        }
    }
    public static void LoadChildType<TComponent, TAttribute>(ref Type[] operatorTypes, ref string[] names, ref string[] descriptions, Action<TAttribute, int> loadAction)
     where TComponent : WeaponRuleComponent
        where TAttribute : WeaponRuleComponentInfoAttribute
    {
        operatorTypes = TypeUtility.LoadChildType<TComponent>();
        names = new string[operatorTypes.Length];
        descriptions = new string[operatorTypes.Length];

        for (int i = 0; i < operatorTypes.Length; i++)
        {
            TAttribute attribute = operatorTypes[i].GetCustomAttribute<TAttribute>(false);
            names[i] = attribute.path;
            descriptions[i] = attribute.description;
            loadAction(attribute, i);
        }
    } 
    public abstract void OnGUI<TComponent>(ref TComponent origin, string label)
        where TComponent : WeaponRuleComponent;
    #endif
}