using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WeaponSystem.Material
{
    [CreateAssetMenu(menuName = "Data/����/���", fileName = "New Material Data")]
    public class WeaponMaterialData : ScriptableObject
    {
        public Sprite icon;
        public string displayedName;
        public string description;

        [SerializeReference] public WeaponMaterialUsage[] usage;
    }
}