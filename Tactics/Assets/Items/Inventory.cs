using UnityEngine;
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

    public int GetItemCount(int itemIndex)
    {
        int index = inventory.FindIndex(item => (item.ItemInstanceID == itemIndex));
        return inventory[index].ItemCount;
    }

    public void Use(int itemId)
    {
        ItemBase foundItem = inventory.Find(invItem => invItem.ItemInstanceID == itemId);
        foundItem.ItemCount--;

        //badfix
        if(itemId == -1)
        {
            RemoveItem(itemId);
        }
    }

    private void RemoveItem(int itemId)
    {
        ItemBase foundItem = inventory.Find(invItem => invItem.ItemInstanceID == itemId);
        inventory.Remove(foundItem);
    }

    public void AddToCount(int itemID)
    {
        ItemBase foundItem = inventory.Find(invItem => invItem.ItemInstanceID == itemID);
        if(foundItem == null)
        {
            Debug.Log("Item not found");
            return;
        }
        foundItem.ItemCount++;
    }

    public List<ItemBase> GetWholeInventory()
    {
        return inventory;
    }

    public ItemBase GetItem(int itemId)
    {
        ItemBase foundItem = inventory.Find(item => item.ItemInstanceID == itemId);
        return foundItem;
    }

}
