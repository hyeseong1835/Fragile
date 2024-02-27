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

    //플레이어 컴포넌트
    UnityEngine.Transform pTransform;

    //직렬화
    public GameObject item;
    public GameObject UI;
    [SerializeField] GameObject breakEffect;
    public Vector2 breakPos;

    void Awake()
    {
        //변수 세팅
        pTransform = transform.parent.parent;

        index = Player.wCon.weaponHolder.childCount - 1;
    }
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
    public void DestroyWeapon()
    {
        breakEffect.transform.parent = null;

        //Weapon
        if (index == Player.wCon.weaponHolder.childCount - 1) //마지막 순서의 무기일 때
        {
            if (Player.wCon.weaponHolder.childCount == 2) //무기가 하나일 때
            {
                Player.wCon.SelectWeapon(0);
            }
            else Player.wCon.SelectWeapon(index - 1); //무기가 더 있을 때
        }
        else Player.wCon.SelectWeapon(index + 1);

        Player.inventoryUI.RemoveToInventory(this);
        Player.wCon.DelayDestroy();

        //효과
        if (breakPos != Vector2.positiveInfinity)
        {
            breakEffect.SetActive(true);
            breakEffect.transform.SetParent(null);
            breakEffect.transform.position = breakPos;
        }
        Destroy(gameObject);
    }
    public void AddDurability(int add)
    {
        durability += add;
        if (durability <= 0)
        {
            DestroyWeapon();
            return;
        }
        UI.transform.parent.GetComponent<UI_Inventory>().ResetDurabilityUI();
    }
    public virtual void OnUse() { }
    public virtual void OnDeUse() { }
    #endregion
}