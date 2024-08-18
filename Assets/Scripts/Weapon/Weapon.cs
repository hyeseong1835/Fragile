using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEventHandler
{

}
public class Weapon : MonoBehaviour
{
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
    }
    void Update()
    {

    }
}
