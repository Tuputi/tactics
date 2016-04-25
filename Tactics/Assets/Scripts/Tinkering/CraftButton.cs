using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour {

    public Button craftButton;
    public List<RecipeSlot> recipeSlots;


    public bool CheckIfAllSlotsFilled()
    {
        foreach(RecipeSlot slot in recipeSlots)
        {
            if (slot.isEmpty)
            {
                return false;
            }
        }
        return true;
    }

    public void CheckRecipe()
    {
        bool temp = CheckIfAllSlotsFilled();
        Debug.Log("Recipe is done? " + temp);
    }
}
