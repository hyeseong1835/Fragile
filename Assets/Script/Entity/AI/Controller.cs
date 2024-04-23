using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class Controller : MonoBehaviour
{
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
    #region Weapon

        [ShowInInspector]
            [Required]
            public Weapon defaultWeapon
            {
                get { return _defaultWeapon; }
                set
                {
                #if UNITY_EDITOR

                    if (value == null)
                    {
                        _defaultWeapon = null;
                        return;
                    }
                    if (curWeapon == null)
                    {
                        curWeapon = value;
                        value.SetUse(true); //선택한 무기 활성화
                    }
                    if (weapons.Contains(value))
                    {
                        weapons.Remove(value);
                    }

                #endif
                    _defaultWeapon = value;
                }
            } Weapon _defaultWeapon;

        [BoxGroup("Weapon")]
            [ReadOnly][Required][PropertyOrder(2)]
            public Weapon curWeapon;

        [HorizontalGroup("Weapon/Horizontal")][PropertyOrder(3)]
        #region Horizontal

            [ReadOnly]
                public List<Weapon> weapons;
    
            [HorizontalGroup("Weapon/Horizontal", width: 30)][ShowInInspector][PropertyOrder(3)]
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

        #endregion

    #endregion

    public void AddWeapon(Weapon weapon)
    {
        if (weapons.Count >= inventorySize)
        {
            Debug.LogWarning("인벤토리가 꽉 참");
            weapon.Drop();
            return;
        }
        weapon.con = this;
        
        weapons.Add(weapon);
    }
    [Button(ButtonStyle.Box)]
    public void RemoveWeapon(Weapon weapon)
    {
        if(weapons.Contains(weapon) == false)
        {
            Debug.LogError("{" + weapon.name + "}을(를) 인벤토리에서 찾을 수 없어 제거하지 못함.");
            return;
        }
        int index = weapons.IndexOf(weapon);
        weapons.RemoveAt(index);

        //무기 선택
        if (index == weapons.Count - 1) //마지막 순서의 무기일 때
        {
            if (weapons.Count == 0) SelectWeapon(defaultWeapon);
            else SelectWeapon(weapons[0]);
        }
        else SelectWeapon(weapons[index]);

        weapon.transform.parent = null;

    }

#if UNITY_EDITOR
    
    [Button(ButtonStyle.Box)]
    void SelectWeaponInInspector(int index)
    {
        if(index == -1) SelectWeapon(defaultWeapon);
        else SelectWeapon(weapons[index]);
    }

#endif
    protected void SelectWeapon(Weapon weapon)
    {
        if (weapons.Contains(weapon) == false)
        {
            Debug.LogError("{" + weapon.name + "}을(를) 인벤토리에서 찾을 수 없어 선택하지 못함.");
            return;
        }
        curWeapon.SetUse(false); //현재 무기 비활성화

        curWeapon = weapon;
        weapon.SetUse(true); //선택한 무기 활성화
    }
    [Button(ButtonStyle.Box)]
    public void DropWeapon(Weapon weapon)
    {
        Item item = null;

        switch (weapon.state)
        {
            case WeaponState.PREFAB:
                Debug.LogError("프리팹 상태에서는 드랍할 수 없습니다.");
                break;

            case WeaponState.ITEM:
                item = weapon.transform.parent.GetComponent<Item>();
                break;

            case WeaponState.HOLD:
                weapon.Remove();
                weapons.Remove(weapon);
                item = ItemManager.WrapWeaponInItem(weapon);
                weapon.state = WeaponState.ITEM;
                break;

            case WeaponState.INVENTORY:
                weapon.Remove();
                weapons.Remove(weapon);
                item = ItemManager.WrapWeaponInItem(weapon);
                weapon.state = WeaponState.ITEM;
                break;

            case WeaponState.REMOVED:
                Debug.LogError("제거된 무기는 드랍할 수 없습니다?");
                break;
        }

        item.transform.position = transform.position;
        
        //던지는 효과----------------------------------------------------
    }
}
