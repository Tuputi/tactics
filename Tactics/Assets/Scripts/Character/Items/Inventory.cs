using UnityEngine;
using System.Collections.Generic;

public class Inventory{

    private List<Item> inventory;

    public void Add(Item item)
    {
        inventory.Add(item);
    }

    public void Remove(Item item)
    {
        inventory.Remove(item);
    }
}
