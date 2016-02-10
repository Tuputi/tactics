using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour{

    public GameObject ButtonHolder;
    UIInventory UIInventory;
    GameObject NextTurnButton;
    public GameObject ActionButtonBase;
    public GameObject BasicButtonBase;
    public Camera gameCamera;

    //item info
    public GameObject ItemInfoHolder;
    public Text ItemName;
    public Text ItemEffectOnRange;
    public Text ItemEffectOnAttackArea;


    //statustemplate
    GameObject StatusTemplate;
    Text hpValue;
    Text hpValueMax;
    Text characterName;

    List<ButtonScript> buttons;
    public static UIManager instance;

    //memory
    public ActionType PendingActionType;
    public bool InventoryOpen = false;

    void Awake()
    {
        instance = this;
        buttons = new List<ButtonScript>();
       // ButtonHolder = GameObject.Find("Canvas").transform.FindChild("ActionButtons").gameObject;
        NextTurnButton = GameObject.Find("NextTurn");
        
        buttons.Add(NextTurnButton.GetComponent<ButtonScript>());
        StatusTemplate = GameObject.Find("StatusDisplayTemplate");
        hpValue = StatusTemplate.transform.FindChild("HP").transform.FindChild("hpValue").GetComponent<Text>();
        hpValueMax = StatusTemplate.transform.FindChild("HP").transform.FindChild("hpValueMax").GetComponent<Text>();
        characterName = StatusTemplate.transform.FindChild("Name").GetComponent<Text>();
        UIInventory = GameObject.Find("Inventory").GetComponent<UIInventory>();
        UIInventory.gameObject.SetActive(false);

    }

    //code by mwk888
    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void UpdateStatus(Character chara)
    {
        StatusTemplate.SetActive(true);
        hpValue.text = chara.Hp.ToString();
        hpValueMax.text = chara.hpMax.ToString();
        characterName.text = chara.characterName;
    }
    public void UpdateStatus(bool visible)
    {
        StatusTemplate.SetActive(visible);
    }

    public void UpdateButtons()
    {
        if (TurnManager.instance.CurrentlyTakingTurn.isAi)
        {
            ButtonHolder.SetActive(false);
        }
        else
        {
            ButtonHolder.SetActive(true);
            foreach (ButtonScript b in buttons)
            {
                b.UpdateButton();
            }
        }

    }

    public void DisableButtons(bool activeStatus)
    {
       foreach(ButtonScript b in buttons)
       { 
           b.gameObject.GetComponent<Button>().enabled = activeStatus;
       }
    }

    public void SelectFacing(Character chara)
    {
        //compas.SetActive(true);
        List<Tile> places = chara.characterPosition.neighbours;
        foreach(Tile t in places)
        {
            t.SetOverlayType(OverlayType.Arrow);
            t.GetComponentInChildren<OverlayArrow>().RotateArrow();
        }
        SelectionScript.SetNoSelection(true);
        //Vector3 screenPos = gameCamera.WorldToScreenPoint(chara.gameObject.transform.position);
        //compas.transform.position = screenPos;
    }

    public void CompleteFacing(Tile myTile)
    {
        CharacterLogic.instance.ChangeFacing(TurnManager.instance.CurrentlyTakingTurn, TurnManager.instance.CurrentlyTakingTurn.characterPosition, myTile);
        DisableButtons(true);
        SelectionScript.SetNoSelection(false);
        foreach(Tile t in TurnManager.instance.CurrentlyTakingTurn.characterPosition.neighbours)
        {
            t.SetOverlayType(OverlayType.None);
        }
        TurnManager.mode = TurnManager.TurnMode.end;
        TurnManager.instance.FindNextInTurn();
    }


    public void OpenInventory(ActionType at)
    {
        if (InventoryOpen)
        {
            CloseInventory();
        }
        InventoryOpen = true;
        PendingActionType = at;
        AttackBase abc = PrefabHolder.instance.actionDictionary[at];
        UIInventory.gameObject.SetActive(true);
       foreach(ItemBase item in TurnManager.instance.CurrentlyTakingTurn.CharacterInventory.GetWholeInventory())
       {
            if (abc.CompatibleItem(item))
            {
                UIInventory.GetComponent<UIInventory>().AddItem(item);
            }
        }
    }

    public void CloseInventory()
    {
        InventoryOpen = false;
        CloseItemInfo();
        UIInventory.CloseInventory();
    }

    public void DisplayItemInfo(ItemBase item)
    {
        ItemInfoHolder.SetActive(true);
        ItemName.text = item.ItemName;
        if(item.EffectToRange < 0)
        {
            ItemEffectOnRange.text = "Range: " + item.EffectToRange.ToString();
        }
        else
        {
            ItemEffectOnRange.text = "Range: +" + item.EffectToRange.ToString();
        }

        if(item.EffectToTArgetArea < 0)
        {
            ItemEffectOnAttackArea.text = "Attack Area: " + item.EffectToTArgetArea.ToString();
        }
        else
        {
            ItemEffectOnAttackArea.text = "Attack Area: +" + item.EffectToTArgetArea.ToString();
        }
    }

    public void CloseItemInfo()
    {
        ItemInfoHolder.SetActive(false);
    }

    public GameObject TurnorderHolder;
    public Text turnorderCharaName;
    public void UpdateTurnOrderDisplay(List<Character> characters)
    {
        for(int i = 0; i < TurnorderHolder.transform.childCount; i++)
        {
            Destroy(TurnorderHolder.transform.GetChild(i).gameObject);
        }
            float currentHeight = 0;
        foreach(Character chara in characters){
            Text tempText = Instantiate(turnorderCharaName) as Text;
            tempText.text = chara.characterName +" - "+ chara.characterEnergy.ToString();
            tempText.transform.SetParent(TurnorderHolder.transform, false);
           // tempText.transform.localScale = new Vector3(1, 1, 1);
            tempText.transform.localPosition = new Vector3(0, currentHeight, 0);
            currentHeight += -(turnorderCharaName.transform.GetComponent<RectTransform>().rect.height);
        }
    }

    public void CreateActionButton(List<AttackBase> actions)
    {
        List<GameObject> tempButtons = new List<GameObject>();
        foreach (ButtonScript go in PrefabHolder.instance.basicActions)
        {
            ButtonScript newButton = Instantiate(go);
            newButton.gameObject.transform.localScale = new Vector3(1, 1, 1);
            newButton.SetUp();
            tempButtons.Add(newButton.gameObject);
        }
        foreach (AttackBase ab in actions)
        {
            GameObject newButton = Instantiate(ActionButtonBase);
            newButton.transform.localScale = new Vector3(1, 1, 1);
            newButton.GetComponent<ActionButton>().ButtonText = ab.GetName();
            newButton.GetComponent<ActionButton>().actionType = ab.GetActionType();
            newButton.GetComponent<ActionButton>().SetUp();
            newButton.GetComponent<ActionButton>().OpenInventory = ab.UsedWithItems;
            tempButtons.Add(newButton);
        }

        buttons.Clear();
        buttons.Add(NextTurnButton.GetComponent<ButtonScript>());
        foreach (GameObject b in tempButtons)
        {
            buttons.Add(b.GetComponent<ButtonScript>());
        }
        RotatinMenu.instance.AddActionButtons(tempButtons);
    }
}
