using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public UI_Inventory inventoryUI;
    [Title("Weapon")]
    public Weapon curWeapon;
    public Weapon[] weapons = new Weapon[11];
    int lastWeaponIndex = 0;
    
    void Awake()
    {
        Player.wCon = this;
    }
    void Update()
    {
        WheelSelect();
        if (Input.GetKeyDown(KeyCode.P)) LoadWeapon("WoodenSword,n/");
    }
    #region 저장
    public void SaveWeapons()
    {
        string weaponSaveData = "";
        for (int i = 1; i < Player.wCon.transform.childCount; i++)
        {
            weaponSaveData +=
                Player.wCon.weapons[i].gameObject.name + "," +
                Player.wCon.weapons[i].durability.ToString() + "/" +
                Player.wCon.weapons[i].LoadCustomData() + "\n";
        }
    }
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    public void LoadWeapons(string data)
    {
        string[] weaponDatas = data.Split('\n');
        foreach (string weaponData in weaponDatas)
        {
            if (LoadWeapon(weaponData) == null) return;
        }
    }
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    public Weapon LoadWeapon(string weaponData)
    {
        if (weaponData == "") return null;

        string[] split = weaponData.Split('/');

        Debug.Log("LoadWeapon: {\"" + "WeaponObjPrefab/" + split[0].Substring(0, split[0].IndexOf(',')) + "\": ( split[0]: [ " + split[0] + " ], " + "split[1]: [ " + split[1] + " ])}");
        GameObject weaponObj = Instantiate((GameObject) Resources.Load("WeaponObjPrefab/" + split[0].Substring(0, split[0].IndexOf(','))), transform);
        Weapon weapon = weaponObj.GetComponent<Weapon>();
        AddWeapon(weapon, split[0], split[1]);
        return weapon;
    }
    #endregion

    #region 관리
    public void AddWeapon(Weapon weapon, string data, string customData)
    {
        if (transform.childCount > 11)
        {
            Debug.LogError("인벤토리 초과");
            return;
        } //LogError: 인벤토리 초과

        weapon.index = transform.childCount - 1;
        weapon.SetData(data.Split(','), customData.Split(','));
        weapons[weapon.index] = weapon;

        //UI
        inventoryUI.AddToInventory(weapon);
    }
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    public void RemoveWeapon(int index)
    {
        if(index == 0)
        {
            Debug.LogError("제거할 수 없는 무기입니다");
            return;
        } //LogError: 제거할 수 없는 무기입니다

        //무기 선택
        if (index + 1 == transform.childCount) //마지막 순서의 무기일 때
        {
            if (transform.childCount == 2) //무기가 하나일 때
            {
                SelectWeapon(0);
            }
            else SelectWeapon(index - 1); //무기가 더 있을 때
        }
        else SelectWeapon(index + 1);

        //제거(인벤토리에서)
        weapons[index].transform.parent = null;

        if (transform.childCount == 1) lastWeaponIndex = 0;
        ResetWeaponIndex();
    }

    #endregion

    #region 교체
    void WheelSelect()
    {
        if (Player.pCon.mouseWheelClickDown) SelectWeapon(0);
        
        if (Player.pCon.mouseWheelScroll == 0) return;

        if (curWeapon.index == 0) //맨손일 때
        {
            if(transform.childCount == 1) //무기가 없을 때
            {
                if (curWeapon.index != 0) SelectWeapon(0);
                return;
            }
            if (lastWeaponIndex != 0) //마지막으로 들었던 무기가 있을 때
            {
                SelectWeapon(lastWeaponIndex); //마지막으로 들었던 무기
                return;
            }
        }
        if (Player.pCon.mouseWheelScroll > 0) //증가
        {
            if (curWeapon.index != 0 && curWeapon.index == transform.childCount - 1) //마지막 순서의 무기일 때
            {
                SelectWeapon(1); //첫 번째 무기
            }
            else SelectWeapon(curWeapon.index + 1); // +1

            return;
        }
        else //감소
        {
            if (curWeapon.index <= 1) //맨손이거나 첫 번째 순서의 무기일 때
            {
                SelectWeapon(transform.childCount - 1);
            }
            else SelectWeapon(curWeapon.index - 1); // -1

            return;
        }
    }
    [DisableInEditorMode]
    [Button(name: "초기화")]
    void ResetWeaponIndex()
    {
        weapons = new Weapon[11];
        for (int i = 0; i < 11; i++)
        {
            if (i < transform.childCount)
            {
                weapons[i] = transform.GetChild(i).GetComponent<Weapon>();
                weapons[i].index = i;
            }
            else weapons[i] = null;
        }
        if (lastWeaponIndex > transform.childCount - 1)
        {
            Debug.LogWarning("lastWeaponIndex 인덱스 초과( " + lastWeaponIndex + "/" + (transform.childCount - 1) + " )");
            lastWeaponIndex = 0;
        } //LogWarning: lastWeaponIndex 인덱스 초과

        inventoryUI.ResetInventoryUI();
    }
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    void SelectWeapon(int index)
    {
        if (index < 0 || transform.childCount - 1 < index)
        {
            Debug.LogWarning("index가 범위를 초과함: (" + index + "/" + (transform.childCount - 1) + " )");
            index = 0;
        } //LogWarning: "index가 범위를 초과함"

        for (int i = 0; i < transform.childCount; i++) //무기 모두 비활성화
        {
            if (weapons[i].isUsing)
            {
                weapons[i].SetUse(false);
            }
        }
        if (weapons[index] == null)
        {
            Debug.LogWarning("호출한 인덱스에 무기가 없음");
            index = 0;
        } //LogWarning: "호출한 인덱스에 무기가 없음"

        curWeapon = weapons[index];

        curWeapon.SetUse(true); //선택한 무기 활성화



        if (index != 0) lastWeaponIndex = index;
        inventoryUI.ChangeWeaponUI(index);
    }
    #endregion
}