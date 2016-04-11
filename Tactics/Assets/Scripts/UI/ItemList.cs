using UnityEngine;
using System.Collections.Generic;

public class ItemList : MonoBehaviour {

    public List<ItemBase> items;

    public static Dictionary<int, ItemBase> itemDictionary;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        int id = 0;
        itemDictionary = new Dictionary<int, ItemBase>();
        foreach(ItemBase item in items)
        {
            item.ItemInstanceID += id;
            itemDictionary.Add(item.ItemInstanceID, item);
            id++;
        }
    }

    public static ItemBase GetItem(int itemId)
    {
       return itemDictionary[itemId];
    }

}
