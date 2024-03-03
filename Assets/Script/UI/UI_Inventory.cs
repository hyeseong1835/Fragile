using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] Image durabillity;
    [SerializeField] Image UI;

    [ShowInInspector] Sprite[] UISprite = new Sprite[11];

    public void ChangeWeaponUI(int index)
    {
        if (UISprite[index] != null) UI.sprite = UISprite[index];
    }
    public void AddToInventory(Weapon weapon)
    {
        if(weapon.UI == null)
        {
            Debug.LogError(weapon.gameObject.name+"(Weapon).UI is null");
            ResetInventoryUI();
        }//LogError: {name}(Weapon).UI is null
        UISprite[weapon.index] = weapon.UI;
    }
    public void ResetInventoryUI()
    {
        UISprite = new Sprite[11];
        for (int i = 0; i < Player.wCon.transform.childCount; i++)
        {
            UISprite[i] = Player.wCon.weapons[i].UI;
        }
        UI.sprite = UISprite[Player.wCon.curWeapon.index];
        ResetDurabilityUI();
    }
    public void ResetDurabilityUI()
    {
        if (Player.wCon.curWeapon.maxDurability == 0)
        {
            Debug.LogError(Player.wCon.curWeapon.name + "의 maxDurabillity는 0일 수 없습니다");
            Player.wCon.curWeapon.maxDurability = 1;
        } //{name}의 maxDurabillity는 0일 수 없습니다
        durabillity.fillAmount = (float) Player.wCon.curWeapon.durability / Player.wCon.curWeapon.maxDurability;
    }
}