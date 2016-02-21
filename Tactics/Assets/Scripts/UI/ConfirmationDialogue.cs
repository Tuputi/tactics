using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ConfirmationDialogue : MonoBehaviour{

    public GameObject DialogueTemplate;
    private Text SituationDesc;
    private Text HitChanceText;
    public static ConfirmationDialogue instance;
    private ConfirmationType ConfirmType;
    private Tile ActionTargetTile = null;


    void Start()
    {
        instance = this;
       // DialogueTemplate = Instantiate(PrefabHolder.instance.ConfirmationTemplate);
       // DialogueTemplate.transform.SetParent(GameObject.Find("Canvas").transform,false);
        SituationDesc = DialogueTemplate.transform.FindChild("SituationDesc").GetComponent<Text>();
        HitChanceText = DialogueTemplate.transform.FindChild("HitChangeText").GetComponent<Text>();
        DialogueTemplate.SetActive(false);
    }

    public void Show(ConfirmationType type, Tile target)
    {     
        CameraScript.instance.SetMoveTarget(target.gameObject);
        DialogueTemplate.SetActive(true);
        ConfirmType = type;
        ActionTargetTile = target;
        UIManager.instance.ActivateButtons(false);
        SelectionScript.SetNoSelection(true);

        switch (type)
        {
            case ConfirmationType.action:
                SituationDesc.text = "Target tile/s with " + TurnManager.instance.CurrentlyTakingTurn.currentAction.GetName() + "?";
                HitChanceText.text = "The hit chance is " + TurnManager.instance.CurrentlyTakingTurn.currentAction.GetHitChance(ActionTargetTile).ToString()+"%";
                break;
            case ConfirmationType.move:
                SituationDesc.text = "Move to this tile?";
                HitChanceText.text = "";
                break;
            default:
                break;
        }

    }

    public void Close()
    {
        DialogueTemplate.SetActive(false);
        UIManager.instance.ActivateButtons(true);
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
                    CharacterLogic.instance.CompleteAction(TurnManager.instance.CurrentlyTakingTurn, ActionTargetTile);
                    UIManager.instance.CloseInventory();
                }
                else
                {
                    UIManager.instance.UnselectAllButtons();
                    CameraScript.instance.SetMoveTarget(TurnManager.instance.CurrentlyTakingTurn.gameObject); 
                    TurnManager.instance.CurrentlyTakingTurn.currentAction = null;
                    TurnManager.mode = TurnManager.TurnMode.undecided;   
                    Debug.Log("Action canceled");
                }
                break;
            case ConfirmationType.move:
                if (selected)
                {
                    List<Tile> tempPath = Pathfinding.GetPath(TurnManager.instance.CurrentlyTakingTurn.characterPosition, ActionTargetTile);
                    CharacterLogic.instance.CompleteMove(TurnManager.instance.CurrentlyTakingTurn, tempPath);
                }
                else
                {
                    UIManager.instance.UnselectAllButtons();
                    CameraScript.instance.SetMoveTarget(TurnManager.instance.CurrentlyTakingTurn.gameObject);
                    TurnManager.mode = TurnManager.TurnMode.undecided;
                    Debug.Log("Move canceled");
                }
                break;
            default:
                break;
        }
        Close();
    }

}
