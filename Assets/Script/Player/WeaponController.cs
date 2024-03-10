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
    #region ����
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

    #region ����
    public void AddWeapon(Weapon weapon, string data, string customData)
    {
        if (transform.childCount > 11)
        {
            Debug.LogError("�κ��丮 �ʰ�");
            return;
        } //LogError: �κ��丮 �ʰ�

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
            Debug.LogError("������ �� ���� �����Դϴ�");
            return;
        } //LogError: ������ �� ���� �����Դϴ�

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

        //����(�κ��丮����)
        weapons[index].transform.parent = null;

        if (transform.childCount == 1) lastWeaponIndex = 0;
        ResetWeaponIndex();
    }

    #endregion

    #region ��ü
    void WheelSelect()
    {
        if (Player.pCon.mouseWheelClickDown) SelectWeapon(0);
        
        if (Player.pCon.mouseWheelScroll == 0) return;

        if (curWeapon.index == 0) //�Ǽ��� ��
        {
            if(transform.childCount == 1) //���Ⱑ ���� ��
            {
                if (curWeapon.index != 0) SelectWeapon(0);
                return;
            }
            if (lastWeaponIndex != 0) //���������� ����� ���Ⱑ ���� ��
            {
                SelectWeapon(lastWeaponIndex); //���������� ����� ����
                return;
            }
        }
        if (Player.pCon.mouseWheelScroll > 0) //����
        {
            if (curWeapon.index != 0 && curWeapon.index == transform.childCount - 1) //������ ������ ������ ��
            {
                SelectWeapon(1); //ù ��° ����
            }
            else SelectWeapon(curWeapon.index + 1); // +1

            return;
        }
        else //����
        {
            if (curWeapon.index <= 1) //�Ǽ��̰ų� ù ��° ������ ������ ��
            {
                SelectWeapon(transform.childCount - 1);
            }
            else SelectWeapon(curWeapon.index - 1); // -1

            return;
        }
    }
    [DisableInEditorMode]
    [Button(name: "�ʱ�ȭ")]
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
            Debug.LogWarning("lastWeaponIndex �ε��� �ʰ�( " + lastWeaponIndex + "/" + (transform.childCount - 1) + " )");
            lastWeaponIndex = 0;
        } //LogWarning: lastWeaponIndex �ε��� �ʰ�

        inventoryUI.ResetInventoryUI();
    }
    [DisableInEditorMode]
    [Button(ButtonStyle.Box)]
    void SelectWeapon(int index)
    {
        if (index < 0 || transform.childCount - 1 < index)
        {
            Debug.LogWarning("index�� ������ �ʰ���: (" + index + "/" + (transform.childCount - 1) + " )");
            index = 0;
        } //LogWarning: "index�� ������ �ʰ���"

        for (int i = 0; i < transform.childCount; i++) //���� ��� ��Ȱ��ȭ
        {
            if (weapons[i].isUsing)
            {
                weapons[i].SetUse(false);
            }
        }
        if (weapons[index] == null)
        {
            Debug.LogWarning("ȣ���� �ε����� ���Ⱑ ����");
            index = 0;
        } //LogWarning: "ȣ���� �ε����� ���Ⱑ ����"

        curWeapon = weapons[index];

        curWeapon.SetUse(true); //������ ���� Ȱ��ȭ



        if (index != 0) lastWeaponIndex = index;
        inventoryUI.ChangeWeaponUI(index);
    }
    #endregion
}