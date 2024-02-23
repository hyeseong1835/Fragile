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
    //�⺻
    public bool isUsing = false;
    public int index;

    //����
    public float damage;
    public int maxDurability;
    public int durability;
    public float attackCooltime;

    public int attackStack = 0;
    public float attackCool = 0;

    //�÷��̾� ������Ʈ
    UnityEngine.Transform pTransform;
    protected Player player;
    protected PlayerController pCon;
    protected WeaponController wCon;

    //����ȭ
    public GameObject item;
    public GameObject UI;
    [SerializeField] GameObject breakEffect;
    public Vector2 breakPos;

    void Awake()
    {
        //���� ����
        pTransform = transform.parent.parent;
        player = pTransform.GetComponent<Player>();
        pCon = pTransform.GetComponent<PlayerController>();

        wCon = pTransform.GetComponent<WeaponController>();
        UI = wCon.inventoryUI.AddToInventory(this);
        index = wCon.weaponHolder.childCount - 1;
    }
    void Update()
    {
        if (attackCool > 0) attackCool -= Time.deltaTime;
        if (attackCool < 0) attackCool = 0;

        #region �Է�
        if (!isUsing) return;

        if (pCon.mouse0Down) Mouse0Down();
        if (pCon.mouse0) Mouse0();
        if (pCon.mouse0Up) Mouse0Up();
        if (pCon.mouse1Down) Mouse1Down();
        if (pCon.mouse1) Mouse1();
        if (pCon.mouse1Up) Mouse1Up();

        if (pCon.attackDown) AttackDown();
        if (pCon.attack) Attack();
        if (pCon.attackUp) AttackUp();
        #endregion
    }

    #region �Է�
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


    #region ���� ����
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
        if (index == wCon.weaponHolder.childCount - 1) //������ ������ ������ ��
        {
            if (wCon.weaponHolder.childCount == 2) //���Ⱑ �ϳ��� ��
            {
                wCon.SelectWeapon(0);
            }
            else wCon.SelectWeapon(index - 1); //���Ⱑ �� ���� ��
        }
        else wCon.SelectWeapon(index + 1);

        wCon.inventoryUI.RemoveToInventory(this);
        wCon.DelayDestroy();

        //ȿ��
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