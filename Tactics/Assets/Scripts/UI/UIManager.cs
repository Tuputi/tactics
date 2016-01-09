using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager{

    GameObject ButtonHolder;
    static List<ButtonScript> buttons;

    void Awake()
    {
        buttons = new List<ButtonScript>();
        ButtonHolder = GameObject.Find("ActionButtons");
        for(int i = 0; i < ButtonHolder.transform.childCount; i++)
        {
            buttons.Add(ButtonHolder.transform.GetChild(i).GetComponent<ButtonScript>());
        }
        Debug.Log("Buttoncount " + buttons.Count);
    }

    public static void UpdateButtons()
    {
        foreach(ButtonScript b in buttons)
        {
            b.UpdateButton();
        }
    }
}
