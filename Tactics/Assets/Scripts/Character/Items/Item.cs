using UnityEngine;
using System.Collections;

public class Item : ActionBase {

    public int ItemCount;
    public string ItemName = "NaN";
    public ItemType itemType;

    public ItemType GetItemType()
    {
        itemType = ItemType.Bomb;
        return itemType;
    }

    public void Use()
    {
        if(--ItemCount > 0)
        {
            Debug.Log("Use");
            ItemCount--;
        }
        else
        {
            Debug.Log("Run ut of items");
        }
    }

}
