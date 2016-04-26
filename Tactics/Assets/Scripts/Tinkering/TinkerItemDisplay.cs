using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TinkerItemDisplay : MonoBehaviour {

    public Button craftButton;
    public List<RecipeSlot> recipeSlots;
    public GameObject ItemInfo;
    public GameObject CraftUI;
    public static TinkerItemDisplay instance;
    public Recipe myRecipe;
    private int totalPP = 0;

    void Awake()
    {
        instance = this;
        InitiateCraftUI();
    }

    public void DisplayItemInfo(Craftable craft)
    {
        Image img = ItemInfo.GetComponentInChildren<Image>();
        img.sprite = craft.Sprite;

        Text pp_points = ItemInfo.GetComponentInChildren<Text>();
        pp_points.text = craft.PropertPoints.ToString();
    }

    public void InitiateCraftUI()
    {
        CraftUI.transform.FindChild("NeedPP").GetComponent<Text>().text = myRecipe.MinimunPropertyPoints.ToString();
    }

    public void UpdateCraftUI()
    {
        totalPP = 0;
        foreach (RecipeSlot slot in recipeSlots)
        {
            if (!slot.isEmpty)
            {
                totalPP += slot.MyItem.PropertPoints;
                Debug.Log("Total points changed to " + totalPP);
            }
        }
        CraftUI.transform.FindChild("TotalPP").GetComponent<Text>().text = totalPP.ToString();
    }

  


    public bool CheckIfAllSlotsFilled()
    {
        foreach (RecipeSlot slot in recipeSlots)
        {
            if (slot.isEmpty)
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckIfEnoughPP()
    {
        if(totalPP >= myRecipe.MinimunPropertyPoints)
        {
            return true;
        }
        return false;
    }

    public void CheckIfRecipeDone()
    {
        if( CheckIfAllSlotsFilled() && CheckIfEnoughPP())
        {
            Debug.Log("Recipe is done? yes");
        }
        else
            Debug.Log("Recipe is done? no");
    }
}
