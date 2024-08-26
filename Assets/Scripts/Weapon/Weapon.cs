using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace WeaponSystem
{
    public abstract class Weapon : MonoBehaviour
    {
        [ShowInInspector]
        public Controller owner;

        [NonSerialized]
        [ShowInInspector]
        public bool input = false;
    }
}