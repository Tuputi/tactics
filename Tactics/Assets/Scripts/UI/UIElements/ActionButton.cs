using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ActionButton : ButtonScript {

    public ActionType actionType;
    public bool OpenInventory = false;

    public override void SetUp()
    {
        this.transform.FindChild("Text").GetComponent<Text>().text = ButtonText;
        button = this.GetComponent<Button>();
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
            return;
        }

        if (OpenInventory)
        {
            UIManager.instance.OpenInventory(actionType);
        }
        else {
            TurnManager.instance.Action(actionType);
        }
       
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
