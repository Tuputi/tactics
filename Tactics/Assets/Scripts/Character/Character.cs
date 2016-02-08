using UnityEngine;
using System.Collections.Generic;
using BonaJson;
using System.Collections;

public class Character : MonoBehaviour, System.IComparable
{

    public string characterName = "tempName";
    public int characterID = 0;
    

    //game logic stats
    public float characterEnergy = 5f;
    public int movementRange = 5;
    public float rangedRange = 5f;
    public float TravelSpeed = 0.1f;
    public bool isAi = false;

    public List<AttackBase> availableAttacks;

    //logic
    [HideInInspector]
    public Tile characterPosition;
    [HideInInspector]
    public ItemBase currentItem = null;
    [HideInInspector]
    public ActionBaseClass currentAction = null;
    [HideInInspector]
    public bool AttackAnimationCompleted = true;
    [HideInInspector]
    public bool MoveCompleted = true;
    [HideInInspector]
    public Tile targetTile;
    [HideInInspector]
    public Inventory CharacterInventory;
    [HideInInspector]
    public List<Tile> possibleRange;

    //Movement
    [HideInInspector]
    public Facing facing = Facing.Down;
    Tile previousPostition;
    int DistanceToGo = 0;
    private Animator characterAnimator;

    //stats
    [Header("Stats")]
   
    public int hpMax = 100;
    public int mpMax = 100;
    public int speed = 10;

    //private stats && setter/getters
    private int hp = 100;
    private int mp = 100;

    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
        }
    }
    public int Mp
    {
        get
        {
            return mp;
        }

        set
        {
            mp = value;
        }
    }
    public bool isAlive
    {
        get
        {
            return (Hp > 0);
        }
    }

    private GameObject Weapon;

    //lists
    public List<ItemType> items;
  

    


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
        if (!UIManager.instance.IsPointerOverUIObject())
        {
            characterPosition.SelectThis();
        }
    }



    //Animation Controllers

    public void PlayAttackanimation(string AttackAnimationName)
    {
        AttachWeapon();
        if (characterAnimator == null)
        {
            characterAnimator = this.gameObject.GetComponent<Animator>();
        }
        characterAnimator.SetBool(AttackAnimationName, true);
        AttackAnimationCompleted = false;
    }

    public void AnimationFinished(string AttackName)
    {
        Debug.Log(AttackName+" complete");
        AttackAnimationCompleted = true;
        characterAnimator.SetBool(AttackName, false);
        RemoveWeapon();
    }

    public void AttachWeapon()
    {
        Transform handBone = gameObject.transform.FindChild("Armature/UpperBody/Shoulder_R/Elbow_R/Hand_R").FindChild("Hand_R_1");
        GameObject newBow = Instantiate(PrefabHolder.instance.Bow);
        Weapon = newBow;
       // newBow.transform.position = new Vector3(0, 0, 0);
        newBow.transform.SetParent(handBone);
        newBow.transform.localPosition = new Vector3(0, 0, 0);
        //change rotation as well
    }

    public void RemoveWeapon()
    {
        Destroy(Weapon);
    }

    public void DisplayDamage()
    {
        if(targetTile == null)
        {
            Debug.Log("Missed Target");
            CharacterLogic.instance.DisplayEffect(this, 0);
            return;
        }
        currentAction.CompleteAction(targetTile);
        targetTile = null;
    }

    public void MoveCharacter(Character chara, List<Tile> path)
    {
        if(characterAnimator == null)
        {
            characterAnimator = this.gameObject.GetComponent<Animator>();
        }

        characterAnimator.SetBool("Walking", true);
        MoveCompleted = false;
        DistanceToGo = path.Count - 1;
        // CameraScript.instance.SetMoveTarget(path[0].gameObject);
        previousPostition = path[DistanceToGo];
        CharacterLogic.instance.SetCharacterPosition(chara, path[0]);
        CharacterLogic.instance.ChangeFacing(chara, characterPosition, path[DistanceToGo]);

        StartCoroutine(MovePath(chara, path));
    }

    IEnumerator MovePath(Character chara, List<Tile> path)
    {
        while (DistanceToGo >= 0)
        {
            //turn to look the right way
            int tempInt = DistanceToGo;
            if (!(--tempInt < 0))
            {
                CharacterLogic.instance.ChangeFacing(chara, previousPostition, path[DistanceToGo - 1]);
            }
            else
            {
                CharacterLogic.instance.ChangeFacing(chara, previousPostition, path[DistanceToGo]);
            }

            Tile target = path[DistanceToGo];
            Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y + 1f, target.transform.position.z);
            Vector3 sourcePos = chara.gameObject.transform.position;
            chara.transform.position = Vector3.MoveTowards(sourcePos, targetPos, Mathf.SmoothStep(0, 1f, TravelSpeed));
            if (chara.transform.position == targetPos)
            {
                
                previousPostition = path[DistanceToGo];
                DistanceToGo--;
            }
            yield return 0;
        }
        Debug.Log("Move complete");
        MoveCompleted = true;
        characterAnimator.SetBool("Walking", false);
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