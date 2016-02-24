using UnityEngine;
using UnityEngine.UI;

public class ActionButton : ButtonScript {

    public ActionType actionType;
    public bool OpenInventory = false;

    public override void SetUp()
    {
        this.transform.FindChild("Text").GetComponent<Text>().text = ButtonText;
        button = this.GetComponent<Button>();
        MyImage = this.GetComponent<Image>();
        UnselectButton();
    }

    void Awake()
    {
        //SetUp();
    }

    public override void SelectAction()
    {
        if (UIManager.instance.InventoryOpen)
        {
            UIManager.instance.CloseInventory();
            if (this.Selected)
            {
                SelectionScript.ClearSelection();
                TurnManager.instance.CurrentlyTakingTurn.possibleRange.Clear();
                TurnManager.mode = TurnManager.TurnMode.undecided;
                UnselectButton();
                return;
            }
        }

        if (Selected)
        {
            SelectionScript.ClearSelection();
            TurnManager.instance.CurrentlyTakingTurn.possibleRange.Clear();
            TurnManager.mode = TurnManager.TurnMode.undecided;
            UnselectButton();
            return;
        }

        if (OpenInventory)
        {
            SelectionScript.ClearSelection();
            UIManager.instance.OpenInventory(actionType, TurnManager.instance.CurrentlyTakingTurn.inventoryType);
            SelectButton();
            UIManager.instance.UnselectAllActionButtonsExcept(this);
        }
        else {
            Debug.Log("Open no inventory");
            TurnManager.instance.Action(actionType);
            SelectButton();
            UIManager.instance.UnselectAllActionButtonsExcept(this);
        }

    }

    public override void SelectButton()
    {
        MyImage.sprite = SelectedButton;
        Selected = true;
    }

    public override void UnselectButton()
    {
        MyImage.sprite = UnselectedButton;
        Selected = false;        
    }

    public override void UpdateButton()
    {
        if (TurnManager.instance.hasActed)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
