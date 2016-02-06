using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveButton : ButtonScript {

    public override void SetUp()
    {
        this.transform.FindChild("Text").GetComponent<Text>().text = ButtonText;
        button = this.GetComponent<Button>();
    }

    void Awake()
    {
        SetUp();
    }

    public override void SelectButton()
    {
        UIManager.instance.CloseInventory();
        TurnManager.instance.Move();
    }

    public override void UpdateButton()
    {
        if (TurnManager.instance.hasMoved)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
