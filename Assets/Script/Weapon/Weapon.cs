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
            //하이어라키에 드롭다운 >> Controller에 추가
            if (transform.parent != null)
            {
                //부모가 WeaponHolder
                if (transform.parent.gameObject.name == Controller.weaponHolderName)
                {
                    con = transform.parent.parent.GetComponent<Controller>();

                    if (con.weapons.Count >= con.inventorySize)
                    {
                        Debug.LogWarning("인벤토리가 가득참.");

                        if (EditorApplication.isPlaying) Destroy(gameObject);
                        else DestroyImmediate(gameObject);
                        
                        return;
                    }
                    con.AddWeapon(this);

                    parent = transform.parent;
                }
                else
                {
                    Debug.LogError("아이템이 아닌 무기는 항상 \"WeaponHolder\"안에 있어야 합니다.");
                } //LogError: 아이템이 아닌 무기는 항상 "WeaponHolder"안에 있어야 합니다.
            }
            //씬뷰에 플로팅 시작 >> 아이템화
            else
            {
                ItemManager.WrapWeaponInItem(this);
            }
        } //드롭 다운 위치에 따른 빠른 수정

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
                    Debug.LogError("장착된 무기는 항상 \"WeaponHolder\"안에 있어야 합니다.");
                    return;
                } //LogError: 장착된 무기는 항상 "WeaponHolder"안에 있어야 합니다.
                
                transform.parent.position = transform.position;

                break; //드롭다운 >> 부모 위치 수정(1회)

            case WeaponState.ITEM:
                //부모가 유효하지 않음
                if (!transform.parent.gameObject.name.Contains(ItemManager.itemSuffix))
                {
                    Debug.LogError("아이템이 아닌 무기는 항상 \"WeaponHolder\"안에 있어야 합니다.");
                    break;
                } //LogError: 아이템 상태인 무기는 항상 Item으로 포장되어 있어야 합니다.

                transform.localPosition = Vector3.zero;
                
                break; //아이템화 >> 무기 위치 고정
        }

        #endif

        WeaponUpdate();
    }
    void LateUpdate()
    {
        #if UNITY_EDITOR
        
        #region 자동 디버깅
        switch (state)
        {
            case WeaponState.ITEM:
                //LogError: 부모의 이름이 올바르지 않습니다.
                if (transform.parent.gameObject.name != weaponName + ItemManager.itemSuffix)
                {
                    Debug.LogError("부모의 이름이 올바르지 않습니다.");
                } 

                break; 

            case WeaponState.HOLD:
                if (transform.parent != parent)
                {
                    Debug.LogWarning("하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.");

                    transform.parent = parent;
                    transform.SetSiblingIndex(prevChildIndex);
                } //LogWarning: 하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.

                if (con.defaultWeapon != null)
                {
                    if (this != con.defaultWeapon 
                        && transform.GetSiblingIndex() != con.weapons.IndexOf(this) + 1)
                    {
                        Debug.Log("다름");
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
                        Debug.Log("다름");
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
                    Debug.LogWarning("하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.");

                    transform.parent = parent;
                    transform.SetSiblingIndex(prevChildIndex);
                } //LogWarning: 하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.

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
                Debug.LogError("무기 상태가 없습니다.");
                if (EditorApplication.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
                break; //LogError: 무기 상태가 없습니다. >> Destroy
            case WeaponState.PREFAB:
                Debug.LogError("무기는 프리팹 상태로 존재할 수 없습니다.");
                if (EditorApplication.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
                break; //LogError: 무기는 프리팹 상태로 존재할 수 없습니다. >> Destroy
            default:
                Debug.LogError("무기 상태가 유효하지 않습니다.");
                if (EditorApplication.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
                break; //LogError: 무기 상태가 유효하지 않습니다. >> Destroy
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

    #region 입력
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

    #region 데이터
    
    public abstract WeaponData GetData();
    public abstract void SetData(WeaponData data);

    #endregion

    #region 도구
    /// <summary>
    /// true: INVENTORY -> HOLD //
    /// false: HOLD -> INVENTORY //
    /// </summary>
    /// <param name="use"></param>
    public void SetUse(bool use)
    {
        if (state == WeaponState.PREFAB)
        {
            Debug.LogError("무기를 활성화할 수 없습니다.");
            return;
        }// LogError: 무기를 활성화할 수 없습니다. >> return
        if (state == WeaponState.ITEM)
        {
            Debug.LogError("무기를 활성화할 수 없습니다.");
            return;
        } //LogError: 무기를 활성화할 수 없습니다 >> return
        
        if (use)
        {
            if(state == WeaponState.REMOVED)
            {
                Debug.LogError("무기를 활성화할 수 없습니다.");
                return;
            } //LogError: 무기를 활성화할 수 없습니다. >> return

            if (state == WeaponState.HOLD)
            {
                Debug.LogWarning("이미 활성화되었습니다.");
            } //LogWarning: 이미 활성화되었습니다.

            OnUse();
         
            state = WeaponState.HOLD;
        }
        else
        {
            if (state == WeaponState.INVENTORY)
            {
                Debug.LogWarning("이미 비활성화되었습니다.");
            } //LogWarning: 이미 비활성화되었습니다.

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

        Debug.LogError("무기 상태가 유효하지 않습니다.");
    }

    #endregion

    #region 이벤트

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
            Debug.LogError("무기 상태가 유효하지 않습니다.");
            return; //LogError: 무기 상태가 유효하지 않습니다.
    }

*/