using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour {



    public List<InventorySlot> InventorySlots;
    public GameObject inventoryHolder;
    public InventorySlot inventorySlot;

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
        InventorySlots = new List<InventorySlot>();

        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                InventorySlot newSlot = Instantiate(inventorySlot);
                InventorySlots.Add(newSlot);
                newSlot.transform.SetParent(inventoryHolder.transform, false);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.transform.localPosition = new Vector3((slotWidth * i) + offsetWidth, (-slotHeight * j) -offsetHeight, 0);
                newSlot.GetComponent<InventorySlot>().Init();
            }
        }
    }


    public void AddItem(Item item)
    {
        foreach (InventorySlot slot in InventorySlots)
        {
            if (slot.isEmpty)
           {
                slot.AddItem(item);
                //slot.GetComponent<Button>().onClick.AddListener(delegate { slot.SelectItemForDisplay(); });
               // Debug.Log("Added item to inventory " + item.ItemName + "," + item.ItemCount.ToString());
                return;
           }
        }
    }

    public virtual void CloseInventory()
    {
        UnselectSlots();
        foreach(InventorySlot slot in InventorySlots)
        {
            if (!slot.isEmpty)
            {
                slot.ClearSlot();
            }
        }
        this.gameObject.SetActive(false);
    }

    public virtual void CloseInventoryAfterAttack()
    {
        UnselectSlots();
        foreach (InventorySlot slot in InventorySlots)
        {
            if (!slot.isEmpty)
            {
                slot.ClearSlot();
            }
        }
        this.gameObject.SetActive(false);
    }

    public virtual void SelectASlot(InventorySlot iS)
    {
        foreach(InventorySlot go in InventorySlots)
        {
            go.UnselectSlot();
        }
        iS.SelectSlot();
        TurnManager.instance.Action(UIManager.instance.PendingActionType, iS.MyItem);
    }

    public void UnselectSlots()
    {
        foreach (InventorySlot go in InventorySlots)
        {
            go.UnselectSlot();
        }
    }
}
