﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour {

    public Sprite EmptySlot;
    public Text ItemCount;
    public bool slotSelected = false;

    UIInventory parentInventory;

    public bool isEmpty = true;

    public void AddItem(Sprite sprite, int itemCount){
        if(itemCount > 1) {
            ItemCount.text = itemCount.ToString();
        }
        else
        {
            ItemCount.text = "";
        }

        isEmpty = false;
        if(sprite == null)
        {
            Debug.Log("no sprite");
        }
        this.GetComponent<Image>().sprite = sprite;
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ClearSlot()
    {
        ItemCount.text = "";
        isEmpty = true;
        this.GetComponent<Image>().sprite = EmptySlot;
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SelectItem(ItemBase item)
    {
        if(parentInventory == null)
        {
            parentInventory = GetComponentInParent<UIInventory>();
        }

        if (!slotSelected)
        {  
            UIManager.instance.DisplayItemInfo(item);
            parentInventory.SelectASlot(this);
            return;
        }

        TurnManager.instance.Action(UIManager.instance.PendingActionType, item);
        UIManager.instance.CloseInventory();
        UIManager.instance.CloseItemInfo();
        parentInventory.UnselectSlots();
    }

}
