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
    
    //�Է�
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
                        value.SetUse(true); //������ ���� Ȱ��ȭ
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
            Debug.LogWarning("�κ��丮�� �� ��");
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
            Debug.LogError("{" + weapon.name + "}��(��) �κ��丮���� ã�� �� ���� �������� ����.");
            return;
        }
        int index = weapons.IndexOf(weapon);
        weapons.RemoveAt(index);

        //���� ����
        if (index == weapons.Count - 1) //������ ������ ������ ��
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
            Debug.LogError("{" + weapon.name + "}��(��) �κ��丮���� ã�� �� ���� �������� ����.");
            return;
        }
        curWeapon.SetUse(false); //���� ���� ��Ȱ��ȭ

        curWeapon = weapon;
        weapon.SetUse(true); //������ ���� Ȱ��ȭ
    }
    [Button(ButtonStyle.Box)]
    public void DropWeapon(Weapon weapon)
    {
        Item item = null;

        switch (weapon.state)
        {
            case WeaponState.PREFAB:
                Debug.LogError("������ ���¿����� ����� �� �����ϴ�.");
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
                Debug.LogError("���ŵ� ����� ����� �� �����ϴ�?");
                break;
        }

        item.transform.position = transform.position;
        
        //������ ȿ��----------------------------------------------------
    }
}
