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
            Weapon weapon = Player.wCon.weapons[i];
            UISprite[weapon.index] = weapon.UI;
        }
        transform.GetChild(Player.wCon.weaponCount).gameObject.SetActive(true);
        ResetDurabilityUI();
    }
    public void ResetDurabilityUI()
    {
        durabillity.fillAmount = Player.wCon.curWeapon.durability / Player.wCon.curWeapon.maxDurability;
    }
}
