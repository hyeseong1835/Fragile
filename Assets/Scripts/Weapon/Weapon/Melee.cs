using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using WeaponSystem.Material;
using WeaponSystem.Component;
using WeaponSystem.Component.Behavior;

namespace WeaponSystem
{
    [AddComponentMenu("무기/근접 무기")]
    public class Melee : Weapon
    {
        public MeleeData data;

        public BodyMaterial handle;
        public BodyMaterial[] blade;

        [NonSerialized]
        [ShowInInspector]
        bool canInvoke = true;

        protected void Awake()
        {

        }
        protected void OnEnable()
        {

        }
        protected void Update()
        {
            if (input)
            {
                if (canInvoke)
                {
                    Execute();
                    canInvoke = false;
                }
            }
        }
        void Execute()
        {
            foreach (WeaponBehavior behavior in data.rule.executeBehaviors)
            {
                behavior.Execute(this);
            }
            Debug.Log("휘두름!");
        }
    }
}