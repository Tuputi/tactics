using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class IncredientSlot : MonoBehaviour {

    public Sprite EmptySlot;
    public Sprite SelectedSlot;
    public bool slotSelected = false;
    public ItemBase MyItem = null;

    private Image MySlotImage;
    private Image itemSprite;
    private SpellForm parentSpell;

    private List<IncredientSlot> toggleGroup;

    public bool isEmpty = true;

    public void Init(List<IncredientSlot> toggle, SpellForm parent)
    {
        itemSprite = transform.FindChild("ItemSprite").GetComponent<Image>();
        itemSprite.gameObject.SetActive(false);
        MyItem = null;
        isEmpty = true;
        MySlotImage = this.GetComponent<Image>();
        MySlotImage.sprite = EmptySlot;
        toggleGroup = toggle;
        parentSpell = parent;
    }

    public void AddItem(ItemBase item)
    {
        MyItem = item;
        isEmpty = false;

        if (item.ItemSprite == null)
            Debug.Log("no sprite");

        itemSprite.gameObject.SetActive(true);
        itemSprite.sprite = item.ItemSprite;
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ClearSlot()
    {
        Debug.Log("clearslot");
        MyItem = null;
        isEmpty = true;
        itemSprite.gameObject.SetActive(false);
        parentSpell.UpdateSpell();
        //this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SelectItemForDisplay()
    {
        UIManager.instance.DisplayItemInfo(MyItem);
        ItemInfoAreaDisplay.instance.SlackLights();
        ItemInfoAreaDisplay.instance.LightUpRange(TargetAreaType.line, MyItem.EffectToTArgetArea + TurnManager.instance.CurrentlyTakingTurn.AvailableActionDictionary[UIManager.instance.PendingActionType].TargetAreaSize);
        TurnManager.instance.Action(UIManager.instance.PendingActionType, MyItem);
    }

    public void SelectItem()
    {
        TurnManager.instance.Action(UIManager.instance.PendingActionType, MyItem);
        UIManager.instance.CloseInventory();
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

        foreach (IncredientSlot slot in toggleGroup)
        {
            slot.UnselectSlot();
        }
        SelectSlot();
    }
}
