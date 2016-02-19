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
    public Toggle rotationToggle;
    public Toggle tiltToggle;
    public GameObject InTurnMarker;
    private GameObject MyInTurnMarker;

    //item info
    public GameObject ItemInfoHolder;
    public Text ItemName;
    public Image ItemImage;
    public Text ItemEffectOnRange;


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
        NextTurnButton = GameObject.Find("NextTurn");
        StatusTemplate = GameObject.Find("StatusDisplayTemplate");
        hpValue = StatusTemplate.transform.FindChild("HP").transform.FindChild("hpValue").GetComponent<Text>();
        hpValueMax = StatusTemplate.transform.FindChild("HP").transform.FindChild("hpValueMax").GetComponent<Text>();
        characterName = StatusTemplate.transform.FindChild("Name").GetComponent<Text>();
        UIInventory = GameObject.Find("Inventory").GetComponent<UIInventory>();
        UIInventory.gameObject.SetActive(false);
        MyInTurnMarker = Instantiate(InTurnMarker);

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

    public void UpdateStatusWindow(Character chara)
    {
        StatusTemplate.SetActive(true);
        hpValue.text = chara.Hp.ToString();
        hpValueMax.text = chara.hpMax.ToString();
        characterName.text = chara.characterName;
    }
    public void SetStatusWindowActiveStatus(bool visible)
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

    public void ActivateButtons(bool activeStatus)
    {
       foreach(ButtonScript b in buttons)
       { 
           b.gameObject.GetComponent<Button>().enabled = activeStatus;
       }
    }

    public void UnselectAllActionButtonsExcept(ButtonScript button)
    {
        foreach(ButtonScript bs in RotatingMenu.instance.GetAllActiveButtons())
        {
            bs.UnselectButton();
        }
        button.SelectButton();
    }

    public void StartSelectFacingPhase(Character chara)
    {
        List<Tile> places = chara.characterPosition.neighbours;
        foreach(Tile t in places)
        {
            t.SetOverlayType(OverlayType.Arrow);
            t.GetComponentInChildren<OverlayArrow>().RotateArrow();
        }
        SelectionScript.SetNoSelection(true);
    }

    public void CompleteFacing(Tile myTile)
    {
        CharacterLogic.instance.ChangeFacing(TurnManager.instance.CurrentlyTakingTurn, TurnManager.instance.CurrentlyTakingTurn.characterPosition, myTile);
        ActivateButtons(true);
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
        ItemImage.sprite = item.ItemSprite;
        if(item.EffectToRange < 0)
        {
            ItemEffectOnRange.text = "Range: " + item.EffectToRange.ToString();
        }
        else
        {
            ItemEffectOnRange.text = "Range: +" + item.EffectToRange.ToString();
        }

        ItemInfoAreaDisplay.instance.LightUpRange(TargetAreaType.line, item.EffectToTArgetArea);
    }

    public void CloseItemInfo()
    {
        ItemInfoHolder.SetActive(false);
    }

    public void SelectItemFromItemInfo()
    {
        foreach(GameObject invSlot in UIInventory.InventorySlots)
        {
            InventorySlot iS = invSlot.GetComponent<InventorySlot>();
            if (iS.slotSelected)
            {
                iS.SelectItem();
            }
        }
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
       // buttons.Add(NextTurnButton.GetComponent<ButtonScript>());
        foreach (GameObject b in tempButtons)
        {
            buttons.Add(b.GetComponent<ButtonScript>());
        }
        RotatingMenu.instance.AddActionButtons(tempButtons);
    }

    public void AssignInTurnMaker(Character currentCharacter)
    {
        MyInTurnMarker.gameObject.transform.SetParent(currentCharacter.gameObject.transform);
        MyInTurnMarker.gameObject.transform.localPosition = new Vector3(0, 13f, 0);
    }

    public void HideInTurnMarker()
    {
        MyInTurnMarker.SetActive(false);
    }

    public void ChangePanelVisibility(GameObject panel)
    {
        Animator anim = panel.GetComponent<Animator>();
        bool visibility = anim.GetBool("Visible");
        anim.SetBool("Visible", !visibility);
    }

}
