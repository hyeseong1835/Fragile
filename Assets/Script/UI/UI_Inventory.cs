using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] WeaponController wCon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject AddToInventory(Weapon weapon)
    {
        StartCoroutine(AddToInventoryCoroutine());
        return Instantiate(weapon.UI, transform);
    }
    IEnumerator AddToInventoryCoroutine()
    {
        yield return null;
        ResetInventoryUI();
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
        transform.GetChild(wCon.curWeaponIndex).gameObject.SetActive(true);
        ResetDurabilityUI();
    }
    public void ResetDurabilityUI()
    {

    }
}
