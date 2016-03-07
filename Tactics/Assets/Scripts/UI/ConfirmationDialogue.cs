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
    bool waitingAnimationToFinish;
    float templateHeight;


    void Start()
    {
        instance = this;
        SituationDesc = DialogueTemplate.transform.FindChild("SituationDesc").GetComponent<Text>();
        templateHeight = DialogueTemplate.GetComponentInChildren<Image>().GetComponent<RectTransform>().rect.height;
        if(Application.platform == RuntimePlatform.Android)
        {
            templateHeight *= 3;
        }
        DialogueTemplate.SetActive(false);
    }

    void Update(){
        if (waitingAnimationToFinish)
        {
            if (!CameraScript.instance.MoveToTargetActive)
            {
                Show();
                waitingAnimationToFinish = false;
            }
        }
    }

    public void Show(ConfirmationType type, Tile target)
    {     
        CameraScript.instance.SetMoveTarget(target.gameObject);
        waitingAnimationToFinish = true;
        ConfirmType = type;
        ActionTargetTile = target;
        UIManager.instance.ActivateButtons(false);
        UIManager.instance.NextTurnButton.GetComponent<Button>().interactable = false;

        SelectionScript.SetNoSelection(true);
    }

    private void Show()
    {
        Vector3 position = UIManager.instance.ConvertPositionToScreenPoint(ActionTargetTile.gameObject);
        if (position.y + 200 > Screen.height)
        {
            position += new Vector3(0, -200, 0);
        }
        else if((position.y - 200 < 0))
        {
            position += new Vector3(0,+200,0);
        }
 
        DialogueTemplate.transform.position = position;
        DialogueTemplate.SetActive(true);

        switch (ConfirmType)
        {
            case ConfirmationType.action:
                SituationDesc.text = "Target tile(s) with " + TurnManager.instance.CurrentlyTakingTurn.currentAction.GetName() + "?";
               // HitChanceText.text = "The hit chance is " + TurnManager.instance.CurrentlyTakingTurn.currentAction.GetHitChance(ActionTargetTile).ToString() + "%";
                break;
            case ConfirmationType.move:
                SituationDesc.text = "Move to this tile?";
               // HitChanceText.text = "";
                break;
            default:
                break;
        }

    }

    public void Close()
    {
        DialogueTemplate.SetActive(false);
        UIManager.instance.UpdateButtons();
        UIManager.instance.NextTurnButton.GetComponent<Button>().interactable = true;
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
                    UIManager.instance.CloseInventoryAfterCompletedAttack();
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
