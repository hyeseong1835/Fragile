using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class Weapon : MonoBehaviour
{
    //기본
    public bool isUsing = false;
    public int index;

    //스탯
    public float damage;
    public int maxDurability;
    public int durability;
    public float attackCooltime;

    public int attackStack = 0;
    public float attackCool = 0;

    public GameObject item;
    public Sprite UI;
    [SerializeField] GameObject breakEffect;
    public Vector2 breakPos;

    void Update()
    {
        if (attackCool > 0) attackCool -= Time.deltaTime;
        if (attackCool < 0) attackCool = 0;

        #region 입력
        if (!isUsing) return;

        if (Player.pCon.mouse0Down) Mouse0Down();
        if (Player.pCon.mouse0) Mouse0();
        if (Player.pCon.mouse0Up) Mouse0Up();
        if (Player.pCon.mouse1Down) Mouse1Down();
        if (Player.pCon.mouse1) Mouse1();
        if (Player.pCon.mouse1Up) Mouse1Up();

        if (Player.pCon.attackDown) AttackDown();
        if (Player.pCon.attack) Attack();
        if (Player.pCon.attackUp) AttackUp();
        #endregion
    }

    #region 입력
    public virtual void AttackDown() { }
    public virtual void Attack() { }
    public virtual void AttackUp() { }
    public virtual void Mouse0Down() { }
    public virtual void Mouse0() { }
    public virtual void Mouse0Up() { }
    public virtual void Mouse1Down() { }
    public virtual void Mouse1() { }
    public virtual void Mouse1Up() { }
    #endregion


    #region 무기 관리
    public virtual void SetData(string[] datas)
    {
        if (0 < datas.Length) durability = int.Parse(datas[0]);
    }
    public virtual string LoadData()
    {
        return durability.ToString();
    }

    public void Use(bool use)
    {
        if (use)
        {
            isUsing = true;
            OnUse();
        }
        else
        {
            isUsing = false;
            OnDeUse();
        }
    }
    public void AddDurability(int add)
    {
        durability += add;
        if (durability <= 0)
        {
            breakEffect.SetActive(true);
            breakEffect.transform.parent = null;
            breakEffect.transform.position = breakPos;
            Player.wCon.RemoveWeapon(index);
            return;
        }
        WeaponController.inventoryUI.ResetDurabilityUI();
    }
    public virtual void OnUse() { }
    public virtual void OnDeUse() { }
    public virtual void OnWeaponDestroy() { }
    public virtual void OnWeaponBreak() 
    {
        Player.wCon.RemoveWeapon(index);
    }
    #endregion
}