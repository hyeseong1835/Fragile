using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Material.Usage;

namespace WeaponSystem.Material
{
    [CreateAssetMenu(menuName = "Data/무기/재료", fileName = "New Material Data")]
    public class WeaponMaterialData : ScriptableObject
    {
        public Sprite icon;
        public string displayedName;
        public string description;

        [SerializeReference] public WeaponMaterialUsage[] usages = new WeaponMaterialUsage[0];

#if UNITY_EDITOR
        public int IndexOfUsage(Type usageType)
        {
            for (int i = 0; i < usages.Length; i++)
            {
                if(usages[i].GetType().Equals(usageType))
                {
                    return i;
                }
            }
            return -1;
        }
        public void AddUsage(WeaponMaterialUsageInfo info)
        {
            WeaponMaterialUsage usage = (WeaponMaterialUsage)Activator.CreateInstance(info.type);

            for (int i = 0; i < info.need.Length; i++)
            {
                ref Type needType = ref info.need[i];

                int index = IndexOfUsage(needType);
                if (index == -1)
                {
                    AddUsage(new WeaponMaterialUsageInfo(needType));
                }
            }
        }
#endif
    }
}