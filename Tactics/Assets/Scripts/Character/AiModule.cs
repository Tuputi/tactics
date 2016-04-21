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
    private ActionType currentActionType;
    bool waitingToCompleteMove = false;
    bool waitingToCompleteAttack = false;
    public bool WaitingToEndTurn = false;
    bool waitAMoment = false;
    private Tile attackTargetTile = null;

    float currentTime = 0f;
    float WaitTime = 2f;
    void Awake()
    {
        myCharacter = this.gameObject.GetComponent<Character>();
        myCharacter.isAi = true;

        behaviourList = new Dictionary<BehaviourType, BehaviourModuleBase>();
        //myCharacter.ActionTypes = new List<ActionType>();
        foreach (BehaviourModuleBase bmb in publicBehaviorList)
        {
            behaviourList.Add(bmb.behaviorType, bmb);
        }
    }

    public void TakeTurn()
    {
        SelectBehaviour();
        if (!TargetWithinAttackRange())
        {
           TurnManager.instance.Move();
           SelectionScript.SetNoSelection(true);
           waitAMoment = true;        
        }
        else
        {
            TurnManager.instance.Action(currentActionType);
            waitAMoment = true;
        }
    }



    void Update()
    {
        if (waitAMoment)
        {
            currentTime += Time.deltaTime;
            if(currentTime > WaitTime)
            {
                SelectionScript.SetNoSelection(false);
                Debug.Log("Done!");
                currentTime = 0f;
                waitAMoment = false;
                switch (TurnManager.mode)
                {
                    case TurnManager.TurnMode.undecided:
                        break;
                    case TurnManager.TurnMode.start:
                        break;
                    case TurnManager.TurnMode.move:
                        AiMove();
                        SelectionScript.SetNoSelection(true);
                        waitingToCompleteMove = true;
                        break;
                    case TurnManager.TurnMode.action:
                        AttackTarget();
                        SelectionScript.SetNoSelection(true);
                        waitingToCompleteAttack = true;
                        break;
                    case TurnManager.TurnMode.facing:
                        break;
                    case TurnManager.TurnMode.end:
                        break;
                    default:
                        break;
                }
            }
        }

        if (waitingToCompleteMove)
        {
            if (myCharacter.MoveCompleted)
            {
                waitingToCompleteMove = false;
                SelectionScript.SetNoSelection(false);

                if (TargetWithinAttackRange())
                {
                    TurnManager.instance.Action(currentActionType);
                    SelectionScript.SetNoSelection(true);
                    waitAMoment = true;
                }
                else
                {
                    EndTurn();
                }
            }
        }

        if (waitingToCompleteAttack)
        {
            Debug.Log("Artificial doubleclick");
            SelectionScript.SetNoSelection(false);
            TacticsInput.instance.ResetDoubleClick();
            TacticsInput.instance.CreateDoubleClick();
            waitingToCompleteAttack = false;
        }

        if (WaitingToEndTurn)
        {
            if (myCharacter.AttackAnimationCompleted)
            {
                WaitingToEndTurn = false;
                EndTurn();
            }
        }
    }

    private void EndTurn()
    {
        Debug.Log("turn done");
        Tile targetT = GetClosestEnemy();
        CharacterLogic.instance.ChangeFacing(myCharacter, myCharacter.characterPosition, targetT);
        TurnManager.instance.FindNextInTurn();
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

    public bool TargetWithinAttackRange(){
        foreach (AttackBase ab in myCharacter.AvailableActions)
        {
            List<Tile> attackRange = ab.CalculateActionRange(myCharacter.characterPosition);
            foreach (Tile t in attackRange)
            {
                if (t.isOccupied)
                {
                    if (t.tileCharacter.characterName.Equals(targetCharacter.characterName))
                    {
                        currentActionType = ab.actionType;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void AttackTarget()
    {
        attackTargetTile = null;
        foreach(Tile t in myCharacter.possibleRange)
        {
            List<Tile> tempRange = new List<Tile>(myCharacter.AvailableActionDictionary[currentActionType].DrawTargetArea(t));
            foreach(Tile tile in tempRange)
            {
                if (tile.isOccupied)
                {
                    if (tile.tileCharacter.characterName.Equals(targetCharacter.characterName))
                    {
                        attackTargetTile = tile;
                        break;
                    }
                }
            }
        }
        if(attackTargetTile != null)
        {
            attackTargetTile.SelectThis();
        }
    }

    public void AiMove()
    {
        Tile bestTile = null;
        Debug.Log("Current possible range "+ myCharacter.possibleRange.Count);
        foreach(Tile t in myCharacter.possibleRange)
        {
            float distance = Pathfinding.GetHeuristic(t, targetCharacter.characterPosition);
            if(bestTile == null)
            {
                bestTile = t;
            }
            else if(distance < Pathfinding.GetHeuristic(bestTile, targetCharacter.characterPosition))
            {
                bestTile = t;
            }
        }
      
        List<Tile> foundPath = Pathfinding.GetPath(myCharacter.characterPosition, bestTile);

        if (foundPath == null)
        {
            Debug.Log("No possible tile to go to found");
            return;
        }
        CharacterLogic.instance.CompleteMove(myCharacter, foundPath);
    }

    private Tile FindAnAccessableTarget()
    {
        List<Character> possibleCharas = new List<Character>();
        foreach(Character chara in TurnManager.characters)
        {
            if (!chara.isAi)
            {
                possibleCharas.Add(chara);
            }
        }

        foreach(Character cha in possibleCharas)
        {
            tempTotalAttackRange = new List<Tile>();
            foreach (AttackBase ab in myCharacter.AvailableActions)
            {
                foreach (Tile t in ab.CalculateActionRange(cha.characterPosition))
                {
                    if (!tempTotalAttackRange.Contains(t))
                    {
                        tempTotalAttackRange.Add(t);
                    }
                }
            }
        }

        Tile GoToLargerSearch = Pathfinding.FindTargetTile(myCharacter.characterPosition, tempTotalAttackRange);
        return GoToLargerSearch;
    }

}
