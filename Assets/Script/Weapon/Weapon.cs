using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.Collections.Generic;
using Unity.VisualScripting;

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

    [Title("Object")]
    [Required]
    public Sprite UI;
    
    [SerializeField] protected Transform hand_obj;
    [SerializeField] protected BreakParticle breakParticle;


    [Title("Stat")]
    public float damage = 1;
    public float attackCooltime = 0;

    [HorizontalGroup("Durability")] 
        public int durability = 1;
    
    [HorizontalGroup("Durability", width: 150)]
        [HideLabel] 
        public int maxDurability = 1;

    #if UNITY_EDITOR
    [HideInInspector] public Transform parent = null;
    int prevChildIndex;
    #endif

    void Awake()
    {
        #if UNITY_EDITOR
        //프리팹 수정 창이 아닌 경우
        if (PrefabUtility.GetPrefabInstanceStatus(gameObject) != PrefabInstanceStatus.NotAPrefab
            || EditorApplication.isPlaying)
        {
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

                        if (parent == null) parent = transform.parent;
                        prevChildIndex = transform.GetSiblingIndex();
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
            }
        } //드롭 다운 위치에 따른 빠른 수정
        
        #endif

        WeaponAwake();
    }
    void Start()
    {
        WeaponStart();
    }
    void LateUpdate()
    {
        #if UNITY_EDITOR

        if (PrefabUtility.GetPrefabInstanceStatus(gameObject) != PrefabInstanceStatus.NotAPrefab
            || EditorApplication.isPlaying)
        {
            switch (state)
            {
                case WeaponState.PREFAB:

                    if (transform.parent != null)
                    {
                        transform.parent.position = transform.position;
                        state = WeaponState.ITEM;
                    } //드롭다운 >> 부모 위치 수정(1회)

                    gameObject.name = weaponName + "[Prefab]";
                    break;
                case WeaponState.HOLD:
                    gameObject.name = "[Hold] ";
                    gameObject.name += weaponName + "(" + con.gameObject.name;
                    if (this == con.defaultWeapon) gameObject.name += "(Default)";
                    gameObject.name += ")";

                    WeaponUpdate();
                    break;
                case WeaponState.INVENTORY:
                    gameObject.name = weaponName + "(" + con.gameObject.name;
                    if (this == con.defaultWeapon) gameObject.name += "(Default)";
                    gameObject.name += ")";

                    WeaponBackGroundUpdate();
                    break;
                case WeaponState.ITEM:
                    gameObject.name = weaponName + "[Item]";
                    break;
                case WeaponState.REMOVED:
                    gameObject.name = weaponName + "[Removed(" + con.gameObject.name + ")]";
                    break;
                case WeaponState.NULL:
                    gameObject.name = weaponName + "[NULL]";
                    break;
            }

            AutoDebug();
        }
        #endif
    }
    void OnDrawGizmos()
    {
        WeaponOnDrawGizmos();
    }
    protected virtual void WeaponAwake() { }
    protected virtual void WeaponStart() { }
    protected virtual void WeaponUpdate() { }
    protected virtual void WeaponBackGroundUpdate() { }
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
    
    public virtual WeaponData GetData()
    {
        return new WeaponData
            (
                weaponName,
                durability,
                null
            );
    }
    public virtual void SetData(WeaponData data)
    {
        weaponName = data.name;
        durability = data.durability;
    }

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

            hand_obj.gameObject.SetActive(true);
            con.grafic.HandLink(HandMode.ToHand, hand_obj);
            OnUse();
         
            state = WeaponState.HOLD;
        }
        else
        {
            if (state == WeaponState.INVENTORY)
            {
                Debug.LogWarning("이미 비활성화되었습니다.");
            } //LogWarning: 이미 비활성화되었습니다.

            hand_obj.gameObject.SetActive(false);
            con.grafic.HandLink(null);
            OnDeUse();
        
            state = WeaponState.INVENTORY;
        }
    }

    public void AddDurability(int add)
    {
        if (durability == -1) return;

        durability += add;
        if (durability == 0)
        {
            Break();
            return;
        }
    }

    /// <summary>
    /// 무기를 파괴하는 함수
    /// INVENTORY -> HOLD -> REMOVED >> OnWeaponDestroy() >> Destroy(gameObject) //
    /// ITEM >> OnWeaponDestroy(item) >> Destroy //
    /// PREFAB >> DestroyImmediate(gameObject) //
    /// </summary>
    public void Destroy()
    {
        //PREFAB >> Destroy(gameObject) >> return
        if (state == WeaponState.PREFAB)
        {
            if (EditorApplication.isPlaying) DestroyImmediate(gameObject);
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

    #if UNITY_EDITOR
    public void AutoDebug()
    {
        switch (state)
        {
            // 부모 검사
            case WeaponState.ITEM:
                if (transform.parent == null)
                {
                    Debug.LogError("부모가 없습니다.");
                    break;
                }

                if (transform.parent.gameObject.name != "Item(" + weaponName + ")")
                {
                    Debug.LogError("부모의 이름이 올바르지 않습니다.");
                    break;
                }//LogError: 부모의 이름이 올바르지 않습니다.

                transform.localPosition = Vector3.zero;
                break;

            //부모 변경 억제
            case WeaponState.HOLD:
                //부모 변경 억제
                if (transform.parent != parent)
                {
                    Debug.LogWarning("하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.");

                    transform.parent = parent;
                    transform.SetSiblingIndex(prevChildIndex);
                } //LogWarning: 하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.

                break;

            //부모 변경 억제
            case WeaponState.INVENTORY:
                //부모 변경 억제
                if (transform.parent != parent)
                {
                    Debug.LogWarning("하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.");

                    transform.parent = parent;
                    transform.SetSiblingIndex(prevChildIndex);
                } //LogWarning: 하이어라키에서 무기를 다른 오브젝트로 옮길 수 없습니다.

                break;
        }
     
        prevChildIndex = transform.GetSiblingIndex();
    }
    #endif
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