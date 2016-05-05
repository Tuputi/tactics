using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour {

    public Sprite EmptySlot;
    public Sprite SelectedSlot;
    public bool slotSelected = false;
    public Item MyItem = null;

    private Image MySlotImage;
    private Image itemSprite;

    public bool isEmpty = true;

    public Craftable accepts;
    private ItemType acceptableType;
    public bool specificItem = false;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        itemSprite = transform.FindChild("ItemSprite").GetComponent<Image>();
        itemSprite.gameObject.SetActive(false);
        isEmpty = true;
        MySlotImage = this.GetComponent<Image>();
        MySlotImage.sprite = EmptySlot;
    }

    public void AddItem(Item item)
    {
        MyItem = item;
        isEmpty = false;

        if (item.Sprite == null)
            Debug.Log("no sprite");

        itemSprite.gameObject.SetActive(true);
        itemSprite.sprite = item.Sprite;
        this.transform.localScale = new Vector3(1, 1, 1);
        TinkerItemDisplay.instance.UpdateCraftUI();
    }

    public void ClearSlot()
    {
        MyItem = null;
        isEmpty = true;
        itemSprite.gameObject.SetActive(false);
        TinkerItemDisplay.instance.UpdateCraftUI();
    }

    public void SelectItem()
    {
        
    }

    public void SelectSlot()
    {
        slotSelected = true;
        MySlotImage.sprite = SelectedSlot;
        Debug.Log("Selected");
    }

    public void UnselectSlot()
    {
        slotSelected = false;
        MySlotImage.sprite = EmptySlot;
    }

    public void ButtonPress()
    {
        if (this.slotSelected)
        {
            UnselectSlot();
            ClearSlot();
        }
        else
        {
            SelectSlot();
        }
    }

    public void SelectButton()
    {
        if (slotSelected)
        {
            UnselectSlot();
            ClearSlot();
            return;
        }

        SelectSlot();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        bool addItem = false;
        InventorySlot invSlot = col.gameObject.GetComponent<InventorySlot>();
        if (!(invSlot == null))
        {
            if (specificItem)
            {
                if(invSlot.MyItem == accepts)
                {
                    Debug.Log("specific item accepted");
                    addItem = true;
                }
            }
            else
            {
                if (invSlot.MyItem.itemCategories.Contains(accepts.itemCategories[0]))
                {
                    Debug.Log("Category correct");
                    addItem = true;
                }
            }
           
        }

        if (addItem)
        {
            AddItem(invSlot.MyItem);
            invSlot.UnselectSlot();
            col.gameObject.GetComponent<DraggableObject>().ReturnToOrigLocation();         
        }

    }

}
