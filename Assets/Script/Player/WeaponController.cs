using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public UI_Inventory inventoryUI;

    [SerializeField] public Weapon[] weapons = new Weapon[11];
    public Weapon curWeapon { get; private set; }
    int lastWeaponIndex = 0;
    

    void Awake()
    {
        Player.wCon = this;

        curWeapon = weapons[0];
        LoadWeapons("WoodenSword,3/\n");

        ResetWeaponIndex();
    }
    void Update()
    {
        WheelSelect();
        if (Input.GetKeyDown(KeyCode.P)) LoadWeapon("WoodenSword,3/");
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
                Player.wCon.weapons[i].LoadData() + "\n";
        }
    }
    public void LoadWeapons(string weaponDatas)
    {
        string[] weapons = weaponDatas.Split('\n');
        foreach (string weapon in weapons)
        {
            LoadWeapon(weapon);
        }
    }
    public Weapon LoadWeapon(string weaponData)
    {
        if (weaponData == "") return null;

        string[] split = weaponData.Split('/');

        Debug.Log("LoadWeapon: {" + split[0].Substring(0, split[0].IndexOf(','))+"split[0]: [" + split[0]+"], " + "split[1]: [" + split[1] +"]}");
        GameObject weaponObj = 
            Instantiate((GameObject) Resources.Load("WeaponObjPrefab/" + split[0].Substring(0, split[0].IndexOf(','))), transform);
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
            DropItem(weapon);
            return;
        } //LogError: 인벤토리 초과

        weapon.index = transform.childCount - 1;
        weapon.SetData(data.Split(','), customData.Split(','));
        weapons[weapon.index] = weapon;

        //UI
        inventoryUI.AddToInventory(weapon);
    }
    public void RemoveWeapon(int index)
    {
        if(index == 0)
        {
            Debug.LogError("제거할 수 없는 무기입니다");
            return;
        } //LogError: 제거할 수 없는 무기입니다

        Weapon weapon = weapons[index];
        weapon.OnWeaponDestroy();

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

        //제거

        Destroy(weapon.gameObject);

        ResetWeaponIndex();
        if (transform.childCount == 1) lastWeaponIndex = 0;
    }
    #endregion

    #region 교체
    void WheelSelect()
    {
        if (Player.pCon.mouseWheelClickDown) SelectWeapon(0);
        
        if (Player.pCon.mouseWheelScroll == 0) return;

        if (curWeapon.index == 0) //맨손일 때
        {
            if (transform.childCount != 1 && lastWeaponIndex != 0) // 맨손을 제외한 무기가 존재하고 마지막으로 들었던 무기가 있을 때
            {
                SelectWeapon(lastWeaponIndex); //맨손을 제외하고 마지막으로 들었던 무기
                return;
            }
        }
        if (Player.pCon.mouseWheelScroll > 0) //증가
        {
            if (curWeapon.index != 0 && curWeapon.index == transform.childCount - 1) //맨손이 아닌 마지막 순서의 무기일 때
            {
                SelectWeapon(1); //첫 번째 무기
            }
            else SelectWeapon(curWeapon.index + 1); // +1

            return;
        }
        else //감소
        {
            if (curWeapon.index == 1) //첫 번째 순서의 무기일 때
            {
                SelectWeapon(transform.childCount - 1);
            }
            else SelectWeapon(curWeapon.index - 1); // -1

            return;
        }
    }
    void SelectWeapon(int index)
    {
        if (index < 0 || transform.childCount - 1 < index)
        {
            Debug.LogError("index가 범위를 초과함: (" + index + "/" + (transform.childCount - 1) + " )");
            SelectWeapon(0);
            return;
        } //LogError: "index가 범위를 초과함"

        for (int i = 0; i < transform.childCount; i++) //무기 모두 비활성화
        {
            if (weapons[i].isUsing)
            {
                weapons[i].Use(false);
            }
        }
        if (weapons[index] == null)
        {
            Debug.LogError("호출한 인덱스에 무기가 없음");
            SelectWeapon(0);
            return;
        } //LogError: "호출한 인덱스에 무기가 없음"

        curWeapon = weapons[index];

        curWeapon.Use(true); //선택한 무기 활성화

        if (index != 0) lastWeaponIndex = index;
        inventoryUI.ChangeWeaponUI(index);
    }
    void ResetWeaponIndex()
    {
        for (int i = 1; i < 11; i++)
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
            Debug.LogError("lastWeaponIndex 인덱스 초과");
            lastWeaponIndex = 0;
        } //LogError: lastWeaponIndex 인덱스 초과

        inventoryUI.ResetInventoryUI();
    }
    #endregion
    
    #region 획득
    void DropItem(Weapon weapon)
    {
        GameObject item = ItemManager.SpawnItem(weapon, transform.position, weapon.LoadData());
        RemoveWeapon(weapon.index);
    }
    /// <summary>
    /// 무기 인덱스 초기화: Weapon.index, pCon.wCon.curWeaponIndex
    /// </summary>
    #endregion
}