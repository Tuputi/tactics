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
                go.name = cha.characterName;
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
        int rowChange = System.Math.Abs(at.xPos - to.xPos);
        int columnChange = System.Math.Abs(at.yPos - to.yPos);

        GameObject rotateObj = chara.gameObject;

        if (rowChange > columnChange)
        {
            if (at.xPos < to.xPos)
            {
                chara.facing = Facing.Left;
                float rotation = 180f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
                chara.facing = Facing.Right;
                float rotation = 0f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
        }
        else
        {
            if (at.yPos > to.yPos)
            {
                chara.facing = Facing.Down;
                float rotation = 270f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
                chara.facing = Facing.Up;
                float rotation = 90f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
        }
        chara.characterPosition.SetCharacter(chara);
    }

    public void CreateInventory(Character chara)
    {
        int id = 0;
        chara.CharacterInventory = new Inventory();
        foreach (ItemType item in chara.items)
        {
            ItemBase newItem = ScriptableObject.CreateInstance<ItemBase>();
            ItemBase template = ItemList.GetItem(item);
            newItem.Init(template.ItemCount, template.ItemName, template.itemType, template.ItemSprite, template.ItemMaxStackSize);
            newItem.InitEffect(template.EffectToRange, template.EffectToTArgetArea, template.EffectToDamageStatic, template.EffectToDamageMultiplayer);
            newItem.ItemId = id;
            newItem.itemCategories = template.itemCategories;
            id++;
            chara.CharacterInventory.Add(newItem);
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
        SelectMultipleTiles(chara.possibleRange);
    }

    public void CompleteMove(Character chara, List<Tile> path)
    {
        chara.MoveCharacter(chara, path);
        SelectionScript.ClearSelection();
        chara.possibleRange.Clear();
        TurnManager.mode = TurnManager.TurnMode.end;
        TurnManager.instance.hasMoved = true;
        UIManager.instance.UpdateButtons();
    }

    public void Action(Character chara, ActionBaseClass ab)
    {
        chara.currentAction = ab;
        chara.possibleRange = ab.CalculateActionRange(chara.characterPosition);
        SelectionScript.selectMultiple = true;
        foreach (Tile t in chara.possibleRange)
        {
            t.SelectThis();
        }
        SelectionScript.selectMultiple = false;
    }

    public void Action(Character chara, ActionBaseClass ab, ItemBase ib)
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
            chara.CharacterInventory.Use(chara.currentItem.ItemId);
        }
        chara.possibleRange.Clear();
        SelectionScript.ClearSelection();

        ChangeFacing(chara, chara.characterPosition, tile);

        int random = Random.Range(1, 100);
        chara.PlayAttackanimation(chara.currentAction.AnimationName);
        chara.characterEnergy -= chara.currentAction.EnergyCost;
        if (random <= chara.currentAction.GetHitChance(tile))
        {
            chara.targetTile = tile;
        }
        else
        {
            Debug.Log("Miss");
        }
        TurnManager.mode = TurnManager.TurnMode.end;
        TurnManager.instance.hasActed = true;  
        UIManager.instance.UpdateButtons();
    }

 

    public void DisplayEffect(Character chara, int damageAmount)
    {
        GameObject damageText = (GameObject)Instantiate(PrefabHolder.instance.DamageText);   
        if(damageAmount > 0)
        {
            damageText.GetComponentInChildren<UnityEngine.UI.Text>().color = Color.green;
        }

        if (damageAmount == 0)
        {
            damageText.GetComponentInChildren<UnityEngine.UI.Text>().text = "miss";
        }
        else {
            damageText.GetComponentInChildren<UnityEngine.UI.Text>().text = damageAmount.ToString();
        }
        damageText.transform.SetParent(chara.gameObject.transform);
        damageText.transform.localPosition = new Vector3(0, 1f, 0);
        UIManager.instance.UpdateStatus(chara);
    }

    public void SetCharacterPosition(Character chara, Tile tile)
    {
        tile.SetCharacter(chara);
    }

   
    
}
