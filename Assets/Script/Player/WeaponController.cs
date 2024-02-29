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

        if (Player.pCon.mouseWheelScroll > 0) //증가
        {
            if (curWeaponIndex == weaponHolder.childCount - 1) //마지막 순서의 무기일 때
            {
                if (weaponHolder.childCount == 1) SelectWeapon(0); //무기가 없을 때
                else SelectWeapon(1); //무기가 더 있을 때
            }
            else SelectWeapon(curWeaponIndex + 1);
        }
        else //감소
        {
            if (curWeaponIndex == 1) //첫 번째 순서의 무기일 때
            {
                if (weaponHolder.childCount == 1) SelectWeapon(0); //무기가 없을 때
                else SelectWeapon(weaponHolder.childCount - 1); //무기가 더 있을 때
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
        GameObject weaponObj = Instantiate((GameObject) Resources.Load("Assets/Resources/WeaponObjPrefab/"), weaponHolder); //무기 오브젝트 생성

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
            Debug.LogError("index가 범위를 초과함: (" + index + "/" + (weaponHolder.childCount - 1) + " )");
            return;
        } //LogError: "index가 범위를 초과함"

        for (int i = 0; i < weaponHolder.childCount; i++) //무기 오브젝트 모두 비활성화
        {
            if (weaponHolder.GetChild(i).GetComponent<Weapon>().isUsing)
            {
                weaponHolder.GetChild(i).GetComponent<Weapon>().Use(false);
            }
        }
        if (index > weaponHolder.childCount - 1) 
        {
            Debug.LogError("인덱스에 무기가 없음");
            SelectWeapon(0);
            return;
        }//LogError: "인덱스에 무기가 없음"

        curWeaponIndex = index;
        curWeapon = weaponHolder.GetChild(index).GetComponent<Weapon>();

        curWeapon.Use(true); //선택한 무기 활성화

        if (index != 0) lastWeaponIndex = index;
        inventoryUI.ChangeWeaponUI(index);
    }
    void DropItem(Weapon weapon)//수정 필요---------------------------
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
        //다음 무기 선택
        if (index + 1 == weaponHolder.childCount) //마지막 순서의 무기일 때
        {
            if (weaponHolder.childCount == 2) //무기가 하나일 때
            {
                SelectWeapon(0);
            }
            else SelectWeapon(index - 1); //무기가 더 있을 때
        }
        else SelectWeapon(index + 1);

        //제거

        weaponHolder.GetChild(index).GetComponent<Weapon>().OnWeaponDestroy();

        Player.wCon.DestroyWeapon(index);
        Destroy(gameObject);

        ResetWeaponIndex();
        SelectWeapon(curWeaponIndex);
        if (weaponHolder.childCount == 1) lastWeaponIndex = 0;
    }
}
