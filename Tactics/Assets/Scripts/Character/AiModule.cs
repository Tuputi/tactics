using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiModule : MonoBehaviour {

    [HideInInspector]
    public Character targetCharacter;
    public List<BehaviourModuleBase> publicBehaviorList;
    Dictionary<BehaviourType, BehaviourModuleBase> behaviourList;
    [HideInInspector]
    public BehaviourModuleBase currentBehaviour;
    private List<Tile> tempTotalAttackRange;
    private Character myCharacter;

    void Awake()
    {
        myCharacter = this.gameObject.GetComponent<Character>();
        myCharacter.isAi = true;

        behaviourList = new Dictionary<BehaviourType, BehaviourModuleBase>();
        myCharacter.availableAttacks = new List<AttackBase>();
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
            if (myCharacter.MoveCompleted)
            {
                waitingToCompleteMove = false;
                AttackTarget();
                waitingToCompleteAttack = true;
            }
        }

        if (waitingToCompleteAttack)
        {
            if (myCharacter.AttackAnimationCompleted)
            {
                waitingToCompleteAttack = false;
                Debug.Log("turn done");
                Tile targetT = GetClosestEnemy();
                CharacterLogic.instance.ChangeFacing(myCharacter, myCharacter.characterPosition, targetT);
                TurnManager.instance.FindNextInTurn();
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
                if(Pathfinding.GetHeuristic(closest.characterPosition, myCharacter.characterPosition) > Pathfinding.GetHeuristic(chara.characterPosition, myCharacter.characterPosition))
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
            if (behaviourList[behaviourType].CheckConditions(myCharacter))
            {
                currentBehaviour = behaviourList[behaviourType];
                currentBehaviour.SetAvailableAttacks(myCharacter);
                Tile tempTile = currentBehaviour.GetTarget(myCharacter);
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
        TurnManager.mode = TurnManager.TurnMode.action;
        foreach (AttackBase ab in myCharacter.availableAttacks)
        {
            Debug.Log(ab.name);
            List<Tile> attackRange = ab.CalculateActionRange(myCharacter.characterPosition);
            foreach (Tile t in attackRange)
            {
                if (t.isOccupied)
                {
                   if (t.tileCharacter.characterName.Equals(targetCharacter.characterName))
                    {
                        myCharacter.currentAction = ab;
                        myCharacter.currentAction.DrawTargetArea(t);
                        CharacterLogic.instance.CompleteAction(myCharacter, t);
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
        TurnManager.mode = TurnManager.TurnMode.move;
        Tile targetTile = Pathfinding.FindTarget(myCharacter.characterPosition, false);
        if(targetTile == null)
        {
            Debug.Log("No target tile found");
            return;
        }

        //from which distance can we attack the target?
        tempTotalAttackRange = new List<Tile>();
        foreach(AttackBase ab in myCharacter.availableAttacks)
        {
            foreach(Tile t in ab.CalculateActionRange(targetTile))
            {
                if (!tempTotalAttackRange.Contains(t))
                {
                    tempTotalAttackRange.Add(t);
                }
            }
        }

        Tile goToTile = Pathfinding.FindTargetTile(myCharacter.characterPosition, tempTotalAttackRange);
        List<Tile> foundPath = Pathfinding.GetPath(myCharacter.characterPosition, goToTile);

        if (foundPath !=null)
        {
            List<Tile> takePath = new List<Tile>();
            foreach (Tile t in foundPath)
            {
                if (t.pathfindingCost <= myCharacter.movementRange)
                {
                    takePath.Add(t);
                }
            }
            CharacterLogic.instance.CompleteMove(myCharacter, foundPath);
        }
        else
        {
            Debug.Log("nope");
        }
    }


}
