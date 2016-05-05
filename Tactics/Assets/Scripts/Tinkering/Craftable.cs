using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu]

public class Craftable : ScriptableObject {


    public Sprite Sprite;
    public string Name;
    public List<ItemType> itemCategories;
    public int ItemCount = 1;

    [TextArea]
    public string Description;

    [Header("TinkerProperties")]
    public int PropertPoints = 4;

    public string GetName()
    {
        if (Name.Equals("Category"))
        {
            return itemCategories[0].ToString();
        }
        return Name;
    }
}
