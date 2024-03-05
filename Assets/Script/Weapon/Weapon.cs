using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    const string nullDurabilityText = "n";

    public bool isUsing = false;

    public bool dropable = true;
    public bool useAttackInput = true;

    public int index = 0;

    [Title("Object")]
    [ShowIf("dropable", true)] public Sprite item;
    public Sprite UI;

    [Title("Stat")]
    public float damage = 1;
    [ShowIf("useAttackInput", true)] public float attackCooltime = 0;

    [Title("Break")]
    public bool breakable = true;
    [ShowIf("breakable", true)][HorizontalGroup] public int durability = 1;
    [ShowIf("breakable", true)][HorizontalGroup][HideLabel] public int maxDurability = 1;
    [ShowIf("breakable", true)] [SerializeField] protected BreakParticle breakParticle = null;



    void Update()
    {
        #region 입력
        if (!isUsing) return;

        if (Player.pCon.mouse0Down) Mouse0Down();
        if (Player.pCon.mouse0Stay) Mouse0();
        if (Player.pCon.mouse0Up) Mouse0Up();
        if (Player.pCon.mouse1Down) Mouse1Down();
        if (Player.pCon.mouse1) Mouse1();
        if (Player.pCon.mouse1Up) Mouse1Up();

        if (Player.pCon.attack) Attack();
        #endregion
    }

    #region 입력
    public virtual void Attack() { }
    public virtual void Mouse0Down() { }
    public virtual void Mouse0() { }
    public virtual void Mouse0Up() { }
    public virtual void Mouse1Down() { }
    public virtual void Mouse1() { }
    public virtual void Mouse1Up() { }
    #endregion

    #region 데이터
    public void SetData(string[] datas, string[] customDatas)
    {
        if (datas[1] == nullDurabilityText) durability = maxDurability;
        else durability = int.Parse(datas[1]);
        SetCustomData(customDatas);
    }
    protected virtual void SetCustomData(string[] datas) { }
    public virtual string LoadCustomData() { return ""; }
    #endregion

    #region 도구
    public void SetUse(bool use)
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
        if (!breakable) return;

        durability += add;
        if (durability <= 0)
        {
            WeaponBreak();
            return;
        }
        Player.wCon.inventoryUI.ResetDurabilityUI();
    }
    /// <summary>
    /// 인벤토리에서 무기 제거
    /// </summary>
    public void Drop()
    {
        if (!dropable) return;

        GameObject item = ItemManager.SpawnItem(this, Player.transform.position, LoadCustomData());
        Remove();
        Destroy();
    }
    public void Remove()
    {
        OnWeaponRemoved();
        Player.wCon.RemoveWeapon(index);
    }
    public void Destroy()
    {
        if(transform.parent == Player.wCon.transform) 
        {
            Debug.LogWarning("인벤토리에서 제거되지 않은 무기를 제거할 수 없습니다");
            Remove();
        } //LogWarning: 인벤토리에서 제거되지 않은 무기를 제거할 수 없습니다
        OnWeaponDestroyed();
        Destroy(gameObject);
    }
    #endregion

    #region 이벤트
    protected virtual void OnUse() { }
    protected virtual void OnDeUse() { }
    protected abstract void WeaponBreak();
    protected abstract void OnWeaponRemoved();
    protected abstract void OnWeaponDestroyed();
    #endregion


}