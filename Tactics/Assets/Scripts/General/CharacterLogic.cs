using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterLogic : MonoBehaviour{

    int DistanceToGo = 0;
    public float TravelSpeed = 0.2f;
    public static CharacterLogic instance;

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
            if (at.yPos < to.yPos)
            {
                chara.facing = Facing.Down;
                float rotation = 90f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
                chara.facing = Facing.Up;
                float rotation = 270f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
        }
        chara.characterPosition.SetCharacter(chara);
    }

    public void CreateInventory(Character chara)
    {
        chara.CharacterInventory = new Inventory();
        foreach (ItemBase item in chara.items)
        {
            ItemBase instanceItem = (ItemBase)ScriptableObject.CreateInstance(item.name);
            chara.CharacterInventory.Add(instanceItem);
        }
    }

    //actions
    public void Move(Character chara)
    {
        chara.possibleRange = Pathfinding.GetPossibleRange(chara.characterPosition, chara.movementRange, false);
        foreach (Tile t in chara.possibleRange)
        {
            t.SetOverlayType(OverlayType.Selected);
        }
    }

    public void CompleteMove(Character chara, List<Tile> path)
    {
        MoveCharacter(chara, path);
        foreach (Tile t in chara.possibleRange)
        {
            t.SetOverlayType(OverlayType.None);
        }
        chara.possibleRange.Clear();
        TurnManager.mode = TurnManager.TurnMode.end;
        TurnManager.instance.hasMoved = true;
        UIManager.instance.UpdateButtons();
    }

    public void Action(Character chara, ActionBaseClass ab)
    {
        chara.currentAction = ab;
        chara.possibleRange = ab.CalculateActionRange(chara.characterPosition);
        foreach (Tile t in chara.possibleRange)
        {
            t.SetOverlayType(OverlayType.Selected);
        }
    }

    public void CompleteAction(Character chara, Tile tile)
    {
        foreach (Tile t in chara.possibleRange)
        {
            t.SetOverlayType(OverlayType.None);
        }
        chara.possibleRange.Clear();
        SelectionScript.ClearSelection();
        chara.currentAction.CompleteAction(tile);
        TurnManager.mode = TurnManager.TurnMode.end;
        TurnManager.instance.hasActed = true;
        UIManager.instance.UpdateButtons();
    }

    public void TakeDamage(Character chara, int damageAmount)
    {
        GameObject damageText = (GameObject)Instantiate(PrefabHolder.instance.DamageText);
        damageText.GetComponentInChildren<UnityEngine.UI.Text>().text = damageAmount.ToString();
        damageText.transform.SetParent(chara.gameObject.transform);
        damageText.transform.localPosition = new Vector3(0, 1f, 0);
    }

    public void SetCharacterPosition(Character chara, Tile tile)
    {
        tile.SetCharacter(chara);
    }

    public void MoveCharacter(Character chara, List<Tile> path)
    {
        SetCharacterPosition(chara, path[0]);
        DistanceToGo = path.Count - 1;
        StartCoroutine(MovePath(chara, path));
    }

    IEnumerator MovePath(Character chara, List<Tile> path)
    {
        while (DistanceToGo >= 0)
        {
            Tile target = path[DistanceToGo];
            Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y + 1.2f, target.transform.position.z);
            Vector3 sourcePos = chara.gameObject.transform.position;
            chara.transform.position = Vector3.MoveTowards(sourcePos, targetPos, Mathf.SmoothStep(0, 1f, TravelSpeed));
            if (chara.transform.position == targetPos)
            {
                DistanceToGo--;
            }
            yield return 0;
        }
    }
}
