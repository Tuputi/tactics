using UnityEngine;
using System.Collections;
[CreateAssetMenu]
public class ItemBase : ActionBaseClass {

    public int ItemCount = 3;
    public string ItemName = "NaN";
    public ItemType itemType;

    public ItemType GetItemType()
    {
        itemType = ItemType.Potion;
        return itemType;
    }

    public override void  CompleteAction(Tile TargetTile)
    {
        TurnManager.instance.CurrentlyTakingTurn.CharacterInventory.Use(itemType);
        CalculateEffect(TargetTile);
    }

    public override int CalculateEffect(Tile targetTile)
    {
        int healing = 0;
        if (targetTile.isOccupied)
        {
            healing = (int)(targetTile.tileCharacter.hp * 0.3); //heal 30%
            CharacterLogic.instance.DisplayEffect(targetTile.tileCharacter, healing);
            targetTile.tileCharacter.hp += healing;
        }
        return healing;
    }
}
