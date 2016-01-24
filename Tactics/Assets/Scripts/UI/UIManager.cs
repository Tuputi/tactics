using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour{

    GameObject ButtonHolder;
    GameObject NextTurnButton;
    GameObject compas;
    public Camera gameCamera;

    //statustemplate
    GameObject StatusTemplate;
    Text hpValue;
    Text hpValueMax;
    Text characterName;

    List<ButtonScript> buttons;
    public static UIManager instance;

    void Awake()
    {
        instance = this;
        buttons = new List<ButtonScript>();
        ButtonHolder = GameObject.Find("Canvas").transform.FindChild("ActionButtons").gameObject;
        NextTurnButton = GameObject.Find("NextTurn");
        for(int i = 0; i < ButtonHolder.transform.childCount; i++)
        {
            buttons.Add(ButtonHolder.transform.GetChild(i).GetComponent<ButtonScript>());
        }
        buttons.Add(NextTurnButton.GetComponent<ButtonScript>());
        StatusTemplate = GameObject.Find("StatusDisplayTemplate");
        hpValue = StatusTemplate.transform.FindChild("HP").transform.FindChild("hpValue").GetComponent<Text>();
        hpValueMax = StatusTemplate.transform.FindChild("HP").transform.FindChild("hpValueMax").GetComponent<Text>();
        characterName = StatusTemplate.transform.FindChild("Name").GetComponent<Text>();

        compas = GameObject.Find("compas");
        compas.SetActive(false);
    }

    public void UpdateStatus(Character chara)
    {
        StatusTemplate.SetActive(true);
        hpValue.text = chara.hp.ToString();
        hpValueMax.text = chara.hpMax.ToString();
        characterName.text = chara.characterName;
    }
    public void UpdateStatus(bool visible)
    {
        StatusTemplate.SetActive(visible);
    }

    public void UpdateButtons()
    {
        if (TurnManager.instance.CurrentlyTakingTurn.isAi)
        {
            ButtonHolder.SetActive(false);
        }
        else
        {
            ButtonHolder.SetActive(true);
            foreach (ButtonScript b in buttons)
            {
                b.UpdateButton();
            }
        }

    }

    public void DisableButtons(bool activeStatus)
    {
       foreach(ButtonScript b in buttons)
       { 
           b.gameObject.GetComponent<Button>().enabled = activeStatus;
       }
    }

    public void SelectFacing(Character chara)
    {
        compas.SetActive(true);
        Vector3 screenPos = gameCamera.WorldToScreenPoint(chara.gameObject.transform.position);
        compas.transform.position = screenPos;
    }

    public void CompleteFacing(int facing)
    {
        CharacterLogic.instance.ChangeFacing(TurnManager.instance.CurrentlyTakingTurn, (Facing)facing);
        Debug.Log("New facing is " + (Facing)facing);
        compas.SetActive(false);
        DisableButtons(true);
        TurnManager.mode = TurnManager.TurnMode.end;
        TurnManager.instance.NextInTurn();
    }
}
