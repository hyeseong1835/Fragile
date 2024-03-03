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
    #region ����
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

    #region ����
    public void AddWeapon(Weapon weapon, string data, string customData)
    {
        if (transform.childCount > 11)
        {
            Debug.LogError("�κ��丮 �ʰ�");
            DropItem(weapon);
            return;
        } //LogError: �κ��丮 �ʰ�

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
            Debug.LogError("������ �� ���� �����Դϴ�");
            return;
        } //LogError: ������ �� ���� �����Դϴ�

        Weapon weapon = weapons[index];
        weapon.OnWeaponDestroy();

        //���� ����
        if (index + 1 == transform.childCount) //������ ������ ������ ��
        {
            if (transform.childCount == 2) //���Ⱑ �ϳ��� ��
            {
                SelectWeapon(0);
            }
            else SelectWeapon(index - 1); //���Ⱑ �� ���� ��
        }
        else SelectWeapon(index + 1);

        //����

        Destroy(weapon.gameObject);

        ResetWeaponIndex();
        if (transform.childCount == 1) lastWeaponIndex = 0;
    }
    #endregion

    #region ��ü
    void WheelSelect()
    {
        if (Player.pCon.mouseWheelClickDown) SelectWeapon(0);
        
        if (Player.pCon.mouseWheelScroll == 0) return;

        if (curWeapon.index == 0) //�Ǽ��� ��
        {
            if (transform.childCount != 1 && lastWeaponIndex != 0) // �Ǽ��� ������ ���Ⱑ �����ϰ� ���������� ����� ���Ⱑ ���� ��
            {
                SelectWeapon(lastWeaponIndex); //�Ǽ��� �����ϰ� ���������� ����� ����
                return;
            }
        }
        if (Player.pCon.mouseWheelScroll > 0) //����
        {
            if (curWeapon.index != 0 && curWeapon.index == transform.childCount - 1) //�Ǽ��� �ƴ� ������ ������ ������ ��
            {
                SelectWeapon(1); //ù ��° ����
            }
            else SelectWeapon(curWeapon.index + 1); // +1

            return;
        }
        else //����
        {
            if (curWeapon.index == 1) //ù ��° ������ ������ ��
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
            Debug.LogError("index�� ������ �ʰ���: (" + index + "/" + (transform.childCount - 1) + " )");
            SelectWeapon(0);
            return;
        } //LogError: "index�� ������ �ʰ���"

        for (int i = 0; i < transform.childCount; i++) //���� ��� ��Ȱ��ȭ
        {
            if (weapons[i].isUsing)
            {
                weapons[i].Use(false);
            }
        }
        if (weapons[index] == null)
        {
            Debug.LogError("ȣ���� �ε����� ���Ⱑ ����");
            SelectWeapon(0);
            return;
        } //LogError: "ȣ���� �ε����� ���Ⱑ ����"

        curWeapon = weapons[index];

        curWeapon.Use(true); //������ ���� Ȱ��ȭ

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
            Debug.LogError("lastWeaponIndex �ε��� �ʰ�");
            lastWeaponIndex = 0;
        } //LogError: lastWeaponIndex �ε��� �ʰ�

        inventoryUI.ResetInventoryUI();
    }
    #endregion
    
    #region ȹ��
    void DropItem(Weapon weapon)
    {
        GameObject item = ItemManager.SpawnItem(weapon, transform.position, weapon.LoadData());
        RemoveWeapon(weapon.index);
    }
    /// <summary>
    /// ���� �ε��� �ʱ�ȭ: Weapon.index, pCon.wCon.curWeaponIndex
    /// </summary>
    #endregion
}