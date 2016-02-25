﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour {



    public List<InventorySlot> InventorySlots;
    public GameObject inventoryHolder;
    public InventorySlot inventorySlot;
    public ItemBase SelectedItem;

    public int rows;
    public int columns;
    public float slotWidth;
    public float slotHeight;

    public float offsetWidth = 50;
    public float offsetHeight = 50;

    void Awake()
    {
        CreateInventory();
    }

    public void CreateInventory()
    {
        InventorySlots = new List<InventorySlot>();

        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                InventorySlot newSlot = Instantiate(inventorySlot);
                InventorySlots.Add(newSlot);
                newSlot.transform.SetParent(inventoryHolder.transform, false);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.transform.localPosition = new Vector3((slotWidth * i) + offsetWidth, (-slotHeight * j) -offsetHeight, 0);
                newSlot.GetComponent<InventorySlot>().Init();
            }
        }
    }


    public void AddItem(ItemBase item)
    {
        Debug.Log(InventorySlots.Count);
        foreach (InventorySlot slot in InventorySlots)
        {
            if (slot.isEmpty)
           {
                slot.AddItem(item, item.GetItemSprite(), item.ItemCount);
                slot.GetComponent<Button>().onClick.AddListener(delegate { slot.SelectItemForDisplay(); });
                Debug.Log("Added item to inventory " + item.ItemName + "," + item.ItemCount.ToString());
                return;
           }
        }
    }

    public void CloseInventory()
    {
        UnselectSlots();
        foreach(InventorySlot slot in InventorySlots)
        {
            if (!slot.isEmpty)
            {
                slot.ClearSlot();
            }
        }
        this.gameObject.SetActive(false);
    }

    public void SelectASlot(InventorySlot iS)
    {
        foreach(InventorySlot go in InventorySlots)
        {
            go.UnselectSlot();
        }
        iS.SelectSlot();
    }

    public void UnselectSlots()
    {
        foreach (InventorySlot go in InventorySlots)
        {
            go.UnselectSlot();
        }
    }
}
