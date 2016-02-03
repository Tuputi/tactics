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
    public int movementRange = 5;
    public float rangedRange = 5f;

    //logic
    [HideInInspector]
    public bool isAlive
    {
        get
        {
            return (hp > 0);
        }
    }
    public bool isAi = false;
    public ActionBaseClass currentAction = null;
    int DistanceToGo = 0;
    public float TravelSpeed = 0.1f;
    public bool MoveCompleted = true;

    //Movement
    public Facing facing = Facing.Down;

    //stats
    [Header("Stats")]
    public int hp = 100;
    public int hpMax = 100;
    public int mp = 100;
    public int speed = 10;

    //lists
    public List<ItemType> items;
    [HideInInspector]
    public List<Tile> possibleRange;
    public Inventory CharacterInventory;

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

    //on mouse down
    void OnMouseDown()
    {
        if (!characterPosition.IsPointerOverUIObject())
        {
            characterPosition.SelectThis();
        }
    }

    Tile previousPostition;
    public void MoveCharacter(Character chara, List<Tile> path)
    {
        MoveCompleted = false;
        DistanceToGo = path.Count - 1;
        // CameraScript.instance.SetMoveTarget(path[0].gameObject);
        CharacterLogic.instance.ChangeFacing(chara, characterPosition, path[DistanceToGo]);
        previousPostition = path[DistanceToGo];
        CharacterLogic.instance.SetCharacterPosition(chara, path[0]);
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
                CharacterLogic.instance.ChangeFacing(chara, previousPostition, path[DistanceToGo]);
                previousPostition = path[DistanceToGo];
                DistanceToGo--;
            }
            yield return 0;
        }
        Debug.Log("Move complete");
        MoveCompleted = true;
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