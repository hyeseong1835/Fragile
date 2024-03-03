using System;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    static Transform droppedItem;
    static GameObject itemPrefab;

    void Awake()
    {

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
