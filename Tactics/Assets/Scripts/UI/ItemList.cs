using UnityEngine;
using System.Collections.Generic;

public class ItemList : MonoBehaviour {

    public List<ItemBase> items;

    public static Dictionary<ItemType, ItemBase> itemDictionary;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        itemDictionary = new Dictionary<ItemType, ItemBase>();
        foreach(ItemBase item in items)
        {
            itemDictionary.Add(item.itemType, item);
        }
    }

    public static ItemBase GetItem(ItemType itemType)
    {
       return itemDictionary[itemType];
    }

}
