using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace WeaponSystem.Material.Usage
{
#if UNITY_EDITOR
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MaterialUsageInfoAttribute : Attribute
    {
        public string name;
        public string description;
        public bool isAbstract;
        public Type[] need;

        public MaterialUsageInfoAttribute(string path, string description, bool isAbstract, params Type[] need)
        {
            this.name = path;
            this.description = description;
            this.isAbstract = isAbstract;
            this.need = need;
        }
    }
    public class WeaponMaterialUsageInfo
    {
        public Type type;
        public Type[] need;
        public string name;
        public string description;

        public WeaponMaterialUsageInfo(Type type, Type[] need, string name, string description)
        {
            this.type = type;
            this.need = need;
            this.name = name;
            this.description = description;
        }
        public WeaponMaterialUsageInfo(Type type)
        {
            MaterialUsageInfoAttribute attribute = type.GetCustomAttribute<MaterialUsageInfoAttribute>();

            this.type = type;
            this.need = attribute.need;
            this.name = attribute.name;
            this.description = attribute.description;
        }
        public override string ToString()
        {
            return $"[{type}] \"{name}\" : ({description})";
        }
    }
#endif

    [Serializable]
    public abstract class WeaponMaterialUsage
    {
#if UNITY_EDITOR
        public static WeaponMaterialUsage GetDefault() => new BodyUsage();
        public static List<WeaponMaterialUsageInfo> infos = new List<WeaponMaterialUsageInfo>();

        public WeaponMaterialData data;
        public WeaponMaterialUsageInfo info;

        [InitializeOnLoadMethod]
        static void LoadComponentInfo()
        {
            Type materialUsageType = typeof(WeaponMaterialUsage);
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsClass && type.IsAbstract == false && materialUsageType.IsAssignableFrom(type))
                {
                    infos.Add(new WeaponMaterialUsageInfo(type));
                }
            }
        }

        [MenuItem("Tools/머티리얼 구조 확인")]
        static void DebugComponentInfo()
        {
            foreach (WeaponMaterialUsageInfo info in infos)
            {
                Debug.Log(info);
            }
        }
        public static WeaponMaterialUsageInfo GetInfo(Type type)
        {
            for (int i = 0; i < infos.Count; i++)
            {
                WeaponMaterialUsageInfo info = infos[i];
                if (type.Equals(info))
                {
                    return info;
                }
            }
            return null;
        }
        public abstract void OnGUI();
        public virtual void SetMenu(GenericMenu menu)
        {

        }
#endif
    }
}