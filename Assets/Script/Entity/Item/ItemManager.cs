using System;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    static Transform droppedItem;
    static GameObject itemPrefab;

    void Awake()
    {

    }
    public static Item LoadItem(ItemData data)
    {
        GameObject itemObj = Instantiate(itemPrefab);
        itemObj.name = "Item_" + data.name;

        Item item = itemObj.GetComponent<Item>();
        item.weaponName = data.name;

        item.weapon = Utility.LoadWeapon(data.name, data.weaponData);

        item.transform.position = data.pos;

        return item;
    }
    public static Item SpawnItem(string weaponName)
    {
        GameObject itemObj = Instantiate(itemPrefab);
        itemObj.name = "Item_" + weaponName;

        Item item = itemObj.GetComponent<Item>();
        item.weaponName = weaponName;
        item.weapon = Utility.LoadWeapon(weaponName);

        return item;
    }
    public static Item WeaponToItem(Weapon weapon)
    {
        Item item = Instantiate(itemPrefab).GetComponent<Item>();

        weapon.transform.SetParent(item.transform);
        weapon.transform.localPosition = Vector3.zero;
     
        item.weaponName = weapon.weaponName;
        item.weapon = weapon;

        return item;
    }
}
