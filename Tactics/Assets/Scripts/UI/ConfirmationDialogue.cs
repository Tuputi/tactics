using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ConfirmationDialogue : MonoBehaviour{

    public GameObject DialogueTemplate;
    private Text SituationDesc;
    public static ConfirmationDialogue instance;
    private ConfirmationType ConfirmType;
    private Tile ActionTargetTile = null;


    void Start()
    {
        instance = this;
        SituationDesc = DialogueTemplate.transform.FindChild("SituationDesc").GetComponent<Text>();
        DialogueTemplate.SetActive(false);
    }

    public void Show(ConfirmationType type, Tile target)
    {
        Debug.Log("Show dialogue");
        switch (type)
        {
            case ConfirmationType.action:
                SituationDesc.text = "Target tile/s with " + TurnManager.instance.CurrentlyTakingTurn.currentAction.GetName() + "?";
                break;
            case ConfirmationType.move:
                SituationDesc.text = "Move to this tile?";
                break;
            default:
                break;
        }
        DialogueTemplate.gameObject.SetActive(true);
        ConfirmType = type;
        ActionTargetTile = target;
    }

    public void Close()
    {
        DialogueTemplate.SetActive(false);
    }

    public void SelectOption(bool selected)
    {
        switch (ConfirmType)
        {
            case ConfirmationType.action:
                if (selected)
                {
                    TurnManager.instance.CurrentlyTakingTurn.CompleteAction(ActionTargetTile);
                }
                else
                {
                    Debug.Log("Action canceled");
                }
                break;
            case ConfirmationType.move:
                if (selected)
                {
                    List<Tile> tempPath = Pathfinding.GetPath(TurnManager.instance.CurrentlyTakingTurn.characterPosition, ActionTargetTile);
                    TurnManager.instance.CurrentlyTakingTurn.CompleteMove(tempPath);
                }
                else
                {
                    Debug.Log("Move canceled");
                }
                break;
            default:
                break;
        }
        Close();
    }
}
