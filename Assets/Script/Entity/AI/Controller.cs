using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
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
public abstract class Controller : MonoBehaviour
{
    public static string weaponHolderName = "WeaponHolder";

    [Required]
        public Grafic grafic;

    public Vector2 targetPos;
    public Vector2 targetDir 
    { 
        get 
        {
            return new Vector3(
                targetPos.x - transform.position.x,
                targetPos.y - transform.position.y,
                0).normalized; 
        } 
    }
    public Vector3 moveVector;
    [HideInInspector] public Vector3 prevMoveVector;

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
                        Weapon weapon = transform.Find(weaponHolderName).GetChild(index).GetComponent<Weapon>();

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
                    if (index == 0) SelectWeapon(defaultWeapon);
                    else SelectWeapon(weapons[index - 1]);
                }

#endif

    #endregion

        [VerticalGroup("Weapon/Inventory")]
        [HorizontalGroup("Weapon/Inventory/Horizontal")]
        #region Horizontal Inventory

                [ReadOnly]
                public List<Weapon> weapons = new List<Weapon>();

            
            [HorizontalGroup("Weapon/Inventory/Horizontal", width: 30)]
            [VerticalGroup("Weapon/Inventory/Horizontal/Manage")][ShowInInspector]
            #region Vertical Manage        
            
                    [HideLabel]
                    public int inventorySize
                    {
                        get { return _inventorySize; }
                        set
                        {
                            for (int i = weapons.Count - 1; i >= _inventorySize; i--)
                            {
                                DropWeapon(weapons[i]);
                            }
                            _inventorySize = value;
                        }
                    } int _inventorySize = 1;

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

            #endregion

            #if UNITY_EDITOR

            [HorizontalGroup("Weapon/Inventory/Manage")]
            #region Horizontal Manage

                    [Button(name: "Add")]
                    void AddWeaponInInspector(Weapon weapon)
                    {
                        AddWeapon(weapon);
                    }

                [HorizontalGroup("Weapon/Inventory/Manage")]
                    [Button(name: "Destroy")]
                    void DestroyWeaponInInspector(int index)
                    {
                        if (index == -1)
                        {
                            if (EditorApplication.isPlaying) Destroy(defaultWeapon.gameObject);
                            else DestroyImmediate(defaultWeapon.gameObject);

                            defaultWeapon = null;
                        }
                        else weapons[index].Destroy();
                    }

    #endregion

#endif

    #endregion

    #endregion

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
    /// 무기 추가
    /// </summary>
    /// <param name="weapon">무기 상태에 대해 안전하지 않음</param>
    public void AddWeapon(Weapon weapon)
    {
        if (weapons.Count > inventorySize)
        {
            Debug.LogWarning("인벤토리가 꽉 참");
            if (weapon.state == WeaponState.PREFAB) weapon.Destroy();
            return;
        } //LogWarning: 인벤토리가 꽉 참 >> return

        if (weapons.Contains(weapon))
        {
            Debug.LogWarning("이미 인벤토리에 있음");
            return;
        } //LogWarning: 이미 인벤토리에 있음 >> return

        weapon.con = this;
        weapon.state = WeaponState.INVENTORY;

        weapons.Add(weapon);
    }

    /// <summary>
    /// HOLD -> INVENTORY -> REMOVED -> ITEM >> 무기 드랍 //
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
