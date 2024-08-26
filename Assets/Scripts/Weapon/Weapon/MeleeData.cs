using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(fileName = "New Weapon Data", menuName = "Data/무기/근접 무기/데이터")]
    public class MeleeData : WeaponData
    {
        public override WeaponRule Rule => rule;
        public MeleeRule rule;
    }
}