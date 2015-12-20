using UnityEngine;
using System.Collections;
using BonaJson;

public class Character : MonoBehaviour {

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

    //stats
    public int hp = 100;
    public int mp = 100;
    public int speed = 10;


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