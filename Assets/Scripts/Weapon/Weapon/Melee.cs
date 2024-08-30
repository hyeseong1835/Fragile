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

        public WeaponMaterial handle;
        public WeaponMaterial[] blade;

        [FoldoutGroup("Input")]
        #region Input  - - - - - - - - - - - - - - - - - - - - - -|

            [HorizontalGroup("Input/Attack")]
            #region Attack  - - - - - - - - - - - - - - - - -|    :

                [SerializeField] float attackDelay = 0.5f;//-|
                                                              [HorizontalGroup("Input/Attack", width: InspectorLayout.noLabelCheckBoxWidth)]
                [NonSerialized][ShowInInspector][ReadOnly]
                [HideLabel]
                bool canInvokeAttack = true;

            #endregion - - - - - - - - - - - - - - - - - - - -|   :

            [HorizontalGroup("Input/Special")]
            #region Special  - - - - - - - - - - - - - - - - -|   :

                [SerializeField] float specialDelay = 1.0f;//-|
                                                               [HorizontalGroup("Input/Special", width: InspectorLayout.noLabelCheckBoxWidth)]
                [NonSerialized][ShowInInspector][ReadOnly]
                [HideLabel]
                bool canInvokeSpecial = true;

            #endregion - - - - - - - - - - - - - - - - - - - -|   :

        #endregion - - - - - - - - - - - - - - - - - - - - - - - -|

        protected void Awake()
        {

        }
        protected void OnEnable()
        {

        }
        protected void Update()
        {
            if (canInvokeAttack && owner.AttackInput)
            {
                Attack();
                StartCoroutine(AttackDelay(attackDelay));
            }
            if (canInvokeSpecial && owner.SpecialInput)
            {
                Special();
                StartCoroutine(SpecialDelay(specialDelay));
            }
        }
        IEnumerator AttackDelay(float delay)
        {
            canInvokeAttack = false;
            yield return new WaitForSeconds(delay);
            canInvokeAttack = true;
        }
        IEnumerator SpecialDelay(float delay)
        {
            canInvokeSpecial = false;
            yield return new WaitForSeconds(delay);
            canInvokeSpecial = true;
        }
        void Attack()
        {
            StartCoroutine(AttackCoroutine());
            foreach (WeaponBehavior behavior in data.rule.attackExecuteBehaviors)
            {
                behavior.Execute(this);
            }
        }
        IEnumerator AttackCoroutine()
        {
            yield return new WaitForSeconds(1);
        }
        void Special()
        {
            foreach (WeaponBehavior behavior in data.rule.specialExecuteBehaviors)
            {
                behavior.Execute(this);
            }
        }
    }
}