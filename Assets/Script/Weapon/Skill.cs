using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WeaponSystem
{
    public abstract class Skill : Module
    {
        protected Weapon weapon;
        protected Controller con { get { return weapon.con; } }

        protected override void InitModule()
        {
            weapon = gameObject.GetComponent<Weapon>();

            InitSkill();
        }
        protected virtual void InitSkill() { }

        void Update()
        {
            SkillUpdate();
        }
        public virtual void SkillInit() { }
        public virtual void SkillUpdate() { }
        public virtual void OnUseUpdate() { }
        public virtual void DeUseUpdate() { }
        public virtual void OnUse() { }
        public virtual void DeUse() { }
        public virtual void Break() { }
        public virtual void Removed() { }
        public virtual void Destroyed() { }
    }
}