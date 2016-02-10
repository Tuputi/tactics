using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	void Start()
    {
        UIManager.instance.ActivateButtons(false);
    }

    public void SelectAction()
    {
        TurnManager.instance.CreateCharacterList();
        TurnManager.instance.FindNextInTurn();
        UIManager.instance.ActivateButtons(true);
        this.transform.gameObject.SetActive(false);
    }
}
