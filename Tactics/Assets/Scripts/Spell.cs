using UnityEngine;
using System.Collections.Generic;

public class Spell : Item {

    public void SpellInit(string SpellName, float targetArea, float range, List<Elements> elementEffects, TargetAreaType tat)
    {
        Name = SpellName;
        EffectToTArgetArea = targetArea;
        EffectToRange = range;
        addElement = elementEffects;
        itemCategories = new List<ItemType>();
        itemCategories.Add(ItemType.Spell);
        targetAreaType = tat;
        ItemInstanceID = -1;
    }

}
