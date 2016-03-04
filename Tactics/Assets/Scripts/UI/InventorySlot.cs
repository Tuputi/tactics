using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour {

    public Sprite EmptySlot;
    public Sprite SelectedSlot;
    public Text ItemCount;
    public int itemCountInt;
    public bool slotSelected = false;
    public ItemBase MyItem = null;
    private Image MySlotImage;
    private Image itemSprite;
    private Image numberBG;

    public UIInventory parentInventory;

    public bool isEmpty = true;

    public void Init()
    {
        parentInventory = GetComponentInParent<UIInventory>();
        itemSprite = transform.FindChild("ItemSprite").GetComponent<Image>();
        itemSprite.gameObject.SetActive(false);
        numberBG = transform.FindChild("NumberBG").GetComponent<Image>();
        numberBG.gameObject.SetActive(false);
        MyItem = null;
        ItemCount.text = "";
        isEmpty = true;
        MySlotImage = this.GetComponent<Image>();
        MySlotImage.sprite = EmptySlot;
    }

    public void AddItem(ItemBase item, Sprite sprite, int itemCount){
        MyItem = item;

        if (itemCount > 1)
        {
            numberBG.gameObject.SetActive(true);
            ItemCount.text = itemCount.ToString();
            itemCountInt = itemCount;
            itemSprite.color = Color.white;
        }
        else if(itemCount == 1){
            numberBG.gameObject.SetActive(false);
            ItemCount.text = "";
            itemCount = 1;
            itemSprite.color = Color.white;
        }
        else
        {
            ItemCount.text = "";
            itemCount = 1;
            itemSprite.color = Color.grey;
        }

        isEmpty = false;

        if(sprite == null)
            Debug.Log("no sprite");

        itemSprite.gameObject.SetActive(true);
        itemSprite.sprite = sprite;
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ClearSlot()
    {
        MyItem = null;
        numberBG.gameObject.SetActive(false);
        ItemCount.text = "";
        isEmpty = true;
        itemSprite.gameObject.SetActive(false);
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SelectItemForDisplay()
    {
        if(MyItem == null)
        {
            return;
        }
      UIManager.instance.DisplayItemInfo(MyItem);
      ItemInfoAreaDisplay.instance.SlackLights();
      ItemInfoAreaDisplay.instance.LightUpRange(MyItem.targetAreaType, MyItem.EffectToTArgetArea + TurnManager.instance.CurrentlyTakingTurn.AvailableActionDictionary[UIManager.instance.PendingActionType].TargetAreaSize);
      parentInventory.SelectASlot(this);
    }

    public void SelectItem()
    {
        TurnManager.instance.Action(UIManager.instance.PendingActionType, MyItem);
        UIManager.instance.CloseInventory();
        parentInventory.UnselectSlots();
    }

    public void SelectSlot()
    {
        slotSelected = true;
        MySlotImage.sprite = SelectedSlot;
    }

    public void UnselectSlot()
    {
        slotSelected = false;
        MySlotImage.sprite = EmptySlot;
    }

}
