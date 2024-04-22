using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public abstract class Weapon : MonoBehaviour
{
    public GameObject weaponPrefab;
    public Controller con;

    public string weaponName;
    public bool isUsing = false;
    public bool dropable = true;
    public bool useAttackInput = true;

    public int index = 0;

    [Title("Object")]
    [ShowIf("dropable", true)] public Sprite item;
    public Sprite UI;
    public Transform handWeapon;

    [Title("Stat")]
    public float damage = 1;
    [ShowIf("useAttackInput", true)] public float attackCooltime = 0;

    [Title("Break")]
    public bool breakable = true;
    [ShowIf("breakable", true)][HorizontalGroup] public int durability = 1;
    [ShowIf("breakable", true)][HorizontalGroup][HideLabel] public int maxDurability = 1;
    [ShowIf("breakable", true)] [SerializeField] protected BreakParticle breakParticle = null;

    void Awake()
    {
        WeaponAwake();
    }
    void Start()
    {
        WeaponStart();
    }
    private void FixedUpdate()
    {
        WeaponFixedUpdate();
    }
    void Update()
    {
        #region �Է�
        if (!isUsing) return;


        #endregion

        WeaponUpdate();
    }
    void LateUpdate()
    {
        WeaponLateUpdate();
    }
    void OnDrawGizmos()
    {
        WeaponOnDrawGizmos();
    }
    public virtual void WeaponAwake() { }
    public virtual void WeaponStart() { }
    public virtual void WeaponFixedUpdate() { }
    public virtual void WeaponUpdate() { }
    public virtual void WeaponLateUpdate() { }
    public virtual void WeaponOnDrawGizmos() { }

    #region �Է�
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    public virtual void Attack() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse0"), Button(ButtonStyle.Box)]
    public virtual void Mouse0Down() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse0"), Button(ButtonStyle.Box)]
    public virtual void Mouse0() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse0"), Button(ButtonStyle.Box)]
    public virtual void Mouse0Up() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse1"), Button(ButtonStyle.Box)]
    public virtual void Mouse1Down() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse1"), Button(ButtonStyle.Box)]
    public virtual void Mouse1() { }
    [DisableInEditorMode]
    [HorizontalGroup("Mouse1"), Button(ButtonStyle.Box)]
    public virtual void Mouse1Up() { }
    #endregion

    #region ������
    
    public abstract string[] GetData();
    public abstract void SetData(string[] data);

    #endregion

    #region ����
    public void SetUse(bool use)
    {
        if (use)
        {
            handWeapon.gameObject.SetActive(true);
            con.grafic.HandLink(HandMode.ToHand, handWeapon);

            isUsing = true;
            OnUse();
        }
        else
        {
            handWeapon.gameObject.SetActive(false);
            con.grafic.HandLink(HandMode.NONE, handWeapon);

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
            Break();
            return;
        }
    }
    /// <summary>
    /// �κ��丮���� ���� ����
    /// </summary>
    public void Drop()
    {
        if (!dropable) return;

        Item item = ItemManager.WeaponToItem(this);
        item.transform.position = con.transform.position;

        Remove();
        Destroy();
    }
    public void Remove()
    {
        OnDeUse();
        OnWeaponRemoved();
        con.RemoveWeapon(index);
    }
    public void Destroy()
    {
        if(transform.parent == con.transform) 
        {
            Debug.LogWarning("�κ��丮���� ���ŵ��� ���� ���⸦ ������ �� �����ϴ�");
            Remove();
        } //LogWarning: �κ��丮���� ���ŵ��� ���� ���⸦ ������ �� �����ϴ�
        OnWeaponDestroyed();
        Destroy(gameObject);
    }
    #endregion

    #region �̺�Ʈ
    protected virtual void OnUse() { }
    protected virtual void OnDeUse() { }
    protected abstract void Break();
    protected abstract void OnWeaponRemoved();
    protected abstract void OnWeaponDestroyed();
    #endregion


}