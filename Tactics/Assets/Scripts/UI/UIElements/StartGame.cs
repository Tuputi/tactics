using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	void Start()
    {
        UIManager.instance.DisableButtons(false);
    }

    public void SelectAction()
    {
        TurnManager.instance.CreateCharacterList();
        TurnManager.instance.NextInTurn();
        UIManager.instance.DisableButtons(true);
        this.transform.gameObject.SetActive(false);
    }
}
