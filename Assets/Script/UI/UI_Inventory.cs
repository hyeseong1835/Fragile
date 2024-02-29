using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] Image durabillity;
    
    [SerializeField] Image UI;
    [SerializeField] Sprite[] UISprite = new Sprite[11];
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
        if (UISprite[index] != null) UI.sprite = UISprite[index];
    }
    public void AddToInventory(Weapon weapon)
    {
        UISprite[weapon.index] = weapon.UI;
    }
    public void ResetInventoryUI()
    {
        UISprite = new Sprite[11];
        for (int i = 0; i < transform.childCount; i++) //모두 비활성화
        {
            Weapon weapon = WeaponController.weaponHolder.GetChild(i).GetComponent<Weapon>();
            UISprite[weapon.index] = weapon.UI;
        }
        transform.GetChild(WeaponController.curWeaponIndex).gameObject.SetActive(true);
        ResetDurabilityUI();
    }
    public void ResetDurabilityUI()
    {
        durabillity.fillAmount = WeaponController.curWeapon.durability / WeaponController.curWeapon.maxDurability;
    }
}
