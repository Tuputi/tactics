﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Inventory{

    private List<ItemBase> inventory;

    public Inventory()
    {
        inventory = new List<ItemBase>();
    }

    public void Add(ItemBase item)
    {
        inventory.Add(item);
    }

    public bool Contains(ItemBase item)
    {
        return inventory.Contains(item);
    }

    public bool Contains(ItemType itemType)
    {
        foreach (ItemBase ib in inventory)
        {
            if (ib.GetItemType() == itemType)
            {
                return true;
            }
        }
        return false;
    }

    public int GetItemCount(ItemType itemType)
    {
        int index = inventory.FindIndex(item => (item.GetItemType() == itemType));
        return inventory[index].ItemCount;
    }


    public void Use(ItemType itemType)
    {
        ItemBase foundItem = inventory.Find(invItem => invItem.GetItemType() == itemType);
        foundItem.ItemCount--;
    }

    public List<ItemBase> GetWholeInventory()
    {
        return inventory;
    }

    public void Remove(ItemBase item)
    {
        inventory.Remove(item);
    }


}