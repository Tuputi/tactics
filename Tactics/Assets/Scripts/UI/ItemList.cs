using UnityEngine;
using System.Collections.Generic;

public class ItemList : MonoBehaviour {

    public List<Item> items;

    public static Dictionary<int, Item> itemDictionary;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        int id = 0;
        itemDictionary = new Dictionary<int, Item>();
        foreach(Item item in items)
        {
            item.ItemInstanceID += id;
            itemDictionary.Add(item.ItemInstanceID, item);
            id++;
        }
    }

    public static Item GetItem(int itemId)
    {
       return itemDictionary[itemId];
    }

}
