using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour
{
    [NonSerialized] public Canvas canvas;

    public RectTransform rect;
    public RectTransform inventoryPanel;
    public RectTransform blueprintPanel;
    public RectTransform blueprint;
    public float space;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        blueprintPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, blueprint.rect.width);
        inventoryPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.rect.width - blueprint.rect.width - space);
    }
    void Update()
    {
        
    }
}
