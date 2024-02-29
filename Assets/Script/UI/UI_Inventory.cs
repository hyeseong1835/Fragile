using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] Image durabillity;
    
    GameObject curUI;
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
        if (curUI != null) curUI.SetActive(false);

        curUI = transform.GetChild(0).GetChild(index).gameObject;
        curUI.SetActive(true);
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
            Weapon weapon = Player.wCon.weaponHolder.GetChild(i).GetComponent<Weapon>();
            UISprite[weapon.index] = weapon.UI;
        }
        transform.GetChild(Player.wCon.curWeaponIndex).gameObject.SetActive(true);
        ResetDurabilityUI();
    }
    public void ResetDurabilityUI()
    {
        durabillity.fillAmount = Player.wCon.curWeapon.durability / Player.wCon.curWeapon.maxDurability;
    }
}
