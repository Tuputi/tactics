using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAi : Character {

    public Character targetCharacter;
    public List<BehaviourModuleBase> publicBehaviorList;
    Dictionary<BehaviourType, BehaviourModuleBase> behaviourList;
    public BehaviourModuleBase currentBehaviour;

    void Awake()
    {
        isAi = true;
        characterID = 2;

        behaviourList = new Dictionary<BehaviourType, BehaviourModuleBase>();
        foreach(BehaviourModuleBase bmb in publicBehaviorList)
        {
            behaviourList.Add(bmb.behaviorType, bmb);
        }
    }

    override public void TakeTurn()
    {
        Debug.Log("Taking turn");
        Debug.Log("My name is " + characterName);
        AiModule();
        Debug.Log("My target name is " + targetCharacter.characterName);
        if (targetCharacter.team == this.team)
        {

        }
        else
        {
            bool DoSearch = AttackTarget();
            if (DoSearch)
            {
                //SearchTarget();
                //if (targetCharacter != null)
                //{
                AiMove();
                //}
                AttackTarget();
            }
        }
        /*bool DoSearch = AttackTarget();
        if (DoSearch)
        {
            SearchTarget();
            if (targetCharacter != null)
            {
                AiMove();
            }
            AttackTarget();
        }*/
        Debug.Log("turn done");
    }

    public void AiModule()
    {

        foreach(BehaviourType behaviourType in behaviourList.Keys)
        {
            if (behaviourList[behaviourType].CheckConditions(this))
            {
                currentBehaviour = behaviourList[behaviourType];
                Tile tempTile = currentBehaviour.GetTarget(this);
                if (tempTile != null)
                {
                    targetCharacter = currentBehaviour.GetTarget(this).character;
                }
                break;
            }
        }



        /*if (currentBehaviour.CheckConditions(this))
        {
            targetCharacter = currentBehaviour.GetTarget(this).character;
            Debug.Log("Current behavior ok");
        }
        else
        {
            currentBehaviour = behaviourList[BehaviourType.agressive];
            targetCharacter = currentBehaviour.GetTarget(this).character;
            Debug.Log("Now agressive");

        }

        if (behaviourList[BehaviourType.teamPlayer].CheckConditions(this) && !(currentBehaviour.behaviorType == BehaviourType.teamPlayer))
        {
            currentBehaviour = behaviourList[BehaviourType.teamPlayer];
            Debug.Log("Back to teamPlayer");
        }*/
    }

    public bool AttackTarget()
    {
        Attack(AttackType.Meelee);
        foreach(Tile t in WithinAttackRange)
        {
            if (t.isOccupied)
            {
                if (t.character.team != this.team)
                {
                    if (t.character.isAlive)
                    {
                        //GameUI.AttackInformation();
                        if (t.character.characterName.Equals(targetCharacter.characterName))
                       {
                            CompleteAttack(t);
                            Debug.Log("Attack");
                            return false;
                       }
                    }
                }
            }
        }
        return true;
    }


    public void SearchTarget()
    {
       Tile tempTile = PathFinding.FindTarget(this.position, this.team);
       if (tempTile != null)
       {
            targetCharacter = tempTile.character;
       }
        else
        {
            targetCharacter = null;
        }
    }

   

    public void AiMove()
    {
        //get to a neighbouring tile of target
        Tile targetTile = null;
        foreach(Tile t in targetCharacter.position.neighbours)
        {
            if(targetTile == null)
            {
                if (!t.isOccupied)
                {
                    targetTile = t;
                }
            }
            else if (PathFinding.GetHeuristic(this.position, targetTile) > PathFinding.GetHeuristic(this.position, t))
            {
                if (!t.isOccupied)
                {
                    targetTile = t;
                }
            }
        }
       
        List<Tile> path = PathFinding.GetPath(this.position, targetTile);
        List<Tile> takePath = new List<Tile>();
        float cost = 0;
        foreach(Tile t in path)
        {
            if(t.gCost+cost < this.characterWalkEnergy)
            {
                takePath.Add(t);
                cost += t.gCost;
                //t.SetOverlayType(OverlayType.Path, t);
            }
            else
            {
               // break;
            }
        }

        List<Tile> range = this.PossibleRange();
        this.WithinMovementRange = range;
        CompleteMove(takePath[0]);
        //takePath[0].SetCharacter(this);
    }


}
