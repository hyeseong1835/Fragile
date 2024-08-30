using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace WeaponSystem.Material
{
    [Serializable]
    public class WeaponMaterial
    {
        public WeaponMaterialData data;
        /// <summary>
        /// [g]
        /// </summary>
        public float mass = 100;
    }
}