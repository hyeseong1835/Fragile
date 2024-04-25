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
    //��ǥ
    public Vector2 pos;
    //�⺻ ����(������)
    public WeaponData defaultWeaponData;
    //���� ����(�ε���)
    public int curWeaponIndex;
    //�κ��丮(����(������))
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
    
    //�Է�
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
                    // ����(-1)
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

                        //���� ���Ⱑ ������ �ڵ� ����
                        if (curWeapon == null)
                        {
                            curWeapon = weapon;
                            SelectWeapon(weapon);
                        }

                        //�κ��丮���� �ڵ� ����
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
                        //�⺻ ���� ����
                        if (defaultWeapon != null)
                        {
                            if (EditorApplication.isPlaying) Destroy(defaultWeapon.gameObject);
                            else DestroyImmediate(defaultWeapon.gameObject);

                            defaultWeapon = null;
                        }

                        //�κ��丮 ���� ����
                        for (int i = weapons.Count - 1; i >= 0; i--)
                        {
                            if (weapons[i] == null) continue;
                            
                            if (EditorApplication.isPlaying) Destroy(weapons[i].gameObject);
                            else DestroyImmediate(weapons[i].gameObject);
                        }
                        weapons.Clear();
                        
                        defaultWeapon = null;
                        curWeapon = null;

                        //Ȯ���� ���� (��� �ı� ������ �ε��� ����)
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
                        if (weapons.Count > 0) Debug.LogWarning(weapons.Count + "���� ���� ����Ʈ���� ���ŵ��� ����.");
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
    /// ���� �߰�
    /// </summary>
    /// <param name="weapon">���� ���¿� ���� �������� ����</param>
    public void AddWeapon(Weapon weapon)
    {
        if (weapons.Count > inventorySize)
        {
            Debug.LogWarning("�κ��丮�� �� ��");
            if (weapon.state == WeaponState.PREFAB) weapon.Destroy();
            return;
        } //LogWarning: �κ��丮�� �� �� >> return

        if (weapons.Contains(weapon))
        {
            Debug.LogWarning("�̹� �κ��丮�� ����");
            return;
        } //LogWarning: �̹� �κ��丮�� ���� >> return

        weapon.con = this;
        weapon.state = WeaponState.INVENTORY;

        weapons.Add(weapon);
    }

    /// <summary>
    /// HOLD -> INVENTORY -> REMOVED -> ITEM >> ���� ��� //
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

        //������ ȿ��----------------------------------------------------
    }

    /// <summary>
    /// Hold -> Inventory >> �κ��丮���� ���� ���� >> weapon.OnWeaponRemoved() >> �θ� ����, weapon.state = REMOVED >> �� ���� ���� //
    /// </summary>
    /// <param name="weapon"></param>
    public void RemoveWeapon(Weapon weapon)
    {
        Debug.Log("Remove: "+weapon);
        //�⺻ ���⸦ ������ ��
        if (weapon == defaultWeapon)
        {
            Debug.LogError("�⺻ ����� ������ �� ����");
            return;
        } //LogError: �⺻ ����� ������ �� ���� >> return

        if (weapons.Contains(weapon) == false)
        {
            Debug.LogError("{" + weapon.name + "}��(��) �κ��丮���� ã�� �� ���� �������� ����.");
            return;
        } //LogError: {weapon.name}��(��) �κ��丮���� ã�� �� ���� �������� ����. >> return

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

            //���� ����
            if (index == weapons.Count) //������ ������ ������ ��
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
        } //LogError: {weapon.name}��(��) �κ��丮���� ã�� �� ���� �������� ����. >> return

        if (curWeapon != null && curWeapon.state == WeaponState.HOLD) curWeapon.SetUse(false); //���� ���� ��Ȱ��ȭ

        curWeapon = weapon;
        if (curWeapon.state == WeaponState.INVENTORY) weapon.SetUse(true); //������ ���� Ȱ��ȭ
    }
}
