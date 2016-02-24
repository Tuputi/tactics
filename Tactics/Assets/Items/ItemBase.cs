using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu]
public class ItemBase : ActionBaseClass {

    public int ItemCount = 3;
    public string ItemName = "NaN";
    public int ItemId = 0;
    public ItemType itemType;
    public Sprite ItemSprite;
    public int ItemMaxStackSize;

    public List<ItemType> itemCategories;

    [Header("Item effects")]
    public float EffectToRange = 0;
    public float EffectToTArgetArea = 0;
    public float EffectToDamageStatic = 0;
    public float EffectToDamageMultiplayer = 1f;
    public List<Elements> addElement;


    //to distinguish between unique items at some point
    public int ItemInstanceIndex = 0;

    public void Init(int itemCount,string itemName, ItemType ItemType, Sprite itemSprite, int itemMaxStackSize)
    {
        ItemCount = itemCount;
        ItemName = itemName;
       // ItemId = itemId;
        itemType = ItemType;
        ItemSprite = itemSprite;
        ItemMaxStackSize = itemMaxStackSize;
    }

    public void InitEffect(float effectToRange, float effectToTargetArea, float effectToDamageStatic, float effectToDamageMultiplayer)
    {
        EffectToRange = effectToRange;
        EffectToTArgetArea = effectToTargetArea;
        EffectToDamageStatic = effectToDamageStatic;
        EffectToDamageMultiplayer = effectToDamageMultiplayer;
    }

    public void InitElement(List<Elements> elements)
    {
        addElement = new List<Elements>(elements);
    }

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
            targetTile.tileCharacter.DisplayEffect( healing, DisplayTexts.none);
            targetTile.tileCharacter.Hp += healing;
        }
        return healing;
    }

    public float GetRangeEffect()
    {
        return EffectToRange;
    }
}
