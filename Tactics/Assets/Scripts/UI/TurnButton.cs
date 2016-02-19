using UnityEngine;
using UnityEngine.UI;

public class TurnButton : ButtonScript {

    public override void SetUp()
    {

        button = this.GetComponent<Button>();
        MyImage = this.GetComponent<Image>();
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
            UnselectButton();
            return;
        }

        SelectionScript.ClearSelection();
        TurnManager.instance.FacingPhase();
    }

    public override void SelectButton()
    {
        MyImage.sprite = SelectedButton;
    }

    public override void UnselectButton()
    {
        MyImage.sprite = UnselectedButton;
    }

    public override void UpdateButton()
    {
 
    }
}
