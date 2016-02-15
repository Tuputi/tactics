using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour {

    public Sprite EmptySlot;
    public Text ItemCount;
    public bool slotSelected = false;
    public ItemBase MyItem = null;

    UIInventory parentInventory;

    public bool isEmpty = true;

    public void AddItem(ItemBase item, Sprite sprite, int itemCount){
        if (itemCount > 1) {
            ItemCount.text = itemCount.ToString();
            MyItem = item;
        }
        else
        {
            ItemCount.text = "";
        }

        isEmpty = false;
        if(sprite == null)
        {
            Debug.Log("no sprite");
        }
        this.GetComponent<Image>().sprite = sprite;
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ClearSlot()
    {
        MyItem = null;
        ItemCount.text = "";
        isEmpty = true;
        this.GetComponent<Image>().sprite = EmptySlot;
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SelectItemForDisplay()
    {
        if(parentInventory == null)
        {
            parentInventory = GetComponentInParent<UIInventory>();
        }

        if (!slotSelected)
        {  
            UIManager.instance.DisplayItemInfo(MyItem);
            parentInventory.SelectASlot(this);
            return;
        }

       
    }

    public void SelectItem()
    {
        TurnManager.instance.Action(UIManager.instance.PendingActionType, MyItem);
        UIManager.instance.CloseInventory();
        UIManager.instance.CloseItemInfo();
        parentInventory.UnselectSlots();
    }
}
