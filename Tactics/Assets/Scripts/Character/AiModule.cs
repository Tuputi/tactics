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

    bool waitingToCompleteMove = false;
    bool waitingToCompleteAttack = false;

    void Awake()
    {
        myCharacter = this.gameObject.GetComponent<Character>();
        myCharacter.isAi = true;

        behaviourList = new Dictionary<BehaviourType, BehaviourModuleBase>();
        myCharacter.ActionTypes = new List<ActionType>();
        foreach (BehaviourModuleBase bmb in publicBehaviorList)
        {
            behaviourList.Add(bmb.behaviorType, bmb);
        }
    }

    public void TakeTurn()
    {
        SelectBehaviour();
        bool DoSearch = AttackTarget();
        if (DoSearch)
        {
           AiMove();
           waitingToCompleteMove = true;
        }
        else
        {
            waitingToCompleteAttack = true;
        }
    }



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
        foreach (AttackBase ab in myCharacter.AvailableActions)
        {
            Debug.Log(ab.AttackName);
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

        Tile targetTile = null;
        if (targetCharacter == null)
        {
            targetTile = Pathfinding.FindClosestEnemy(myCharacter.characterPosition, false);
        }
        else
        {
            targetTile = targetCharacter.characterPosition;
        }

        //from which distance can we attack the target?
        tempTotalAttackRange = new List<Tile>();
        foreach(AttackBase ab in myCharacter.AvailableActions)
        {
            foreach(Tile t in ab.CalculateActionRange(targetTile))
            {
                if (!tempTotalAttackRange.Contains(t))
                {
                    tempTotalAttackRange.Add(t);
                }
            }
        }
        //gototile is the closest position where an attack reachers the enemy
        Tile goToTile = Pathfinding.FindTargetTile(myCharacter.characterPosition, tempTotalAttackRange);

        if(goToTile == null)
        {
            //silertejp
            foreach(Tile t in targetTile.neighbours)
            {
                if(!t.isOccupied && t.isWalkable)
                {
                    goToTile = t;
                }
            }
            if(goToTile == null)
            {
                List<Tile> largerSearch = new List<Tile>();
                foreach(Tile t in targetTile.neighbours)
                {
                    largerSearch.Add(t);
                    foreach(Tile ti in t.neighbours)
                    {
                        largerSearch.Add(ti);
                    }
                }

                foreach(Tile til in largerSearch)
                {
                    if (!til.isOccupied && til.isWalkable)
                    {
                        goToTile = til;
                    }
                }
            }
            if(goToTile == null)
            {
                Debug.Log("Do nothing");
                return;
            }


           /* goToTile = FindAnAccessableTarget();
            if(goToTile == null)
            {
                Debug.Log("No target possible,  wahtsoever");
                return;
            }*/
        }

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
