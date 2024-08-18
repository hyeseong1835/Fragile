using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEventHandler
{

}
public abstract class Weapon : MonoBehaviour
{
    public struct InputData
    {
        public InputType type;
        public bool input;
        public float holdTime;
        public bool canUse;
        public bool isUsing;

        public InputData(InputType type, bool input = false, float holdTime = 0, bool canUse = true, bool isUsing = false)
        {
            this.type = type;
            this.input = input;
            this.holdTime = holdTime;
            this.canUse = canUse;
            this.isUsing = isUsing;
        }
    }
    [Header("������")]
    public WeaponData data;
    public WeaponRule rule;

    [Header("����")]
    [NonSerialized] public Controller owner;

    //��Ŭ�� ��ų
    [NonSerialized] public WeaponSkillInvoker attackSkillInvoker;

    //��Ŭ�� ��ų
    [NonSerialized] public WeaponSkillInvoker specialSkillInvoker;

    void Awake()
    {
        gameObject.SetActive(false);

        Initialize();
    }
    public virtual void Initialize()
    {

    }
    void Update()
    {

    }
}
