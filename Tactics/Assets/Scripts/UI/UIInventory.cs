using UnityEngine;
using System.Collections;

public class UIInventory : MonoBehaviour {

    public GameObject inventoryHolder;
    public GameObject inventorySlot;

    public int rows;
    public int columns;
    public float slotWidth;
    public float slotHeight;

    public float offsetWidth = 50;
    public float offsetHeight = 50;

    void Start()
    {
        CreateInventory();
    }

    public void CreateInventory()
    {
       // float newWidth = (rows * slotWidth) + (rows * offsetWidth);
        //float newHeight = (columns * slotHeight) + (columns * offsetHeight);
        //inventoryHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(newHeight, newWidth);
        for(int i = 0; i <= rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                GameObject newSlot = Instantiate(inventorySlot);
                newSlot.transform.SetParent(inventoryHolder.transform);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                //newSlot.transform.localPosition = new Vector3(0, 0, 0);
                newSlot.transform.localPosition = new Vector3((slotWidth * i) + offsetWidth, (-slotHeight * j) -offsetHeight, 0);
            }
        }
    }
}
