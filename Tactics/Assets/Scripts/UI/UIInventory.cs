using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour {



    public List<GameObject> InventorySlots;
    public GameObject inventoryHolder;
    public GameObject inventorySlot;

    public int rows;
    public int columns;
    public float slotWidth;
    public float slotHeight;

    public float offsetWidth = 50;
    public float offsetHeight = 50;

    void Awake()
    {
        CreateInventory();
    }

    public void CreateInventory()
    {
        // float newWidth = (rows * slotWidth) + (rows * offsetWidth);
        //float newHeight = (columns * slotHeight) + (columns * offsetHeight);
        //inventoryHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(newHeight, newWidth);
        InventorySlots = new List<GameObject>();

        for (int i = 0; i <= rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                GameObject newSlot = Instantiate(inventorySlot);
                InventorySlots.Add(newSlot);
                //Debug.Log(InventorySlots.Count);
                newSlot.transform.SetParent(inventoryHolder.transform, false);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                //newSlot.transform.localPosition = new Vector3(0, 0, 0);
                newSlot.transform.localPosition = new Vector3((slotWidth * i) + offsetWidth, (-slotHeight * j) -offsetHeight, 0);
                newSlot.GetComponent<InventorySlot>().ClearSlot();
            }
        }
    }


    public void AddItem(ItemBase item)
    {
        foreach(GameObject slot in InventorySlots)
        {
            InventorySlot invSlot = slot.GetComponent<InventorySlot>();
           if (invSlot.isEmpty)
           {
                invSlot.AddItem(item.GetItemSprite(), item.ItemCount);
                slot.GetComponent<Button>().onClick.AddListener(delegate { invSlot.SelectItem(item); });
                Debug.Log("Added item to inventory " + item.ItemName + "," + item.ItemCount.ToString());
                return;
           }
        }
    }

    public void CloseInventory()
    {
        foreach(GameObject slot in InventorySlots)
        {
            InventorySlot invSlot = slot.GetComponent<InventorySlot>();
            if (!invSlot.isEmpty)
            {
                invSlot.ClearSlot();
            }
        }
        this.gameObject.SetActive(false);
    }
}
