using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public IInventoryItem item { get; private set; }

    public void Set(IInventoryItem item)
    {
        this.item = item;
        icon.sprite = item.Icon;
        icon.gameObject.SetActive(true);
    }
    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        InventoryManager.instance.summary.text = item.DisplayedName;
        InventoryManager.instance.summary.transform.parent.gameObject.SetActive(true);

        InventoryManager.instance.summary.rectTransform.parent.SetParent(transform);
        InventoryManager.instance.summary.rectTransform.parent.localPosition = Vector3.zero;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.instance.summary.transform.parent.gameObject.SetActive(false);
    }
}
