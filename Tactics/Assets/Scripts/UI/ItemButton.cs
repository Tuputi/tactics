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
        CountText.text = "x"+ItemCount.ToString();
    }

    public void SelectAction()
    {
        ItemCount--;
        CountText.text = "x"+ItemCount.ToString();
        TurnManager.instance.Action(itemType);
    }
}
