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
            if (weaponCount == 1) SelectWeapon(0); //맨손
            else if (lastWeaponIndex != 0) SelectWeapon(lastWeaponIndex); //맨손을 제외하고 마지막으로 들었던 무기
        }
        if (Player.pCon.mouseWheelScroll > 0) //증가
        {
            if (curWeaponIndex == weaponCount - 1) //마지막 순서의 무기일 때
            {
                if (weaponCount == 1) SelectWeapon(0); //무기가 없을 때
                else SelectWeapon(1); //무기가 더 있을 때
            }
            else SelectWeapon(curWeaponIndex + 1); //다음 무기
        }
        else //감소
        {
            if (curWeaponIndex == 1) //첫 번째 순서의 무기일 때
            {
                if (weaponCount == 1) SelectWeapon(0); //무기가 없을 때
                else SelectWeapon(weaponCount - 1); //무기가 더 있을 때
            }
            else SelectWeapon(curWeaponIndex - 1); //이전 무기
        }
    }
    public void TakeItem(Weapon weapon, string data)
    {
        if (weaponCount > 11)
        {
            Debug.LogError("인벤토리 초과");
            DropItem(weapon);
            return;
        } //인벤토리 초과

        //GameObject weaponObj = Instantiate((GameObject) Resources.Load("Assets/Resources/WeaponObjPrefab/"), weaponHolder); //무기 오브젝트 생성
        weapon.index = weaponCount - 1;
        weapon.SetData(data.Split(','));

        if (weaponCount > 1) lastWeaponIndex = 1;
        inventoryUI.ResetInventoryUI();
    }
    void SelectWeapon(int index)
    {
        if (index < 0 || weaponCount < index + 1)
        {
            Debug.LogError("index가 범위를 초과함: (" + index + "/" + (weaponHolder.childCount - 1) + " )");
            SelectWeapon(0);
        } //LogError: "index가 범위를 초과함"

        for (int i = 0; i < weaponCount; i++) //무기 모두 비활성화
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
        }//LogError: "호출한 인덱스에 무기가 없음"

        curWeaponIndex = index;
        curWeapon = weapons[index];

        curWeapon.Use(true); //선택한 무기 활성화

        if (index != 0) lastWeaponIndex = index;
        inventoryUI.ChangeWeaponUI(index);
    }
    void DropItem(Weapon weapon)
    {
        GameObject item = ItemManager.SpawnItem(weapon, transform.position, weapon.LoadData());
        RemoveWeapon(weapon.index);
    }
    /// <summary>
    /// 무기 인덱스 초기화: Weapon.index, pCon.wCon.curWeaponIndex
    /// </summary>
    public Weapon AddWeapon(Weapon weapon, int durabillity, string weaponData)
    {


        return weapon;
    }
    public void RemoveWeapon(int index)
    {
        Weapon weapon = weapons[index];
        weapon.OnWeaponDestroy();

        //무기 선택
        if (index + 1 == weaponHolder.childCount) //마지막 순서의 무기일 때
        {
            if (weaponCount == 2) //무기가 하나일 때
            {
                SelectWeapon(0);
            }
            else SelectWeapon(index - 1); //무기가 더 있을 때
        }
        else SelectWeapon(index + 1);

        //제거

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
            Debug.LogError("lastWeaponIndex 인덱스 초과");
            lastWeaponIndex = 0;
        } //lastWeaponIndex 인덱스 초과
    }
}