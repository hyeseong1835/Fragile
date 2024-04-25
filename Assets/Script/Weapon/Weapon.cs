using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.Collections.Generic;

public struct WeaponData
{
    public string name;
    public int durability;
    public string weaponData;

    public WeaponData(string _name, int _durability)
    {
        name = _name;
        durability = _durability;
        weaponData = null;
    }
    public WeaponData(string _name, int _durability, string _weaponData)
    {
        name = _name;
        durability = _durability;
        weaponData = _weaponData;
    }
}
/// <summary>
/// NULL, PREFAB, ITEM, HOLD, INVENTORY, REMOVED
/// </summary>
public enum WeaponState
{
    NULL, PREFAB, ITEM, HOLD, INVENTORY, REMOVED
}
[ExecuteAlways]
public abstract class Weapon : MonoBehaviour
{
    [ReadOnly] public Controller con;

    [Required] public string weaponName;

    public WeaponState state = WeaponState.NULL;

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
    [ShowIf("breakable", true)] [HorizontalGroup] public int durability = 1;
    [ShowIf("breakable", true)] [HorizontalGroup][HideLabel] public int maxDurability = 1;
    [ShowIf("breakable", true)] [SerializeField] protected BreakParticle breakParticle = null;

    #if UNITY_EDITOR
    Transform parent;
    int prevChildIndex;
    #endif

    void Awake()
    {
        #if UNITY_EDITOR

        if (state == WeaponState.PREFAB)
        {
            //���̾��Ű�� ��Ӵٿ� >> Controller�� �߰�
            if (transform.parent != null)
            {
                //�θ� WeaponHolder
                if (transform.parent.gameObject.name == Controller.weaponHolderName)
                {
                    con = transform.parent.parent.GetComponent<Controller>();

                    if (con.weapons.Count >= con.inventorySize)
                    {
                        Debug.LogWarning("�κ��丮�� ������.");

                        if (EditorApplication.isPlaying) Destroy(gameObject);
                        else DestroyImmediate(gameObject);
                        
                        return;
                    }
                    con.AddWeapon(this);

                    parent = transform.parent;
                }
                else
                {
                    Debug.LogError("�������� �ƴ� ����� �׻� \"WeaponHolder\"�ȿ� �־�� �մϴ�.");
                } //LogError: �������� �ƴ� ����� �׻� "WeaponHolder"�ȿ� �־�� �մϴ�.
            }
            //���信 �÷��� ���� >> ������ȭ
            else
            {
                ItemManager.WrapWeaponInItem(this);
            }
        } //��� �ٿ� ��ġ�� ���� ���� ����

        #endif

        WeaponAwake();
    }
    void Start()
    {
        WeaponStart();
    }
    void Update()
    {
        #if UNITY_EDITOR

        switch (state)
        {
            case WeaponState.PREFAB: 
                
                if (transform.parent == null 
                    || transform.parent.gameObject.name != Controller.weaponHolderName)
                {
                    Debug.LogError("������ ����� �׻� \"WeaponHolder\"�ȿ� �־�� �մϴ�.");
                    return;
                } //LogError: ������ ����� �׻� "WeaponHolder"�ȿ� �־�� �մϴ�.
                
                transform.parent.position = transform.position;

                break; //��Ӵٿ� >> �θ� ��ġ ����(1ȸ)

            case WeaponState.ITEM:
                //�θ� ��ȿ���� ����
                if (!transform.parent.gameObject.name.Contains(ItemManager.itemSuffix))
                {
                    Debug.LogError("�������� �ƴ� ����� �׻� \"WeaponHolder\"�ȿ� �־�� �մϴ�.");
                    break;
                } //LogError: ������ ������ ����� �׻� Item���� ����Ǿ� �־�� �մϴ�.

                transform.localPosition = Vector3.zero;
                
                break; //������ȭ >> ���� ��ġ ����
        }

        #endif

        WeaponUpdate();
    }
    void LateUpdate()
    {
        #if UNITY_EDITOR
        
        #region �ڵ� �����
        switch (state)
        {
            case WeaponState.ITEM:
                //LogError: �θ��� �̸��� �ùٸ��� �ʽ��ϴ�.
                if (transform.parent.gameObject.name != weaponName + ItemManager.itemSuffix)
                {
                    Debug.LogError("�θ��� �̸��� �ùٸ��� �ʽ��ϴ�.");
                } 

                break; 

            case WeaponState.HOLD:
                if (transform.parent != parent)
                {
                    Debug.LogWarning("���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.");

                    transform.parent = parent;
                    transform.SetSiblingIndex(prevChildIndex);
                } //LogWarning: ���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.

                if (con.defaultWeapon != null)
                {
                    if (this != con.defaultWeapon 
                        && transform.GetSiblingIndex() != con.weapons.IndexOf(this) + 1)
                    {
                        Debug.Log("�ٸ�");
                        con.weapons = new List<Weapon>();
                        for (int i = 1; i < transform.childCount; i++)
                        {
                            con.weapons.Add(transform.GetChild(i).GetComponent<Weapon>());
                        }
                    }
                }
                else
                {
                    if (transform.GetSiblingIndex() != con.weapons.IndexOf(this))
                    {
                        Debug.Log("�ٸ�");
                        con.weapons = new List<Weapon>();
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            con.weapons.Add(transform.GetChild(i).GetComponent<Weapon>());
                        }
                    }
                }
                
                prevChildIndex = transform.GetSiblingIndex();
                break;

            case WeaponState.INVENTORY:
                if (transform.parent != parent)
                {
                    Debug.LogWarning("���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.");

                    transform.parent = parent;
                    transform.SetSiblingIndex(prevChildIndex);
                } //LogWarning: ���̾��Ű���� ���⸦ �ٸ� ������Ʈ�� �ű� �� �����ϴ�.

                if (con.defaultWeapon != null)
                {
                    if (transform.GetSiblingIndex() != con.weapons.IndexOf(this) + 1)
                    {
                        con.defaultWeapon.transform.SetAsFirstSibling();
                        con.weapons.Clear();
                        for (int i = 1; i < transform.parent.childCount; i++)
                        {
                            con.weapons.Add(transform.parent.GetChild(i).GetComponent<Weapon>());
                        }
                    }
                }
                else
                {
                    if (transform.GetSiblingIndex() != con.weapons.IndexOf(this))
                    {
                        con.weapons.Clear();
                        for (int i = 0; i < transform.parent.childCount; i++)
                        {
                            con.weapons.Add(transform.parent.GetChild(i).GetComponent<Weapon>());
                        }
                    }
                }

                prevChildIndex = transform.GetSiblingIndex();
                break;

            case WeaponState.REMOVED:

                break;

            case WeaponState.NULL:
                Debug.LogError("���� ���°� �����ϴ�.");
                if (EditorApplication.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
                break; //LogError: ���� ���°� �����ϴ�. >> Destroy
            case WeaponState.PREFAB:
                Debug.LogError("����� ������ ���·� ������ �� �����ϴ�.");
                if (EditorApplication.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
                break; //LogError: ����� ������ ���·� ������ �� �����ϴ�. >> Destroy
            default:
                Debug.LogError("���� ���°� ��ȿ���� �ʽ��ϴ�.");
                if (EditorApplication.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
                break; //LogError: ���� ���°� ��ȿ���� �ʽ��ϴ�. >> Destroy
        }
        #endregion
        #endif
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
    
    public abstract WeaponData GetData();
    public abstract void SetData(WeaponData data);

    #endregion

    #region ����
    /// <summary>
    /// true: INVENTORY -> HOLD //
    /// false: HOLD -> INVENTORY //
    /// </summary>
    /// <param name="use"></param>
    public void SetUse(bool use)
    {
        if (state == WeaponState.PREFAB)
        {
            Debug.LogError("���⸦ Ȱ��ȭ�� �� �����ϴ�.");
            return;
        }// LogError: ���⸦ Ȱ��ȭ�� �� �����ϴ�. >> return
        if (state == WeaponState.ITEM)
        {
            Debug.LogError("���⸦ Ȱ��ȭ�� �� �����ϴ�.");
            return;
        } //LogError: ���⸦ Ȱ��ȭ�� �� �����ϴ� >> return
        
        if (use)
        {
            if(state == WeaponState.REMOVED)
            {
                Debug.LogError("���⸦ Ȱ��ȭ�� �� �����ϴ�.");
                return;
            } //LogError: ���⸦ Ȱ��ȭ�� �� �����ϴ�. >> return

            if (state == WeaponState.HOLD)
            {
                Debug.LogWarning("�̹� Ȱ��ȭ�Ǿ����ϴ�.");
            } //LogWarning: �̹� Ȱ��ȭ�Ǿ����ϴ�.

            OnUse();
         
            state = WeaponState.HOLD;
        }
        else
        {
            if (state == WeaponState.INVENTORY)
            {
                Debug.LogWarning("�̹� ��Ȱ��ȭ�Ǿ����ϴ�.");
            } //LogWarning: �̹� ��Ȱ��ȭ�Ǿ����ϴ�.

            OnDeUse();
        
            state = WeaponState.INVENTORY;
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
    /// INVENTORY -> HOLD -> REMOVED >> OnWeaponDestroy() >> Destroy(gameObject) //
    /// ITEM >> OnWeaponDestroy(item) >> Destroy //
    /// PREFAB >> Destroy(Weapon) //
    /// </summary>
    public void Destroy()
    {
        //PREFAB >> Destroy(gameObject) >> return
        if (state == WeaponState.PREFAB)
        {
            if (EditorApplication.isPlaying) Destroy(gameObject);
            else DestroyImmediate(gameObject);
            return;
        }

        //ITEM >> Destroy(parent) >> return
        if (state == WeaponState.ITEM)
        {
            if (EditorApplication.isPlaying) Destroy(transform.parent.gameObject);
            else DestroyImmediate(transform.parent.gameObject);
            return;
        }

        //HOLD -> INVENTORY -> REMOVED ->
        if (state == WeaponState.HOLD
            || state == WeaponState.INVENTORY)
        {
            con.RemoveWeapon(this);
        }

        // REMOVED >> OnWeaponDestroy(item) >> Destroy(gameObject) >> return
        if (state == WeaponState.REMOVED)
        {
            OnWeaponDestroyed();
            if (EditorApplication.isPlaying) Destroy(gameObject);
            else DestroyImmediate(gameObject);
            return;
        }

        Debug.LogError("���� ���°� ��ȿ���� �ʽ��ϴ�.");
    }

    #endregion

    #region �̺�Ʈ

    protected virtual void OnUse() { }
    protected virtual void OnDeUse() { }
    protected virtual void Break() { }
    public virtual void OnWeaponRemoved() { }
    public virtual void OnWeaponDestroyed() { }
    
    #endregion
}

/*

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


        default:
            Debug.LogError("���� ���°� ��ȿ���� �ʽ��ϴ�.");
            return; //LogError: ���� ���°� ��ȿ���� �ʽ��ϴ�.
    }

*/