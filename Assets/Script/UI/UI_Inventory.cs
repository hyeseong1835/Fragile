using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] Image durabillity;

    GameObject curUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeWeaponUI(int index)
    {
        curUI.SetActive(false);

        curUI = transform.GetChild(0).GetChild(index).gameObject;
        curUI.SetActive(true);
    }
    public GameObject AddToInventory(Weapon weapon)
    {
        GameObject weaponUI = Instantiate(weapon.UI, transform);
        ResetInventoryUI();
        return weaponUI;
    }
    public void RemoveToInventory(Weapon weapon)
    {
        Destroy(weapon.UI);
        ResetInventoryUI();
    }
    public void RemoveToInventory(int index)
    {
        Destroy(transform.GetChild(index).gameObject);
        ResetInventoryUI();
    }
    public void ResetInventoryUI()
    {
        for (int i = 0; i < transform.childCount; i++) //모두 비활성화
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(Player.wCon.curWeaponIndex).gameObject.SetActive(true);
        ResetDurabilityUI();
    }
    public void ResetDurabilityUI()
    {
        durabillity.fillAmount = Player.wCon.curWeapon.durability / Player.wCon.curWeapon.maxDurability;
    }
}
