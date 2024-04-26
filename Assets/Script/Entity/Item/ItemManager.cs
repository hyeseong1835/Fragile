using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteAlways]
public class ItemManager : MonoBehaviour
{
    public static Item InstantiateEmptyItem(string weaponName)
    {
        GameObject itemObj = new GameObject("Item(" + weaponName + ")");
        return itemObj.AddComponent<Item>();
    }
    /// <summary>
    /// ItemData -> Item
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Item LoadItem(ItemData data)
    {
        Item item = InstantiateEmptyItem(data.weaponData.name);

        item.weapon = Utility.LoadWeapon(data.weaponData);

        item.transform.position = data.pos;

        return item;
    }
    /// <summary>
    /// {weaponName} -> Item
    /// </summary>
    /// <param name="weaponName"></param>
    /// <returns></returns>
    public static Item SpawnItem(string weaponName)
    {
        return WrapWeaponInItem(Utility.SpawnWeapon(weaponName));
    }
    /// <summary>
    /// {weapon} -> Item
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    public static Item WrapWeaponInItem(Weapon weapon)
    {
        Item item = InstantiateEmptyItem(weapon.weaponName);
        item.weapon = weapon;

        weapon.transform.SetParent(item.transform);

        weapon.state = WeaponState.ITEM;

        return item;
    }
    /// <summary>
    /// Item -> Weapon(NULL)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static Weapon UnWrapItem(Item item)
    {
        Weapon weapon = item.weapon;

        weapon.transform.SetParent(null);
        weapon.state = WeaponState.NULL;

        Destroy(item.gameObject);

        return weapon;
    }
}
