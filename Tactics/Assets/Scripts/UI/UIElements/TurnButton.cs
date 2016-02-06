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

    public override void SelectButton()
    {
        TurnManager.instance.FacingPhase();
    }

    public override void UpdateButton()
    {
 
    }
}
