using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RecipeController : MonoBehaviour {

    public Recipe recipe;
    public RecipeSlot reipeSlotTemplate;
    public static RecipeController instance;

    List<RecipeSlot> slots;

    void Awake()
    {
        instance = this;
    }

    public void CreateincredientSlots()
    {
        slots = new List<RecipeSlot>();
        foreach(Craftable incredient in recipe.Incredients)
        {
            RecipeSlot newSlot = Instantiate(reipeSlotTemplate);
            newSlot.transform.GetComponentInChildren<Text>().text = incredient.GetName();
            if (!incredient.Name.Equals("Category"))
            {
                newSlot.specificItem = true;
            }
            newSlot.accepts = incredient;


            newSlot.transform.SetParent(this.transform);
            newSlot.transform.localScale = new Vector3(1, 1, 1);
            newSlot.transform.localPosition = new Vector3(slots.Count * reipeSlotTemplate.gameObject.GetComponent<RectTransform>().rect.width, 0);
            slots.Add(newSlot);
        }
    }

    public int GetCurrentPPCost()
    {
        int cost = 0;
        foreach(RecipeSlot slot in slots)
        {
            if (slot.MyItem)
            {
                cost += slot.MyItem.PropertPoints;
            }
        }
        return cost;
    }

    public bool AllSlotsFilled()
    {
        bool filled = false;
        foreach(RecipeSlot slot in slots)
        {
            filled = slot.MyItem;
        }
        return filled;
    }

    public bool IsPPAboveMinimun()
    {
        return GetCurrentPPCost() >= recipe.MinimunPropertyPoints;
    }
}
