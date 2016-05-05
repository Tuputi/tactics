using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TinkerItemDisplay : MonoBehaviour {

    public Button craftButton;
    public GameObject ItemInfo;
    public GameObject CraftUI;
    public static TinkerItemDisplay instance;

    void Awake()
    {
        instance = this;
        RecipeController.instance.CreateincredientSlots();
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
        CraftUI.transform.FindChild("NeedPP").GetComponent<Text>().text = RecipeController.instance.recipe.MinimunPropertyPoints.ToString();
    }

    public void UpdateCraftUI()
    {
        CraftUI.transform.FindChild("TotalPP").GetComponent<Text>().text = RecipeController.instance.GetCurrentPPCost().ToString();
    }

    public void CheckIfRecipeDone()
    {
        if(RecipeController.instance.IsPPAboveMinimun() && RecipeController.instance.AllSlotsFilled())
        {
            Debug.Log("Recipe completed succedfully");
        }
        else
        {
            Debug.Log("Recipe not yet complete");
        }
    }
}
