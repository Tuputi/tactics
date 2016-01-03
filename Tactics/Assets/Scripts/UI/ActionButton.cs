using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour {

    public ActionType actionType;
    public string ButtonText;

    void Awake()
    {
        this.transform.FindChild("Text").GetComponent<Text>().text = ButtonText;
    }

    public void SelectAction()
    {
        TurnManager.instance.Action(actionType);
    }


}
