using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[System.Serializable]
public class Character: MonoBehaviour {

	public string characterName;
    public Tile position;
    public Team team = Team.Blue;
    public bool isAi = false;
    public bool isAlive = true;
    public int characterID = 1;
    public Sprite characterSprite;

    //stats
    public float characterMaxHealth = 100f;
    public float characterHealth = 100f;
    public float characterSpeed = 1.1f;
    public float characterStamina = 1.1f;
    //use this for alculating turn
    public float characterEnergy = 2f;
    //use this for calculating walking distance
    public float characterWalkEnergy = 3f;

    public float characterRangeEnergy = 7f;

    public List<Tile> WithinMovementRange = null;
    public List<Tile> WithinAttackRange = null;

    public AttackType currentAttack = AttackType.Meelee;
    public Facing facing = Facing.Left;


    public List<AttackBase> attackList;
    public Dictionary<AttackType, AttackBase> characterAttackList;
    

    void Start()
    {
        characterAttackList = new Dictionary<AttackType, AttackBase>();
        foreach(AttackBase ab in attackList)
        {
           // Debug.Log(ab);
            characterAttackList.Add(ab.attackType, ab);
        }
    }

    virtual public void TakeTurn()
    {

    }

    public void EnergyCalculation(){
        if (characterEnergy < 10)
        {
            characterEnergy += characterSpeed;
        }
    }
    

	/*public int CompareTo(Character other){
		if (this.characterSpeed > other.characterSpeed) {
			return -1; //faster
		} else if (this.characterSpeed < other.characterSpeed) {
			return 1;//slower
		} else if (this.characterSpeed == other.characterSpeed) {
			return 0;//same speed
		} else {
			return 1;//
		}
	}*/

    public void Move()
    {
        List<Tile> range = new List<Tile>();
        OverlayType type = OverlayType.Empty;
        type = OverlayType.Path;
        range = this.PossibleRange();
        this.WithinMovementRange = range;
        if (range.Count > 0)
        {
            foreach (Tile t in range)
            {
                t.SetOverlayType(type, t);
            }
        }
    }

    public void CompleteAttack(Tile targetTile)
    {
        if(this.WithinAttackRange.Contains(targetTile)){    
                           
           ChangeFacing(this.position, targetTile);
           characterAttackList[currentAttack].CalculateDamage(targetTile, this);
           characterAttackList[currentAttack].PlayAnimation(this);
           TurnManager.currentlytakingTurn.characterEnergy += -2f;
           TurnManager.HasAttacked = true; 
           TurnManager.turnState = TurnState.Undecided;
        }
    }

    public void CompleteMove(Tile targetTile)
    {
        if (this.WithinMovementRange.Contains(targetTile))
        {
            SelectionManager.SetTarget(targetTile);
            List<Tile> tempPath = PathFinding.GetPath(this.position, targetTile);
            float cost = 0f;
            foreach (Tile t in tempPath)
            {
                cost += t.gCost;
            }
            TurnManager.currentlytakingTurn.characterEnergy -= cost;
            Tile characterPosition = SelectionManager.selectedCharacter.position;
            CharacterMovementManager.SetPosition(characterPosition.positionRow, characterPosition.height, characterPosition.positionColumn, SelectionManager.selectedCharacter);
            CharacterMovementManager.SetPath(tempPath);
            targetTile.SetCharacter(SelectionManager.selectedCharacter);
            TurnManager.turnState = TurnState.Undecided;
            TurnManager.HasMoved = true;
        }
    }

    public void Attack(AttackType attack)
    {
        currentAttack = attack;

        List<Tile> range = new List<Tile>();
        OverlayType type = OverlayType.Attack;
        range = this.AttackRange(attack);
        this.WithinAttackRange = range;
        if (range.Count > 0)
        {
            foreach (Tile t in range)
            {
                if (!isAi)
                {
                    
                    t.SetOverlayType(type, t);
                }
            }
        }
    }

    public List<Tile> PossibleRange()
    {
        List<Tile> tempList = PathFinding.GetPossibleRange(position, characterWalkEnergy);
        return tempList;
    }

    public List<Tile> AttackRange(AttackType aType)
    {
        return characterAttackList[aType].CalculateAttackRange(this.position);
    }

    public void ChangeMarker(bool status)
    {
        Transform effect = this.transform.FindChild("Effects").gameObject.transform;
        if(effect.childCount > 0 && !status)
        {
            for(int i = 0; i < effect.childCount; i++)
            {
                Destroy(effect.transform.GetChild(i).gameObject);
            }
        }
        else if(status)
        {
            GameObject newEffect = (GameObject)Instantiate(PrefabHolder.instance.marker_prefab);
            newEffect.transform.SetParent(effect);
            newEffect.gameObject.transform.position = effect.position;
            
        }
    }

    public void ChangeFacing(Tile at, Tile to)
    {
        int rowChange = System.Math.Abs(at.positionRow-to.positionRow);
        int columnChange = System.Math.Abs(at.positionColumn - to.positionColumn);

        GameObject rotateObj = this.transform.FindChild("CharacterObject").gameObject;

        if(rowChange > columnChange)
        {
            if (at.positionRow < to.positionRow)
            {
                facing = Facing.Left;
                float rotation = 180f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
                facing = Facing.Right;
                float rotation = 0f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
        }
        else
        {
            if (at.positionColumn < to.positionColumn)
            {
                facing = Facing.Down;
                float rotation = 90f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
                facing = Facing.Up;
                float rotation = 270f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
        }
        position.SetCharacter(this);
        //Debug.Log("Facing now " + facing);
    }

    public void ChangeFacing(Facing faceDirection)
    {
        GameObject rotateObj = this.transform.FindChild("CharacterObject").gameObject;
        float rotation = -1f;
        switch (faceDirection)
        {
            case Facing.Left:
                facing = Facing.Left;
                rotation = 180f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
                break;
            case Facing.Right:
                facing = Facing.Right;
                rotation = 0f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
                break;
            case Facing.Up:
                facing = Facing.Up;
                rotation = 270f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
                break;
            case Facing.Down:
                facing = Facing.Down;
                rotation = 90f;
                rotateObj.gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
                break;
            default:
                break;
        }
        position.SetCharacter(this);
    }
}
