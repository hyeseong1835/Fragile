using System;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class InvokerInfoAttribute : PropertyAttribute
{
    public string name;
    public string description;

    public InvokerInfoAttribute(string name, string description)
    {
        this.name = name;
        this.description = description;
    }
}
#endif

[Serializable]
public abstract class WeaponSkillInvoker : UnityEngine.Object
{
    public static WeaponSkillInvoker CreateDefault() => new WeaponSkillTriggerInvoker();

    public bool input = false;

    public abstract void OnWeaponUpdate();


#if UNITY_EDITOR
    public static Type[] weaponSkillInvokerTypes;
    public static string[] names;
    public static string[] descriptions;

    [UnityEditor.InitializeOnLoadMethod]
    static void OnLoad()
    {
        weaponSkillInvokerTypes = TypeUtility.LoadChildType<WeaponSkillInvoker>();
        names = new string[weaponSkillInvokerTypes.Length];
        for (int i = 0; i < names.Length; i++)
        {
            InvokerInfoAttribute attribute = weaponSkillInvokerTypes[i].GetCustomAttribute<InvokerInfoAttribute>(false);
            names[i] = attribute.name;
        }
    }
    public abstract void OnGUI(UnityEditor.SerializedObject ruleObject, string path);
#endif
}
