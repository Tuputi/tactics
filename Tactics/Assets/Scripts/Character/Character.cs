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

    //Movement
    public Facing facing = Facing.Up;

    //stats
    [Header("Stats")]
    public int hp = 100;
    public int mp = 100;
    public int speed = 10;

    //lists
    public List<ItemBase> items;
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