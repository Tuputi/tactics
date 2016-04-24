using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DisplayRecipe : MonoBehaviour {

    public Recipe myRecipe;
    public List<Text> recipeSlots;
    public Image ResultImage;


    void Start()
    {
        int i = 0;
        foreach(Craftable craftMat in myRecipe.Incredients)
        {
            if (craftMat.Name.Equals("Category"))
            {
                recipeSlots[i].text = craftMat.itemCategories[0].ToString();
            }
            else
            {
                recipeSlots[i].text = craftMat.Name;
            }
            i++;
        }

        ResultImage.sprite = myRecipe.Result.Sprite;
    }
}
