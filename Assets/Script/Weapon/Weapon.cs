using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public enum WeaponState
{
    PREFAB, ITEM, HOLD, INVENTORY, REMOVED
}
[ExecuteAlways]
public abstract class Weapon : MonoBehaviour
{
    public WeaponState state;
    
    [ReadOnly] public Controller con;
    public string weaponName = "";
    public bool dropable = true;

    [Title("Object")]
    [ShowIf("dropable", true)]
    [Required]
        public Sprite item;
    [Required]
    public Sprite UI;

    [Title("Stat")]
    public float damage = 1;
    public float attackCooltime = 0;

    [Title("Break")]
    public bool breakable = true;
    [ShowIf("breakable", true)][HorizontalGroup] public int durability = 1;
    [ShowIf("breakable", true)][HorizontalGroup][HideLabel] public int maxDurability = 1;
    [ShowIf("breakable", true)] [SerializeField] protected BreakParticle breakParticle = null;

    void Awake()
    {
        if (transform.parent.gameObject.name == "WeaponHolder")
        {
            con = transform.parent.parent.GetComponent<Controller>();
            state = WeaponState.INVENTORY;

            con.AddWeapon(this);
        }
        else
        {
            if (state == WeaponState.PREFAB)
            {
                ItemManager.WrapWeaponInItem(this);
            }
        }
        
        //if (weaponName == "") weaponName = gameObject.name;
        
        WeaponAwake();
    }
    void Start()
    {
        WeaponStart();
    }
    void Update()
    {
        switch (state)
        {
            case WeaponState.PREFAB:
                
                if (transform.parent != null)
                {
                    transform.parent.position = transform.position;

                    state = WeaponState.ITEM;
                }
                break;
            
            case WeaponState.ITEM:
                transform.position = transform.parent.position;
                break;
            
            case WeaponState.HOLD:
                WeaponUpdate();
                break;

            case WeaponState.INVENTORY:
                
                break;
            
            case WeaponState.REMOVED:
                WeaponUpdate();
                break;
        }
    }
    void OnDrawGizmos()
    {
        WeaponOnDrawGizmos();
    }
    protected virtual void WeaponAwake() { }
    protected virtual void WeaponStart() { }
    protected virtual void WeaponUpdate() { }
    protected virtual void WeaponOnDrawGizmos() { }

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
            if (state != WeaponState.INVENTORY)
            {
                Debug.LogError("���⸦ ��Ȱ��ȭ�� �� �����ϴ�.");
                return;
            }
            state = WeaponState.HOLD;
            OnUse();
        }
        else
        {
            if (state != WeaponState.HOLD)
            {
                Debug.LogError("���⸦ Ȱ��ȭ�� �� �����ϴ�.");
                return;
            }
            state = WeaponState.INVENTORY;
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
    public void Remove()
    {
        switch (state)
        {
            case WeaponState.PREFAB:
                Debug.LogError("������ ���¿����� ���⸦ ������ �� �����ϴ�.");
                return;
            case WeaponState.ITEM:
                Debug.LogError("������ ���¿����� ���⸦ ������ �� �����ϴ�.");
                return;


            case WeaponState.INVENTORY:
                SetUse(false);
                break;
            case WeaponState.HOLD:
                SetUse(false);
                break;
        }
        
        state = WeaponState.REMOVED;

        OnWeaponRemoved();
        con.RemoveWeapon(this);
    }
    public void Drop()
    {
        
    }
    public void Destroy()
    {
        switch (state)
        {
            case WeaponState.PREFAB:
                Debug.LogError("������ ���¿����� �ı��� �� �����ϴ�.");
                break;

            case WeaponState.ITEM:
                OnWeaponDestroyed();
                Destroy(transform.parent.gameObject);
                break;

            case WeaponState.INVENTORY:
                OnWeaponDestroyed();
                Destroy(gameObject);
                break;
            
            case WeaponState.HOLD:
                SetUse(false);
                OnWeaponDestroyed();
                Destroy(gameObject);
                break;
            
            case WeaponState.REMOVED:
                OnWeaponDestroyed();
                Destroy(gameObject);
                break;

        }
        
    }
    #endregion

    #region �̺�Ʈ
    protected virtual void OnUse() 
    {
        switch (state)
        {
            case WeaponState.PREFAB:
                
                break;

            case WeaponState.ITEM:
                
                break;

            case WeaponState.HOLD:
                
                break;

            case WeaponState.INVENTORY:

                break;

            case WeaponState.REMOVED:
                
                break;
        }
    }
    protected virtual void OnDeUse() { }
    protected abstract void Break();
    protected abstract void OnWeaponRemoved();
    protected abstract void OnWeaponDestroyed();
    #endregion


}