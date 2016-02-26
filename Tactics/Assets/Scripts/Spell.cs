using UnityEngine;
using System.Collections.Generic;

public class Spell : ItemBase {

    public void SpellInit(string SpellName, float targetArea, float range, List<Elements> elementEffects)
    {
        ItemName = SpellName;
        EffectToTArgetArea = targetArea;
        EffectToRange = range;
        addElement = elementEffects;
        itemType = ItemType.Spell;
    }

}
