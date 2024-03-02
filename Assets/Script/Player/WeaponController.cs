using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class WeaponController : MonoBehaviour
{
    [ShowInInspector] Transform weaponHolder { get; set; }
    [ShowInInspector] public UI_Inventory inventoryUI { get; set; }
    public int weaponCount { get { return weaponHolder.childCount; } }
    public Weapon curWeapon { get; private set; }
    [ShowInInspector] public Weapon[] weapons { get; private set; } = new Weapon[11];
    
    public int curWeaponIndex = 0;
    int lastWeaponIndex = 0;

    void Update()
    {
        WheelSelect();
    }
    void WheelSelect()
    {
        if (Player.pCon.mouseWheelClickDown) SelectWeapon(0);
        
        if (Player.pCon.mouseWheelScroll == 0) return;

        if (curWeaponIndex == 0)
        {
            if (weaponCount == 1) SelectWeapon(0); //�Ǽ�
            else if (lastWeaponIndex != 0) SelectWeapon(lastWeaponIndex); //�Ǽ��� �����ϰ� ���������� ����� ����
        }
        if (Player.pCon.mouseWheelScroll > 0) //����
        {
            if (curWeaponIndex == weaponCount - 1) //������ ������ ������ ��
            {
                if (weaponCount == 1) SelectWeapon(0); //���Ⱑ ���� ��
                else SelectWeapon(1); //���Ⱑ �� ���� ��
            }
            else SelectWeapon(curWeaponIndex + 1); //���� ����
        }
        else //����
        {
            if (curWeaponIndex == 1) //ù ��° ������ ������ ��
            {
                if (weaponCount == 1) SelectWeapon(0); //���Ⱑ ���� ��
                else SelectWeapon(weaponCount - 1); //���Ⱑ �� ���� ��
            }
            else SelectWeapon(curWeaponIndex - 1); //���� ����
        }
    }
    public void TakeItem(Weapon weapon, string data)
    {
        if (weaponCount > 11)
        {
            Debug.LogError("�κ��丮 �ʰ�");
            DropItem(weapon);
            return;
        } //�κ��丮 �ʰ�

        //GameObject weaponObj = Instantiate((GameObject) Resources.Load("Assets/Resources/WeaponObjPrefab/"), weaponHolder); //���� ������Ʈ ����
        weapon.index = weaponCount - 1;
        weapon.SetData(data.Split(','));

        if (weaponCount > 1) lastWeaponIndex = 1;
        inventoryUI.ResetInventoryUI();
    }
    void SelectWeapon(int index)
    {
        if (index < 0 || weaponCount < index + 1)
        {
            Debug.LogError("index�� ������ �ʰ���: (" + index + "/" + (weaponHolder.childCount - 1) + " )");
            SelectWeapon(0);
        } //LogError: "index�� ������ �ʰ���"

        for (int i = 0; i < weaponCount; i++) //���� ��� ��Ȱ��ȭ
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
        }//LogError: "ȣ���� �ε����� ���Ⱑ ����"

        curWeaponIndex = index;
        curWeapon = weapons[index];

        curWeapon.Use(true); //������ ���� Ȱ��ȭ

        if (index != 0) lastWeaponIndex = index;
        inventoryUI.ChangeWeaponUI(index);
    }
    void DropItem(Weapon weapon)
    {
        GameObject item = ItemManager.SpawnItem(weapon, transform.position, weapon.LoadData());
        RemoveWeapon(weapon.index);
    }
    /// <summary>
    /// ���� �ε��� �ʱ�ȭ: Weapon.index, pCon.wCon.curWeaponIndex
    /// </summary>
    public Weapon AddWeapon(Weapon weapon, int durabillity, string weaponData)
    {


        return weapon;
    }
    public void RemoveWeapon(int index)
    {
        Weapon weapon = weapons[index];
        weapon.OnWeaponDestroy();

        //���� ����
        if (index + 1 == weaponHolder.childCount) //������ ������ ������ ��
        {
            if (weaponCount == 2) //���Ⱑ �ϳ��� ��
            {
                SelectWeapon(0);
            }
            else SelectWeapon(index - 1); //���Ⱑ �� ���� ��
        }
        else SelectWeapon(index + 1);

        //����

        Destroy(weapon.gameObject);

        ResetWeaponIndex();
        if (weaponCount == 1) lastWeaponIndex = 0;
    }
    void ResetWeaponIndex()
    {
        for (int i = 0; i < 11; i++)
        {
            if (i < weaponCount)
            {
                weapons[i] = weaponHolder.GetChild(i).GetComponent<Weapon>();
                weapons[i].index = i;
            }
            else weapons[i] = null;
        }
        curWeaponIndex = curWeapon.index;
        if (lastWeaponIndex > weaponCount - 1)
        {
            Debug.LogError("lastWeaponIndex �ε��� �ʰ�");
            lastWeaponIndex = 0;
        } //lastWeaponIndex �ε��� �ʰ�
    }
}