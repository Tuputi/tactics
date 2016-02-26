﻿using UnityEngine;
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

    public void Init(List<IncredientSlot> toggle)
    {
        itemSprite = transform.FindChild("ItemSprite").GetComponent<Image>();
        itemSprite.gameObject.SetActive(false);
        MyItem = null;
        isEmpty = true;
        MySlotImage = this.GetComponent<Image>();
        MySlotImage.sprite = EmptySlot;
        toggleGroup = toggle;
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
        MyItem = null;
        isEmpty = true;
        itemSprite.gameObject.SetActive(false);
        this.transform.localScale = new Vector3(1, 1, 1);
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
        }
        else
        {
            SelectSlot();
        }
    }

    public void SelectButton()
    {    
        foreach (IncredientSlot slot in toggleGroup)
        {
            slot.UnselectSlot();
        }
        SelectSlot();
    }
}
