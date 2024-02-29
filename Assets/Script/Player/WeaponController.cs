using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class WeaponController : MonoBehaviour
{
    public Transform weaponHolder;

    public Weapon curWeapon;

    public UI_Inventory inventoryUI;
    public int lastWeaponIndex = 0;
    public int curWeaponIndex = 0;

    void Awake()
    {

    }
    void Start()
    {
        
    }
    void Update()
    {
        WheelSelect();
    }
    void WheelSelect()
    {
        if (Player.pCon.mouseWheelClickDown) SelectWeapon(0);
        
        if (Player.pCon.mouseWheelScroll == 0) return;
        
        if(curWeaponIndex == 0)
        {
            if(weaponHolder.childCount > 1) SelectWeapon(lastWeaponIndex);
            if (weaponHolder.childCount == 1) return;
        }

        if (Player.pCon.mouseWheelScroll > 0) //����
        {
            if (curWeaponIndex == weaponHolder.childCount - 1) //������ ������ ������ ��
            {
                if (weaponHolder.childCount == 1) SelectWeapon(0); //���Ⱑ ���� ��
                else SelectWeapon(1); //���Ⱑ �� ���� ��
            }
            else SelectWeapon(curWeaponIndex + 1);
        }
        else //����
        {
            if (curWeaponIndex == 1) //ù ��° ������ ������ ��
            {
                if (weaponHolder.childCount == 1) SelectWeapon(0); //���Ⱑ ���� ��
                else SelectWeapon(weaponHolder.childCount - 1); //���Ⱑ �� ���� ��
            }
            else SelectWeapon(curWeaponIndex - 1);
        }
    }
    public void TakeItem(Weapon weapon, string data)
    {
        if (weaponHolder.childCount > 11)
        {
            DropItem(weapon);
            return;
        }
        GameObject weaponObj = Instantiate((GameObject) Resources.Load("Assets/Resources/WeaponObjPrefab/"), weaponHolder); //���� ������Ʈ ����

        weapon.index = Player.wCon.weaponHolder.childCount - 1;
        weapon.SetData(data.Split(','));

        if (weaponHolder.childCount > 1) lastWeaponIndex = 1;
        inventoryUI.ResetInventoryUI();
    }
    public void TakeItem()
    {

    }
    public void SelectWeapon(int index)
    {
        if (index < 0 || weaponHolder.childCount < index + 1) 
        {
            Debug.LogError("index�� ������ �ʰ���: (" + index + "/" + (weaponHolder.childCount - 1) + " )");
            return;
        } //LogError: "index�� ������ �ʰ���"

        for (int i = 0; i < weaponHolder.childCount; i++) //���� ������Ʈ ��� ��Ȱ��ȭ
        {
            if (weaponHolder.GetChild(i).GetComponent<Weapon>().isUsing)
            {
                weaponHolder.GetChild(i).GetComponent<Weapon>().Use(false);
            }
        }
        if (index > weaponHolder.childCount - 1) 
        {
            Debug.LogError("�ε����� ���Ⱑ ����");
            SelectWeapon(0);
            return;
        }//LogError: "�ε����� ���Ⱑ ����"

        curWeaponIndex = index;
        curWeapon = weaponHolder.GetChild(index).GetComponent<Weapon>();

        curWeapon.Use(true); //������ ���� Ȱ��ȭ

        if (index != 0) lastWeaponIndex = index;
        inventoryUI.ChangeWeaponUI(index);
    }
    void DropItem(Weapon weapon)//���� �ʿ�---------------------------
    {
        GameObject item = ItemManager.SpawnItem(weapon, weapon.LoadData());
        weapon.DestroyWeapon();
    }
    void ResetWeaponIndex()
    {
        for (int i = 0; i < weaponHolder.childCount; i++)
        {
            weaponHolder.GetChild(i).GetComponent<Weapon>().index = i;
        }
        curWeaponIndex = curWeapon.index;
        if(lastWeaponIndex + 1 > weaponHolder.childCount) lastWeaponIndex = weaponHolder.childCount - 1;
    }
    public void DestroyWeapon(int index)
    {
        //���� ���� ����
        if (index + 1 == weaponHolder.childCount) //������ ������ ������ ��
        {
            if (weaponHolder.childCount == 2) //���Ⱑ �ϳ��� ��
            {
                SelectWeapon(0);
            }
            else SelectWeapon(index - 1); //���Ⱑ �� ���� ��
        }
        else SelectWeapon(index + 1);

        //����

        weaponHolder.GetChild(index).GetComponent<Weapon>().OnWeaponDestroy();

        Player.wCon.DestroyWeapon(index);
        Destroy(gameObject);

        ResetWeaponIndex();
        SelectWeapon(curWeaponIndex);
        if (weaponHolder.childCount == 1) lastWeaponIndex = 0;
    }
}
