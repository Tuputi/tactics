using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemButton : ButtonScript {

    public int itemId;
    public Text CountText;
    public int ItemCount = 3;

    void Awake()
    {
        SetUp();
    }

    public override void SetUp()
    {
        this.transform.FindChild("Text").GetComponent<Text>().text = ButtonText;
        button = this.GetComponent<Button>();
        CountText = this.transform.FindChild("Count").GetComponent<Text>();
        CountText.text = ItemCount.ToString();
        MyImage = this.GetComponent<Image>();
        UnselectButton();
    }

    public override void UpdateButton()
    {
        CountText.text = TurnManager.instance.CurrentlyTakingTurn.CharacterInventory.GetItemCount(itemId).ToString();
        if (TurnManager.instance.hasActed || TurnManager.instance.CurrentlyTakingTurn.CharacterInventory.GetItemCount(itemId) < 1)
        {
            button.interactable = false;
            UnselectButton();
        }
        else
        {
            button.interactable = true;
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

    public override void SelectAction()
    {
        //TurnManager.instance.Action(itemId);
        Debug.Log("This should not be called as items are no longer treated as actions");
        MyImage.sprite = SelectedButton;
        UIManager.instance.UnselectAllActionButtonsExcept(this);

    }
}
