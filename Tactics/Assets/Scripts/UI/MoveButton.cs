using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MoveButton : ButtonScript {

    public override void SetUp()
    {
        this.transform.FindChild("Text").GetComponent<Text>().text = ButtonText;
        button = this.GetComponent<Button>();
        MyImage = this.GetComponent<Image>();
        UnselectButton();
    }

    void Awake()
    {
        SetUp();
    }

    public override void SelectAction()
    {
        if (Selected)
        {
            SelectionScript.ClearSelection();
            TurnManager.instance.CurrentlyTakingTurn.possibleRange.Clear();
            TurnManager.mode = TurnManager.TurnMode.undecided;
            UnselectButton();
            return;
        }
        UIManager.instance.CloseInventory();
        SelectButton();
        UIManager.instance.UnselectAllActionButtonsExcept(this);
        TurnManager.instance.Move();
    }

    public override void UnselectButton()
    {
        MyImage.sprite = UnselectedButton;
        Selected = false;
    }

    public override void SelectButton()
    {
        MyImage.sprite = SelectedButton;
        Selected = true;
    }

    public override void UpdateButton()
    {
        if (TurnManager.instance.hasMoved)
        {
            button.interactable = false;
            MyImage.sprite = UnselectedButton;
        }
        else
        {
            button.interactable = true;
        }
    }
}
