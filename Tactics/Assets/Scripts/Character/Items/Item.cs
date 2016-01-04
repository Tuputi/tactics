using UnityEngine;
using System.Collections;
[CreateAssetMenu]
public class Item : ActionBase {

    public int ItemCount = 3;
    public string ItemName = "NaN";
    public ItemType itemType;

    public ItemType GetItemType()
    {
        itemType = ItemType.Potion;
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
            Debug.Log("Run out of items");
        }
    }

    public override void  CompleteAction(Tile TargetTile)
    {
        Use();
        CalculateDamage(TargetTile);
    }

    public override int CalculateDamage(Tile targetTile)
    {
        int healing = 0;
        if (targetTile.isOccupied)
        {
            healing = (int)(targetTile.tileCharacter.hp * 0.3); //heal 30%
            targetTile.tileCharacter.hp += healing;
        }
        return healing;
    }
}
