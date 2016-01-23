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
        CameraScript.instance.SetMoveTarget(target.gameObject);
        DialogueTemplate.gameObject.SetActive(true);
        ConfirmType = type;
        ActionTargetTile = target;
        UIManager.instance.DisableButtons(false);
        SelectionScript.SetNoSelection(true);
    }

    public void Close()
    {
        DialogueTemplate.SetActive(false);
        UIManager.instance.DisableButtons(true);
        SelectionScript.SetNoSelection(false);
    }

    public void SelectOption(bool selected)
    {
        SelectionScript.ClearSelection();
        switch (ConfirmType)
        {
            case ConfirmationType.action:
                if (selected)
                {
                   // TurnManager.instance.CurrentlyTakingTurn.CompleteAction(ActionTargetTile);
                    CharacterLogic.instance.CompleteAction(TurnManager.instance.CurrentlyTakingTurn, ActionTargetTile);
                }
                else
                {
                    CameraScript.instance.SetMoveTarget(TurnManager.instance.CurrentlyTakingTurn.gameObject);
                    // TurnManager.instance.CurrentlyTakingTurn.Action(TurnManager.instance.CurrentlyTakingTurn.currentAction);    
                    CharacterLogic.instance.Action(TurnManager.instance.CurrentlyTakingTurn, TurnManager.instance.CurrentlyTakingTurn.currentAction);    
                    Debug.Log("Action canceled");
                }
                break;
            case ConfirmationType.move:
                if (selected)
                {
                    List<Tile> tempPath = Pathfinding.GetPath(TurnManager.instance.CurrentlyTakingTurn.characterPosition, ActionTargetTile);
                    //TurnManager.instance.CurrentlyTakingTurn.CompleteMove(tempPath);
                    CharacterLogic.instance.CompleteMove(TurnManager.instance.CurrentlyTakingTurn, tempPath);
                }
                else
                {
                    CameraScript.instance.SetMoveTarget(TurnManager.instance.CurrentlyTakingTurn.gameObject);
                    //TurnManager.instance.CurrentlyTakingTurn.Move();
                    CharacterLogic.instance.Move(TurnManager.instance.CurrentlyTakingTurn);
                    Debug.Log("Move canceled");
                }
                break;
            default:
                break;
        }
        Close();
    }

}
