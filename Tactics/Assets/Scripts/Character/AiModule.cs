using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiModule : Character {

    [HideInInspector]
    public Character targetCharacter;
    public List<BehaviourModuleBase> publicBehaviorList;
    Dictionary<BehaviourType, BehaviourModuleBase> behaviourList;
    [HideInInspector]
    public BehaviourModuleBase currentBehaviour;
    public List<AttackBase> availableAttacks;

    private List<Tile> tempTotalAttackRange;

    void Awake()
    {
        isAi = true;
        behaviourList = new Dictionary<BehaviourType, BehaviourModuleBase>();
        availableAttacks = new List<AttackBase>();
        foreach (BehaviourModuleBase bmb in publicBehaviorList)
        {
            behaviourList.Add(bmb.behaviorType, bmb);
        }
    }

    public void TakeTurn()
    {
        SelectBehaviour();
        //can I atack target?
        bool DoSearch = AttackTarget();
        if (DoSearch)
        {
           //move towards target
           AiMove();
           waitingToCompleteMove = true;
           //wait here until movement completed
           //try to attack target
           //AttackTarget();
            //wait here until attack complete
        }
        else
        {
            waitingToCompleteAttack = true;
        }
        //Debug.Log("turn done");
    }

    bool waitingToCompleteMove = false;
    bool waitingToCompleteAttack = false;

    void Update()
    {
        if (waitingToCompleteMove)
        {
            if (MoveCompleted)
            {
                waitingToCompleteMove = false;
                AttackTarget();
                waitingToCompleteAttack = true;
            }
        }

        if (waitingToCompleteAttack)
        {
            if (AttackAnimationCompleted)
            {
                waitingToCompleteAttack = false;
                Debug.Log("turn done");
                Tile targetT = GetClosestEnemy();
                CharacterLogic.instance.ChangeFacing(this, this.characterPosition, targetT);
                TurnManager.instance.NextInTurn();
            }
        }
    }

    Tile GetClosestEnemy()
    {
        Character closest = null;
        foreach(Character chara in TurnManager.characters)
        {
            if(closest == null && (!chara.isAi))
            {
                closest = chara;
            }
            else if(closest != null && (!chara.isAi))
            {
                if(Pathfinding.GetHeuristic(closest.characterPosition, this.characterPosition) > Pathfinding.GetHeuristic(chara.characterPosition, this.characterPosition))
                {
                    closest = chara;
                }
            }
        }
        return closest.characterPosition;
    }

    public void SelectBehaviour()
    {
        foreach (BehaviourType behaviourType in behaviourList.Keys)
        {
            if (behaviourList[behaviourType].CheckConditions(this))
            {
                currentBehaviour = behaviourList[behaviourType];
                currentBehaviour.SetAvailableAttacks(this);
                Tile tempTile = currentBehaviour.GetTarget(this);
                if (tempTile.tileCharacter != null)
                {
                    targetCharacter = tempTile.tileCharacter;
                    return;
                }
            }
        }
    }

    public bool AttackTarget()
    {
        foreach (AttackBase ab in availableAttacks)
        {
            List<Tile> attackRange = ab.CalculateActionRange(this.characterPosition);
            foreach (Tile t in attackRange)
            {
                if (t.isOccupied)
                {
                   if (t.tileCharacter.characterName.Equals(targetCharacter.characterName))
                    {
                      currentAction = ab;
                      CharacterLogic.instance.CompleteAction(this, t);
                      return false;
                    }
                }
            }
        }
        Debug.Log("None of the attacks could be completed");
        return true;
    }

    public void AiMove()
    {
        Tile targetTile = Pathfinding.FindTarget(this.characterPosition, false);
        if(targetTile == null)
        {
            Debug.Log("No target tile found");
            return;
        }

        //from which distance can we attack the target?
        tempTotalAttackRange = new List<Tile>();
        foreach(AttackBase ab in availableAttacks)
        {
            foreach(Tile t in ab.CalculateActionRange(targetTile))
            {
                if (!tempTotalAttackRange.Contains(t))
                {
                    tempTotalAttackRange.Add(t);
                }
            }
        }

        Tile goToTile = Pathfinding.FindTargetTile(this.characterPosition, tempTotalAttackRange);
        List<Tile> foundPath = Pathfinding.GetPath(this.characterPosition, goToTile);

        if (foundPath !=null)
        {
            List<Tile> takePath = new List<Tile>();
            foreach (Tile t in foundPath)
            {
                if (t.pathfindingCost <= this.movementRange)
                {
                    takePath.Add(t);
                }
            }
            CharacterLogic.instance.CompleteMove(this, foundPath);
        }
        else
        {
            Debug.Log("nope");
        }
    }


}
