using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
using TMPro;
public interface IInventoryItem
{
    public Sprite Icon { get; }
    public string DisplayedName { get; }
}
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public Canvas canvas;
    public Transform content;
    public GameObject slotPrefab;
    public TextMeshProUGUI summary;

    [ShowInInspector][NonSerialized]
    public List<InventorySlot> slots = new List<InventorySlot>();

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Resize(10);
    }
    [Button("¹«±â Àç·á Ãß°¡")]
    void AddItem(WeaponMaterialData data) 
    {
        AddItem(new WeaponMaterial(data));
    }
    public void AddItem(IInventoryItem item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].Set(item);
                return;
            }
        }
        Debug.LogError("ÀÎº¥ ²Ë Ã¡¾î¿ä!");
    }
    public int AddItem(IInventoryItem item, int startIndex)
    {
        for (int i = startIndex; i < slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].Set(item);
                return i;
            }
        }
        Debug.LogError("ÀÎº¥ ²Ë Ã¡¾î¿ä!");
        return -1;
    }
    public void Resize(int length)
    {
        if (length < slots.Count)
        {
            int fillIndex = 0;
            for (int i = length; i < slots.Count; i++)
            {
                if (slots[i].item != null)
                {
                    fillIndex = AddItem(slots[i].item, fillIndex);
                }
            }
            for (int i = slots.Count - 1; i >= length; i--)
            {
                Destroy(slots[i]);
            }
            slots.RemoveRange(length, slots.Count - length);
        }
        else
        {
            for (int i = slots.Count + 1; i <= length; i++)
            {
                InventorySlot slot = Instantiate(slotPrefab, content).GetComponent<InventorySlot>();
                slot.gameObject.name = $"Slot ({i})";
                slots.Add(slot);
            }
        }
    }
}
