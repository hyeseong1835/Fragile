using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(fileName = "New Weapon Data", menuName = "Data/����/���� ����/������")]
    public class MeleeData : WeaponData
    {
        public override WeaponRule Rule => rule;
        public MeleeRule rule;
    }
}