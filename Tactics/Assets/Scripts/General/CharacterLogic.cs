using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterLogic : MonoBehaviour{

   
    public static CharacterLogic instance;
 
    public bool ActionCompleted = true;

    void Awake()
    {
        instance = this;
    }

    //create
    public static void CreateCharacter(int CharaID, Tile tilePos)
    {
        foreach (Character cha in PrefabHolder.instance.characters)
        {
            if (cha.characterID == CharaID)
            {
                Vector3 pos = tilePos.transform.position + cha.transform.position;
                GameObject go = (GameObject)Instantiate(cha.gameObject, pos, Quaternion.identity);

               /* if (!NameCreator.instance.IsNameAlreadyUsed(cha.characterName) || cha.characterNameType == NameType.Animal)
                {
                    go.name = cha.characterName;
                    NameCreator.instance.AddANameToUsed(cha.characterName);
                }
                else
                {*/
                    go.name = NameCreator.instance.GetAName(cha.characterNameType);
                //}
                go.GetComponent<Character>().characterName = go.name;

                GameObject characterHolder = GameObject.Find("Characters");
                go.transform.SetParent(characterHolder.transform);
                tilePos.SetCharacter(go.GetComponent<Character>());
                return;
            }
        }
        if (CharaID != 0)
        {
            Debug.Log("Character Id not found");
        }
    }

    public void ChangeFacing(Character chara, Tile at, Tile to)
    {
        GameObject rotateObj = chara.gameObject;

        if (at.yPos < to.yPos)
        {
            chara.facing = Facing.Up;
            float rotation = 90f;
            rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        if (at.xPos < to.xPos)
        {
            chara.facing = Facing.Left;
            float rotation = 180f;
            rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        if(at.xPos > to.xPos)
        {
            chara.facing = Facing.Right;
            float rotation = 0f;
            rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        if(at.yPos > to.yPos)
        {
            chara.facing = Facing.Down;
            float rotation = 270f;
            rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
   
        chara.characterPosition.SetCharacter(chara);
    }

    public void CreateInventory(Character chara)
    {
        int id = 0;
        chara.CharacterInventory = new Inventory();
        foreach (Item item in chara.items)
        {
            Item newItem = ScriptableObject.CreateInstance<Item>();
            Item template = ItemList.GetItem(item.ItemInstanceID);
            newItem.Init(template.ItemCount, template.Name, template.Sprite, template.ItemMaxStackSize);
            newItem.InitEffect(template.EffectToRange, template.EffectToTArgetArea, template.EffectToDamageStatic, template.EffectToDamageMultiplayer, template.targetAreaType);
            newItem.InitElement(template.addElement);
            newItem.ItemInstanceID = id;
            newItem.itemCategories = template.itemCategories;
            id++;
            chara.CharacterInventory.Add(newItem);
        }
    }

    //mainly for human players
    public void CreateAttackList(Character chara)
    {
        int id = 0;
        chara.AvailableActions = new List<AttackBase>();
        chara.AvailableActionDictionary = new Dictionary<ActionType, AttackBase>();
        foreach (ActionType attack in chara.ActionTypes)
        {
            AttackBase newAttack = ScriptableObject.CreateInstance<AttackBase>();
            AttackBase template = ActionList.GetAction(attack);
            newAttack.Init(template.AttackName, template.minDamage, template.maxDamage, template.MPCost, template.HitChance, template.actionType, template.BasicRange, template.AnimationName);
            newAttack.InitCompatibleItems(template.compatibleItems, template.UsedWithItems);
            newAttack.InitElements(template.ElementalAttributes);
            newAttack.ActionID = id;
            id++;
            chara.AvailableActions.Add(newAttack);
            chara.AvailableActionDictionary.Add(attack, newAttack);
        }
    }

    //mainly for ai
    public void CreateAttackList(Character chara, List<ActionType> actionTypeList)
    {
        int id = 0;
        chara.AvailableActions = new List<AttackBase>();
        foreach (ActionType attack in actionTypeList)
        {
            AttackBase newAttack = ScriptableObject.CreateInstance<AttackBase>();
            AttackBase template = ActionList.GetAction(attack);
            newAttack.Init(template.AttackName, template.minDamage, template.maxDamage, template.MPCost, template.HitChance, template.actionType, template.BasicRange, template.AnimationName);
            newAttack.InitCompatibleItems(template.compatibleItems, template.UsedWithItems);
            newAttack.InitElements(template.ElementalAttributes);
            newAttack.ActionID = id;
            id++;
            chara.AvailableActions.Add(newAttack);
            chara.AvailableActionDictionary.Add(attack, newAttack);
        }
    }

    private void SelectMultipleTiles(List<Tile> tiles)
    {
        SelectionScript.selectMultiple = true;
        foreach (Tile t in tiles)
        {
            t.SelectThis();
        }
        SelectionScript.selectMultiple = false;
    }

    //actions
    public void Move(Character chara)
    {
        chara.possibleRange = Pathfinding.GetPossibleRange(chara.characterPosition, chara.movementRange, false);
        List<Tile> tempList = new List<Tile>();
        foreach(Tile t in chara.possibleRange)
        {
            if (t.isOccupied) //remove all tiles with other characters
            {
               tempList.Add(t);
            }
        }
        foreach(Tile t in tempList)
        {
            chara.possibleRange.Remove(t);
        }
        SelectMultipleTiles(chara.possibleRange);
    }

    public void CompleteMove(Character chara, List<Tile> path)
    {
        chara.MoveCharacter(chara, path);
        SelectionScript.ClearSelection();
        chara.possibleRange.Clear();
        TurnManager.mode = TurnManager.TurnMode.end;
        TurnManager.instance.hasMoved = true;
        TurnOrderTimeline.MoveATimeCard(chara);
        UIManager.instance.UpdateButtons();
    }

    public void Action(Character chara, ActionBaseClass ab)
    {
        chara.currentAction = ab;
        chara.possibleRange = ab.CalculateActionRange(chara.characterPosition);
        SelectMultipleTiles(chara.possibleRange);
    }

    public void Action(Character chara, ActionBaseClass ab, Item ib)
    {
        chara.currentAction = ab;
        chara.currentItem = ib;
        chara.possibleRange = ab.CalculateActionRange(chara.characterPosition);
        SelectMultipleTiles(chara.possibleRange);
    }

    public void CompleteAction(Character chara, Tile tile)
    {
        if (chara.currentItem != null)
        {
            chara.CharacterInventory.Use(chara.currentItem.ItemInstanceID);
        }

        chara.possibleRange.Clear();
        SelectionScript.ClearSelection();

        ChangeFacing(chara, chara.characterPosition, tile);
        UIManager.instance.ShowAttackName(chara.currentAction.GetName());
        chara.PlayAttackanimation(chara.currentAction.AnimationName);
        if (chara.isAi)
        {
            chara.GetComponent<AiModule>().WaitingToEndTurn = true;
        }
        chara.characterEnergy -= chara.currentAction.EnergyCost;
        chara.targetTile = tile;
        TurnManager.mode = TurnManager.TurnMode.end;
        TurnManager.instance.hasActed = true;
        TurnOrderTimeline.MoveATimeCard(chara);
        UIManager.instance.UpdateButtons();
    }

    public void DisplayEffect(Character targetChara, int damageAmount, DisplayTexts displayText)
    {
        targetChara.PlayHurtAnimation();

        GameObject damageText = (GameObject)Instantiate(PrefabHolder.instance.DamageText);
        if (damageAmount > 0)
        {
            damageText.GetComponentInChildren<UnityEngine.UI.Text>().color = Color.green;
        }


        switch (displayText)
        {
            case DisplayTexts.none:
                damageText.GetComponentInChildren<UnityEngine.UI.Text>().text = damageAmount.ToString();
                break;
            case DisplayTexts.miss:
                damageText.GetComponentInChildren<UnityEngine.UI.Text>().text = "miss";
                break;
            case DisplayTexts.immune:
                damageText.GetComponentInChildren<UnityEngine.UI.Text>().text = "immune";
                break;
            default:
                break;
        }

        damageText.transform.SetParent(targetChara.gameObject.transform);
        damageText.transform.localPosition = new Vector3(0, targetChara.inturnmarkerheight, 0);
        UIManager.instance.UpdateStatusWindow(targetChara);
    }

    public void SetCharacterPosition(Character chara, Tile tile)
    {
        tile.SetCharacter(chara);
    }

   
    
}
