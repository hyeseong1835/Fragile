using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    static Transform droppedItem;
    static GameObject itemPrefab;

    void Awake()
    {
        LoadWeapon("Weapon_WoodenSword,5");
    }
    public static GameObject SpawnItem(Weapon weapon, Vector3 pos, string data)
    {
        GameObject itemObj = Instantiate(itemPrefab, droppedItem);
        itemObj.AddComponent(weapon.GetType());
        itemObj.transform.position = pos;
        itemObj.name = data;

        return itemObj;
    }
    public static GameObject SpawnItem(string weapon, Vector3 pos, string data)
    {
        GameObject itemObj = Instantiate(itemPrefab, droppedItem);
        itemObj.AddComponent((new Item((Weapon)Activator.CreateInstance(Type.GetType(weapon)))).GetType());
        itemObj.transform.position = pos;
        itemObj.name = data;

        return itemObj;
    }
    public void SaveWeapons()
    {
        string weaponSaveData = "";
        for (int i = 0; i < Player.wCon.weaponCount; i++)
        {
            weaponSaveData +=
                Player.wCon.weapons[i].gameObject.name + "," +
                Player.wCon.weapons[i].durability.ToString() + "," +
                Player.wCon.weapons[i].LoadData() + "\n";
        }
    }

    public void LoadWeapons(string weaponDatas)
    {
        string[] weapons = weaponDatas.Split('\n');

        foreach (string weapon in weapons)
        {
            string[] split = weapon.Split('/');
            string[] datas = split[0].Split(',');
        }
    }
    public Weapon LoadWeapon(string weaponData)
    {
        //프리팹 가져오기
        string[] split = weaponData.Split('/');
        string[] datas = split[0].Split(',');

        Type componentType = Type.GetType(datas[0]);
        Player.wCon.Weapon.AddComponent(componentType);
        Player.wCon.AddWeapon(datas[0], int.Parse(datas[1]), split[1]));
    }


    public void LoadItems(string itemData)
    {
        string[] items = itemData.Split('\n');

        foreach (string item in items)
        {
            string[] split = item.Split('/');
            string[] datas = split[0].Split(',');
            SpawnItem(datas[0], new Vector3(float.Parse(datas[1]), float.Parse(datas[2]), 0), split[1]);
        }
    }
}
