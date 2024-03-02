using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool isUsing = false;
    public int index;

    [Title("Object")]
    public Sprite item;
    public Sprite UI;
    [SerializeField] GameObject breakEffect;

    [Title("Stat")]
    public float damage;
    public float attackCooltime;
    public int maxDurability;
    public int durability;

    Vector2 breakPos;

    void Update()
    {
        #region 입력
        if (!isUsing) return;

        if (Player.pCon.mouse0Down) Mouse0Down();
        if (Player.pCon.mouse0) Mouse0();
        if (Player.pCon.mouse0Up) Mouse0Up();
        if (Player.pCon.mouse1Down) Mouse1Down();
        if (Player.pCon.mouse1) Mouse1();
        if (Player.pCon.mouse1Up) Mouse1Up();

        if (Player.pCon.attackDown) AttackDown();
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
    public virtual void SetData(string[] datas) { }
    public virtual string LoadData() { return ""; }
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
        Player.wCon.inventoryUI.ResetDurabilityUI();
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