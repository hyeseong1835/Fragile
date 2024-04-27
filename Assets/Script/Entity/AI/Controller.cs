using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public struct ControllerData
{
    //좌표
    public Vector2 pos;
    //기본 무기(데이터)
    public WeaponData defaultWeaponData;
    //현재 무기(인덱스)
    public int curWeaponIndex;
    //인벤토리(무기(데이터))
    public WeaponData[] weaponDatas;

    public ControllerData(
        Vector2 _pos, 
        WeaponData _defaultWeaponData, 
        WeaponData[] _weaponDatas, int _curWeaponIndex
        )
    {
        pos = _pos;
        defaultWeaponData = _defaultWeaponData;
        weaponDatas = _weaponDatas;
        curWeaponIndex = _curWeaponIndex;
    }
}
[ExecuteAlways]
public abstract class Controller : MonoBehaviour
{
    [Required]
        public Grafic grafic;

    [Required]
        public Transform weaponHolder;
    [ShowInInspector] public static string weaponHolderName { get { return "WeaponHolder"; } }

    public Vector2 targetPos;
    public Vector2 targetDir 
    { 
        get 
        {
            return new Vector2(
                targetPos.x - transform.position.x,
                targetPos.y - transform.position.y).normalized; 
        } 
    }

    public Vector2 moveVector = new Vector3(0.5f, 0, 0);
    public Vector2 lastMoveVector = new Vector3(0.5f, 0, 0);

    [Range(0f, 360f)] public float moveRotate = 0;
    
    //입력
    public bool attack = false;
    public bool special = false;

    [BoxGroup("Weapon")]
    #region Box Weapon

        [VerticalGroup("Weapon/DefaultWeapon")]
        [HorizontalGroup("Weapon/DefaultWeapon/Horizontal")]
        #region Horizontal DefaultWeapon

            [VerticalGroup("Weapon/DefaultWeapon/Horizontal/Vertical", PaddingBottom = 25)][ShowInInspector][ReadOnly]
                [Required]
                public Weapon defaultWeapon;

            #if UNITY_EDITOR
            [HorizontalGroup("Weapon/DefaultWeapon/Horizontal", width:150)]
                [Button(name:"Set")]
                void SetDefaultWeapon(int index)
                {
                    // 제거(-1)
                    if (index == -1)
                    {
                        if (curWeapon == defaultWeapon)
                        {
                            curWeapon = null;
                        }
                        weapons.Add(defaultWeapon);
                        defaultWeapon = null;
                    }
                    else
                    {
                        Weapon weapon = weaponHolder.GetChild(index).GetComponent<Weapon>();

                        //현재 무기가 없으면 자동 선택
                        if (curWeapon == null)
                        {
                            curWeapon = weapon;
                            SelectWeapon(weapon);
                        }

                        //인벤토리에서 자동 제거
                        if (weapons.Contains(weapon))
                        {
                            weapons.Remove(weapon);
                        }
                        defaultWeapon = weapon;
                        defaultWeapon.transform.SetAsFirstSibling();
                    }
                }

            #endif

        #endregion


        [HorizontalGroup("Weapon/CurWeapon")]
        #region Horizontal CurWeapon

            [VerticalGroup("Weapon/CurWeapon/Vertical", PaddingBottom = 25)]
                [ReadOnly][Required]
                    public Weapon curWeapon;
            
            #if UNITY_EDITOR

            [HorizontalGroup("Weapon/CurWeapon", width:150)]
                [Button(name:"Set")]
                void SetCurWeapon(int index)
                {
                    if (transform.childCount == 0) return;

                    SelectWeapon(weaponHolder.GetChild(index).GetComponent<Weapon>());
                }

            #endif

        #endregion

        [VerticalGroup("Weapon/Inventory")]
        [HorizontalGroup("Weapon/Inventory/Horizontal")]
        #region Horizontal Inventory

                [ReadOnly]
                public List<Weapon> weapons = new List<Weapon>();

            
            [HorizontalGroup("Weapon/Inventory/Horizontal", width: 30)]
            [VerticalGroup("Weapon/Inventory/Horizontal/Manage")]
            #region Vertical Manage        
            
                    [HideLabel]
                    public int inventorySize = 0;
                    /*{
                        get { return defaultInventorySize; }
                        set
                        {
                            for (int i = weapons.Count - 1; i >= defaultInventorySize; i--)
                            {
                                DropWeapon(weapons[i]);
                            }
                            defaultInventorySize = value;
                        }
                    } int defaultInventorySize;*/

                #if UNITY_EDITOR
                [VerticalGroup("Weapon/Inventory/Horizontal/Manage")]
                    [HideLabel]
                    [Button(name: "Clear", Stretch = true)]
                    void ClearInventory()
                    {
                        //기본 무기 제거
                        if (defaultWeapon != null)
                        {
                            if (EditorApplication.isPlaying) Destroy(defaultWeapon.gameObject);
                            else DestroyImmediate(defaultWeapon.gameObject);

                            defaultWeapon = null;
                        }

                        //인벤토리 무기 제거
                        for (int i = weapons.Count - 1; i >= 0; i--)
                        {
                            if (weapons[i] == null) continue;
                            
                            if (EditorApplication.isPlaying) Destroy(weapons[i].gameObject);
                            else DestroyImmediate(weapons[i].gameObject);
                        }
                        weapons.Clear();
                        
                        defaultWeapon = null;
                        curWeapon = null;

                        //확실히 제거 (즉시 파괴 때문에 인덱스 꼬임)
                        /*
                        Transform weaponHolder = transform.Find(Weapon.weaponHolderName);
                        for (int i = 0; i < weaponHolder.childCount; i++)
                        {
                            if (EditorApplication.isPlaying) Destroy(weaponHolder.GetChild(i).gameObject);
                            else DestroyImmediate(weaponHolder.GetChild(i).gameObject);
                        }
                        foreach(Weapon weapon in weapons)
                        {
                            weapons.Remove(weapon); 
                        }
                        if (weapons.Count > 0) Debug.LogWarning(weapons.Count + "개가 무기 리스트에서 제거되지 않음.");
                        weapons.Clear();
                        */
                    }
                #endif

            #endregion

            #if UNITY_EDITOR

            [HorizontalGroup("Weapon/Inventory/Manage")]
            #region Horizontal Manage

                    [Button(name: "Add")]
                    void AddWeaponInInspector(string weaponName)
                    {
                        AddWeapon(Utility.SpawnWeapon(weaponName));
                    }

                [HorizontalGroup("Weapon/Inventory/Manage")]
                    [Button(name: "Destroy")]
                    void DestroyWeaponInInspector(int index)
                    {
                        if (index == -1)
                        {
                            if (defaultWeapon == null) return;

                            if (EditorApplication.isPlaying) Destroy(defaultWeapon.gameObject);
                            else DestroyImmediate(defaultWeapon.gameObject);

                            defaultWeapon = null;
                        }
                        else
                        {
                            if (weapons[index] == null)
                            {
                                weapons.RemoveAt(index);
                                return;
                            }

                            weapons[index].Destroy();
                        }
                    }

    #endregion

#endif

    #endregion

    #endregion

    void LateUpdate()
    {
        AutoDebug();
    }
    public void AutoDebug()
    {
        //개수 비교 >? 초기화
        if (weaponHolder.childCount != weapons.Count + Convert.ToInt32(defaultWeapon != null))
        {
            if (defaultWeapon != null) defaultWeapon.transform.SetAsFirstSibling();

            weapons.Clear();
            for (int i = Convert.ToInt32(defaultWeapon != null); i < weaponHolder.childCount; i++)
            {
                weapons.Add(weaponHolder.GetChild(i).GetComponent<Weapon>());
            }
            return;
        }

        //무기 검사
        for (int weaponIndex = weapons.Count - 1; weaponIndex >= 0; weaponIndex--)
        {
            //공란 삭제
            if (weapons[weaponIndex] == null) weapons.RemoveAt(weaponIndex);
            
            //기본 무기 최상단
            if (weapons[weaponIndex] == defaultWeapon) defaultWeapon.transform.SetAsFirstSibling();

            //다르면 초기화
            if (transform.GetSiblingIndex() != weaponIndex + Convert.ToInt32(defaultWeapon != null))
            {
                if (defaultWeapon != null) defaultWeapon.transform.SetAsFirstSibling();

                weapons.Clear();
                for (int i = Convert.ToInt32(defaultWeapon != null); i < weaponHolder.childCount; i++)
                {
                    weapons.Add(weaponHolder.GetChild(i).GetComponent<Weapon>());
                }
                return;
            }
        }
    }
    public ControllerData GetData()
    {
        WeaponData[] weaponDatas = new WeaponData[weapons.Count];

        for (int weaponIndex = 0; weaponIndex < weapons.Count; weaponIndex++)
        {
            for (int weaponDataIndex = 0; weaponDataIndex < weapons.Count; weaponDataIndex++)
            {
                weaponDatas[weaponDataIndex] = weapons[weaponIndex].GetData();
            }
        }

        return new ControllerData
            (
                (Vector2)transform.position,
                defaultWeapon.GetData(),
                weaponDatas,
                weapons.IndexOf(curWeapon)
            );

    }
    public void SetData(ControllerData data)
    {
        transform.position = data.pos;
        defaultWeapon = Utility.LoadWeapon(data.defaultWeaponData);
        foreach (WeaponData weaponData in data.weaponDatas)
        {
            AddWeapon(Utility.LoadWeapon(weaponData));
        }
        SelectWeapon(weapons[data.curWeaponIndex]);
    }
    /// <summary>
    /// 무기 추가 >> INVENTORY
    /// </summary>
    /// <param name="weapon">무기 상태에 대해 안전하지 않음</param>
    public void AddWeapon(Weapon weapon)
    {
        if (weapon.state == WeaponState.HOLD
            || weapon.state == WeaponState.INVENTORY)
        {
            Debug.LogWarning("먼저 인벤토리에서 제거된 후 추가해야함");
            return;
        } //LogWarning: 먼저 인벤토리에서 제거된 후 추가해야함 >> return
        
        if (weapons.Contains(weapon))
        {
            Debug.LogWarning("이미 인벤토리에 있음");
            return;
        } //LogWarning: 이미 인벤토리에 있음 >> return

        if (weapons.Count > inventorySize)
        {
            Debug.LogWarning("인벤토리가 꽉 참");
            if (weapon.state == WeaponState.PREFAB) weapon.Destroy();
            return;
        } //LogWarning: 인벤토리가 꽉 참 >> return


        weapon.con = this;

        weapon.transform.parent = weaponHolder;
        #if UNITY_EDITOR
        weapon.parent = weaponHolder;
        #endif
        weapon.state = WeaponState.INVENTORY;

        weapons.Add(weapon);
    }

    /// <summary>
    /// 무기 드랍
    /// HOLD -> INVENTORY -> REMOVED -> ITEM >>  //
    /// </summary>
    /// <param name="weapon"></param>
    public void DropWeapon(Weapon weapon)
    {
        Item item = null;

        if (weapon.state == WeaponState.ITEM)
        {
            item = weapon.transform.parent.GetComponent<Item>();
        }
        else
        {
            RemoveWeapon(weapon);
            item = ItemManager.WrapWeaponInItem(weapon);
        }

        item.transform.position = transform.position;

        //던지는 효과----------------------------------------------------
    }

    /// <summary>
    /// Hold -> Inventory >> 인벤토리에서 무기 제거 >> weapon.OnWeaponRemoved() >> 부모 제거, weapon.state = REMOVED >> 새 무기 선택 //
    /// </summary>
    /// <param name="weapon"></param>
    public void RemoveWeapon(Weapon weapon)
    {
        Debug.Log("Remove: "+weapon);
        //기본 무기를 제거할 때
        if (weapon == defaultWeapon)
        {
            Debug.LogError("기본 무기는 제거할 수 없음");
            return;
        } //LogError: 기본 무기는 제거할 수 없음 >> return

        if (weapons.Contains(weapon) == false)
        {
            Debug.LogError("{" + weapon.name + "}을(를) 인벤토리에서 찾을 수 없어 제거하지 못함.");
            return;
        } //LogError: {weapon.name}을(를) 인벤토리에서 찾을 수 없어 제거하지 못함. >> return

        if (weapon.state == WeaponState.HOLD)
        {
            weapon.SetUse(false);
        } //HOLD -> INVENTORY

        if (weapon.state == WeaponState.INVENTORY)
        {
            int index = weapons.IndexOf(weapon);

            weapon.OnWeaponRemoved();
            
            weapons.Remove(weapon);

            weapon.transform.parent = null;
            weapon.state = WeaponState.REMOVED;

            //무기 선택
            if (index == weapons.Count) //마지막 순서의 무기일 때
            {
                if (weapons.Count == 0)
                {
                    if (defaultWeapon != null) SelectWeapon(defaultWeapon);
                }
                else SelectWeapon(weapons[0]);
            }
            else
            {
                if (weapons[index] != null) SelectWeapon(weapons[index]);
                else if (defaultWeapon != null) SelectWeapon(defaultWeapon);
            }
            return;
        }
    }

    protected void SelectWeapon(Weapon weapon)
    {
        if (weapons.Contains(weapon) == false)
        {
            if (curWeapon.state == WeaponState.HOLD) curWeapon.SetUse(false);
            if (defaultWeapon != null)
            {
                if (defaultWeapon.state == WeaponState.INVENTORY) defaultWeapon.SetUse(true);
            }
            return;
        } //LogError: {weapon.name}을(를) 인벤토리에서 찾을 수 없어 선택하지 못함. >> return

        if (curWeapon != null && curWeapon.state == WeaponState.HOLD) curWeapon.SetUse(false); //현재 무기 비활성화

        curWeapon = weapon;
        if (curWeapon.state == WeaponState.INVENTORY) weapon.SetUse(true); //선택한 무기 활성화
    }
}
