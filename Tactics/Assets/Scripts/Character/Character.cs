using UnityEngine;
using System.Collections.Generic;
using BonaJson;
using System.Collections;

public class Character : MonoBehaviour, System.IComparable
{

    public string characterName = "tempName";
    public NameType characterNameType = NameType.Rat;
    public string characterClass = "Freelancer";
    public int characterID = 0;
    public Sprite profileSprite;

    //game logic stats
    public float inturnmarkerheight = 5f;
    public float characterEnergy = 5f;
    public int movementRange = 5;
    public float rangedRange = 5f;
    public float TravelSpeed = 0.1f;
    public bool isAi = false;

    public List<ActionType> ActionTypes;

    public Resistance FireStatus;
    public Resistance WaterStatus;
    public Resistance EarthStatus;
    public Resistance WindStatus;
    public InventoryType inventoryType;

    public Dictionary<Elements, Resistance> elementalResistances;

    //logic
    [HideInInspector]
    public Tile characterPosition;
    //[HideInInspector]
    public Item currentItem = null;
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
    [HideInInspector]
    public bool AttackMissed = false;
    [HideInInspector]
    public List<AttackBase> AvailableActions;
    public Dictionary<ActionType, AttackBase> AvailableActionDictionary;

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
    public List<Item> items;
  
    
    void Start()
    {
        Hp = hpMax;
        elementalResistances = new Dictionary<Elements, Resistance>();
        elementalResistances.Add(Elements.Fire, FireStatus);
        elementalResistances.Add(Elements.Water, WaterStatus);
        elementalResistances.Add(Elements.Earth, EarthStatus);
        elementalResistances.Add(Elements.Wind, WindStatus);
        characterAnimator = this.gameObject.GetComponent<Animator>();
    }
    


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

    public void PlayHurtAnimation()
    {
        characterAnimator.SetBool("Hurt", true);
    }

    public void FinishHurtAnimation()
    {
        characterAnimator.SetBool("Hurt", false);
    }

    public void PlayAttackanimation(string AttackAnimationName)
    {
        characterAnimator.SetBool(AttackAnimationName, true);
        AttackAnimationCompleted = false;
    }

    public void AnimationFinished(string AttackName)
    {
        Debug.Log(AttackName+" complete");
        AttackAnimationCompleted = true;
        characterAnimator.SetBool(AttackName, false);
        UIManager.instance.HideAttackName();
        if (TurnManager.instance.CheckIfTurnDone())
        {
            TurnManager.instance.FacingPhase();
        }
    }

   

    public void DisplayDamage()
    {
        currentAction.CompleteAction(targetTile);        
        currentAction = null;
        targetTile = null;
    }

   

    public void MoveCharacter(Character chara, List<Tile> path)
    {
        characterAnimator.SetBool("Walking", true);
        MoveCompleted = false;
        DistanceToGo = path.Count - 1;
        previousPostition = path[DistanceToGo];
        CharacterLogic.instance.SetCharacterPosition(chara, path[0]);
        StartCoroutine(MovePath(chara, path));
    }

    IEnumerator MovePath(Character chara, List<Tile> path)
    {
        while (DistanceToGo >= 0)
        {
            //turn to look the right way
            CharacterLogic.instance.ChangeFacing(chara, previousPostition, path[DistanceToGo]);


            Tile target = path[DistanceToGo];
            Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y + 1f, target.transform.position.z);

            if(target.tileType == TileType.Water)
            {
                targetPos = targetPos - new Vector3(0, 0.3f, 0);
            }

            Vector3 sourcePos = chara.gameObject.transform.position;
            chara.transform.position = Vector3.MoveTowards(sourcePos, targetPos, Mathf.SmoothStep(0, 1f, TravelSpeed));
            if (Mathf.Approximately(chara.transform.position.x,targetPos.x) && Mathf.Approximately(chara.transform.position.y, targetPos.y) && Mathf.Approximately(chara.transform.position.z, targetPos.z))
            {     
                previousPostition = path[DistanceToGo];
                DistanceToGo--;
            }
            yield return 0;
        }
        MoveCompleted = true;
        characterAnimator.SetBool("Walking", false);

        if (TurnManager.instance.CheckIfTurnDone())
        {
            TurnManager.instance.FacingPhase();
        }
    }

}
