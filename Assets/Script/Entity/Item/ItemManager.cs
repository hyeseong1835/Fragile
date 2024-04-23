using Sirenix.OdinInspector;
using System;
using UnityEngine;

[ExecuteAlways]
public class ItemManager : MonoBehaviour
{
    static Transform droppedItem;
    [ShowInInspector][ReadOnly] static GameObject itemPrefab;

    void Update()
    {
        if (itemPrefab == null) itemPrefab = Resources.Load<GameObject>("ItemPrefab");
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
    public static Item WrapWeaponInItem(Weapon weapon)
    {
        Item item = Instantiate(itemPrefab).GetComponent<Item>();

        weapon.transform.SetParent(item.transform);
     
        item.weaponName = weapon.weaponName;
        item.weapon = weapon;
        item.gameObject.name = "Item";

        return item;
    }
}
