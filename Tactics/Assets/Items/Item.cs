using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu]
public class Item : Craftable {

    public int ItemCount = 3;
    public int ItemMaxStackSize;

    //do not cahnge this outside of code
    public int ItemInstanceID = 0;

    

    [Header("Item effects")]
    public float EffectToRange = 0;
    public float EffectToTArgetArea = 0;
    public float EffectToDamageStatic = 0;
    public float EffectToDamageMultiplayer = 1f;
    public List<Elements> addElement;
    public TargetAreaType targetAreaType = TargetAreaType.none;



   

    public void Init(int itemCount,string itemName, Sprite itemSprite, int itemMaxStackSize)
    {
        ItemCount = itemCount;
        Name = itemName;
        Sprite = itemSprite;
        ItemMaxStackSize = itemMaxStackSize;
    }

    public void InitEffect(float effectToRange, float effectToTargetArea, float effectToDamageStatic, float effectToDamageMultiplayer, TargetAreaType tat)
    {
        EffectToRange = effectToRange;
        EffectToTArgetArea = effectToTargetArea;
        EffectToDamageStatic = effectToDamageStatic;
        EffectToDamageMultiplayer = effectToDamageMultiplayer;
        targetAreaType = tat;
    }

    public void InitElement(List<Elements> elements)
    {
        addElement = new List<Elements>(elements);
    }

    public int GetItemId()
    {
        return ItemInstanceID;
    }

    public virtual Sprite GetItemSprite(){
        return Sprite;
    }

   /* public override void  CompleteAction(Tile TargetTile)
    {
        TurnManager.instance.CurrentlyTakingTurn.CharacterInventory.Use(ItemInstanceID);
        CalculateEffect(TargetTile);
    }


    //get effect from this.child effectscript?
    public override int CalculateEffect(Tile targetTile)
    {
        int healing = 0;
        if (targetTile.isOccupied)
        {
            healing = (int)(targetTile.tileCharacter.hpMax*0.3); //heal 30%
           CharacterLogic.instance.DisplayEffect(targetTile.tileCharacter, healing, DisplayTexts.none);
            targetTile.tileCharacter.Hp += healing;
        }
        return healing;
    }*/

    public float GetRangeEffect()
    {
        return EffectToRange;
    }
}
