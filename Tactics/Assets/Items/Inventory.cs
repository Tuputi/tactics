using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Inventory{

    private List<Item> inventory;

    public Inventory()
    {
        inventory = new List<Item>();
    }

    public void Add(Item item)
    {
        inventory.Add(item);
    }

    public bool Contains(Item item)
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
        Item foundItem = inventory.Find(invItem => invItem.ItemInstanceID == itemId);
        foundItem.ItemCount--;

        //badfix
        if(itemId == -1)
        {
            RemoveItem(itemId);
        }
    }

    private void RemoveItem(int itemId)
    {
        Item foundItem = inventory.Find(invItem => invItem.ItemInstanceID == itemId);
        inventory.Remove(foundItem);
    }

    public void AddToCount(int itemID)
    {
        Item foundItem = inventory.Find(invItem => invItem.ItemInstanceID == itemID);
        if(foundItem == null)
        {
            Debug.Log("Item not found");
            return;
        }
        foundItem.ItemCount++;
    }

    public List<Item> GetWholeInventory()
    {
        return inventory;
    }

    public Item GetItem(int itemId)
    {
        Item foundItem = inventory.Find(item => item.ItemInstanceID == itemId);
        return foundItem;
    }

}
