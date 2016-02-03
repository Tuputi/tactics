using UnityEngine;
using System.Collections;
[CreateAssetMenu]
public class ItemBase : ActionBaseClass {

    public int ItemCount = 3;
    public string ItemName = "NaN";
    public int ItemId = 0;
    public ItemType itemType;
    public Sprite ItemSprite;
    public int ItemMaxStackSize;

    [Header("Item effects")]
    public float EffectToRange = 0;
    public float EffectToTArgetArea = 0;
    public float EffectToDamageStatic = 0;
    public float EffectToDamageMultiplayer = 1f;


    //to distinguish between unique items at some point
    public int ItemInstanceIndex = 0;

    public ItemType GetItemType()
    {
        return itemType;
    }

    public virtual Sprite GetItemSprite(){
        return ItemSprite;
    }

    public override void  CompleteAction(Tile TargetTile)
    {
        TurnManager.instance.CurrentlyTakingTurn.CharacterInventory.Use(itemType);
        CalculateEffect(TargetTile);
    }


    //get effect from this.child effectscript?
    public override int CalculateEffect(Tile targetTile)
    {
        int healing = 0;
        if (targetTile.isOccupied)
        {
            healing = (int)(targetTile.tileCharacter.hpMax*0.3); //heal 30%
            CharacterLogic.instance.DisplayEffect(targetTile.tileCharacter, healing);
            targetTile.tileCharacter.hp += healing;
        }
        return healing;
    }

    public float GetRangeEffect()
    {
        return EffectToRange;
    }
}
