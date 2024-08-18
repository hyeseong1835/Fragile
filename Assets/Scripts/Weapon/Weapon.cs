using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEventHandler
{

}
public class Weapon : MonoBehaviour
{
    [Header("데이터")]
    public WeaponData data;
    public WeaponRule rule;

    [Header("정보")]
    [NonSerialized] public Controller owner;

    //좌클릭 스킬
    [NonSerialized] public WeaponSkillInvoker attackSkillInvoker;

    //우클릭 스킬
    [NonSerialized] public WeaponSkillInvoker specialSkillInvoker;

    void Awake()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {

    }
}
