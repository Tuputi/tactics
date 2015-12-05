using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapEditorUI : MonoBehaviour {


    GameObject buttonContainer;
	// Use this for initialization
	void Start () {
        buttonContainer = GameObject.Find("SelectTileTypeMenu").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
    }


    public void TileDropDownButton()
    {
        
        GameObject buttons = buttonContainer.transform.FindChild("Buttons").gameObject;
        buttons.SetActive(!buttons.activeSelf);

    }

    public void SetTileTypeText(string buttonName)
    {
        Text text = buttonContainer.transform.FindChild("Text").GetComponent<Text>();
        text.text = buttonName;
    }

    public void Drop()
    {

    }

}
