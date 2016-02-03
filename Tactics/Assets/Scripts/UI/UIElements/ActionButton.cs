using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ActionButton : ButtonScript {

    public ActionType actionType;

    public override void SetUp()
    {
        this.transform.FindChild("Text").GetComponent<Text>().text = ButtonText;
        button = this.GetComponent<Button>();
    }

    void Awake()
    {
        SetUp();
    }

    public override void SelectAction()
    {
        //TurnManager.instance.Action(actionType);
        UIManager.instance.OpenInventory(actionType);
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
