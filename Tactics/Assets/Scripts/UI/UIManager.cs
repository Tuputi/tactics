using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour{

    GameObject ButtonHolder;
    List<ButtonScript> buttons;
    public static UIManager instance;

    void Awake()
    {
        instance = this;
        buttons = new List<ButtonScript>();
        ButtonHolder = GameObject.Find("Canvas").transform.FindChild("ActionButtons").gameObject;
        for(int i = 0; i < ButtonHolder.transform.childCount; i++)
        {
            buttons.Add(ButtonHolder.transform.GetChild(i).GetComponent<ButtonScript>());
        }
        Debug.Log("Buttoncount " + buttons.Count);
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
}
