using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {

    public ItemType itemType;
    public string ButtonText;
    public Text CountText;
    public int ItemCount = 3;

    void Awake()
    {
        this.transform.FindChild("Text").GetComponent<Text>().text = ButtonText;
        CountText = this.transform.FindChild("Count").GetComponent<Text>();
        CountText.text = "?";
    }

    public void UpdateButton()
    {
        if (!TurnManager.instance.CurrentlyTakingTurn.CharacterInventory.Contains(itemType))
        {
            this.gameObject.GetComponent<Button>().enabled = false;
        }
        else if(TurnManager.instance.CurrentlyTakingTurn.CharacterInventory.GetItemCount(itemType) < 1)
        {
            this.gameObject.GetComponent<Button>().enabled = false;
            CountText.text = "0";
        }
        else
        {
            CountText.text = TurnManager.instance.CurrentlyTakingTurn.CharacterInventory.GetItemCount(itemType).ToString();
        }
    }

    public void SelectAction()
    {
        TurnManager.instance.Action(itemType);
    }
}
