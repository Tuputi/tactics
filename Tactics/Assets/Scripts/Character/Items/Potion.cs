using UnityEngine;
using System.Collections;

public class Potion : ItemBase {

    public override string GetName()
    {
        ItemName = "Potion";
        return ItemName;
    }

    public override Sprite GetItemSprite()
    {
        ItemSprite = PrefabHolder.instance.PotionSprite;
        return base.GetItemSprite();
    }

}
