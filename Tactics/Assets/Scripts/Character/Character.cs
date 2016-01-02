using UnityEngine;
using System.Collections.Generic;
using BonaJson;
using System.Collections;

public class Character : MonoBehaviour, System.IComparable
{

    public string characterName = "tempName";
    public int characterID = 0;
    public Tile characterPosition;

    //game logic stats
    public float characterEnergy = 5f;
    public int characterWalkEnergy = 5;

    //logic
    public bool isAlive
    {
        get
        {
            return (hp > 0);
        }
    }

    //Movement
    int DistanceToGo = 0;
    public float TravelSpeed = 0.2f;

    //stats
    public int hp = 100;
    public int mp = 100;
    public int speed = 10;

    //lists
    public List<Tile> possibleRange;

    //compartors
    public bool Equals(Character other)
    {
        if (other.characterName.Equals(this.characterName))
        {
           return true;
        }
        return false;
    }

    public int CompareTo(object other)
    {
        Character otherCharacter = (Character)other;
        if (otherCharacter != null)
        {
            if (otherCharacter.characterEnergy < this.characterEnergy)
            {
                return -1;
            }
            else if (otherCharacter.characterEnergy > this.characterEnergy)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            throw new System.ArgumentException("Object is not a Character");
        }
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
                //have reference to characterHolder somewhere
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


    //actions
    public void Move()
    {
        possibleRange = Pathfinding.GetPossibleRange(characterPosition, characterWalkEnergy, false);
        foreach (Tile t in possibleRange)
        {
            t.SetOverlayType(OverlayType.Selected);
        }
    }

    public void CompleteMove(Tile tile)
    {
        Debug.Log("move to "+ tile);
        List<Tile> foundPath = Pathfinding.GetPath(this.characterPosition, tile);
        if(foundPath.Count > 0)
        {
            MoveCharacter(foundPath);
        }
        foreach (Tile t in possibleRange)
        {
            t.SetOverlayType(OverlayType.None);
        }
        possibleRange.Clear();
        TurnManager.mode = TurnManager.TurnMode.end;
        TurnManager.instance.hasMoved = true;
    }

    public void Action()
    {
        possibleRange = Pathfinding.GetPossibleRange(characterPosition, 2f, true);
        foreach(Tile t in possibleRange)
        {
            t.SetOverlayType(OverlayType.Selected);
        }
    }

    public void CompleteAction(Tile tile)
    {
        Debug.Log("Action completed");
        foreach (Tile t in possibleRange)
        {
            t.SetOverlayType(OverlayType.None);
        }
        possibleRange.Clear();
        TurnManager.mode = TurnManager.TurnMode.end;
        TurnManager.instance.hasActed = true;
    }

    public void SetCharacterPosition(Tile tile)
    {
        tile.SetCharacter(this);
    }

    public void MoveCharacter(List<Tile> path)
    {
        SetCharacterPosition(path[0]);
        DistanceToGo = path.Count-1;
        StartCoroutine(MovePath(path));
    }

    IEnumerator MovePath(List<Tile> path)
    {          
       while (DistanceToGo >= 0)
       {
           Tile target = path[DistanceToGo];
           Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y+1.2f, target.transform.position.z);
           Vector3 sourcePos = this.gameObject.transform.position;
           transform.position = Vector3.MoveTowards(sourcePos, targetPos, Mathf.SmoothStep(0, 1f, TravelSpeed));
           if(transform.position == targetPos)
           {
               DistanceToGo--;
           }
           yield return 0;
           Debug.Log("WE are done");
       }
    }

}

public class CharacterSave
{
    public string name;
    public int id;
    public int xPos;
    public int yPos;

    public CharacterSave(string characterName, int characterId, Tile position)
    {
        name = characterName;
        id = characterId;
        xPos = position.xPos;
        yPos = position.yPos;

        //level, equipment, health, etc to be added later
    }

    public CharacterSave()
    {

    }

    public JObject JsonSave(int row, int column)
    {
        var chara = new JObjectCollection();
        chara.Add("Name", name);
        chara.Add("Id", id);
        chara.Add("xPos", xPos);
        chara.Add("yPos", yPos);

        return chara;
    }


    //load based on id, then tweak into the 'generated character' that was saved
    public void JsonLoad(JObject jObject)
    {
        this.name = jObject["Name"].Value<string>();
        this.id = jObject["Id"].Value<int>();
        this.xPos = jObject["xPos"].Value<int>();
        this.yPos = jObject["yPos"].Value<int>();

    }
}