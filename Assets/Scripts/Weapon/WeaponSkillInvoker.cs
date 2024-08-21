using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public abstract class WeaponSkillInvoker
{
    public WeaponSkillInvokerData data;
    public bool input = false;

    public abstract void WeaponInvokeUpdate();
}

[Serializable]
public abstract class WeaponSkillInvokerData
{
    public abstract WeaponSkillInvoker CreateInvoker();

#if UNITY_EDITOR
    public static Type[] weaponSkillInvokerDataTypes;
    public static string[] names;
    public static WeaponSkillInvokerData Default => new WeaponSkillTriggerInvokerData();

    [InitializeOnLoadMethod]
    static void OnLoad()
    {
        weaponSkillInvokerDataTypes = TypeUtility.LoadChildType<WeaponSkillInvokerData>();
        names = new string[weaponSkillInvokerDataTypes.Length];
        for (int i = 0; i < names.Length; i++)
        {
            InvokerDropdownAttribute attribute = weaponSkillInvokerDataTypes[i].GetCustomAttribute<InvokerDropdownAttribute>(false);
            names[i] = attribute.name;
        }
    }
    public virtual void OnGUI()
    {
        
    }
#endif
}

#if UNITY_EDITOR
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class InvokerDropdownAttribute : PropertyAttribute
{
    public string name;
    public InvokerDropdownAttribute(string name)
    {
        this.name = name;
    }
}
#endif