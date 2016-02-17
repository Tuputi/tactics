using UnityEngine;
using UnityEngine.UI;

public class TurnButton : ButtonScript {

    public override void SetUp()
    {
        this.transform.FindChild("Text").GetComponent<Text>().text = ButtonText;
        button = this.GetComponent<Button>();
    }

    void Awake()
    {
        SetUp();
    }

    public override void SelectAction()
    {
        if(TurnManager.mode == TurnManager.TurnMode.facing)
        {
            TurnManager.mode = TurnManager.TurnMode.undecided;
            foreach(Tile t in TurnManager.instance.CurrentlyTakingTurn.characterPosition.neighbours)
            {
                t.SetOverlayType(OverlayType.None);
            }
            SelectionScript.SetNoSelection(false);
            UIManager.instance.ActivateButtons(true);
            return;
        }

        SelectionScript.ClearSelection();
        TurnManager.instance.FacingPhase();
    }

    public override void UpdateButton()
    {
 
    }
}
