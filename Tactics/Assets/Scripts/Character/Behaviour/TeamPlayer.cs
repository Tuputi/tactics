using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamPlayer : BehaviourModuleBase {

	void Awake()
    {

    }

    public override bool CheckConditions(Character currentCharacter)
    {
        List<Tile> reachableArea = Pathfinding.GetPossibleRange(currentCharacter.characterPosition, currentCharacter.characterRangeEnergy, false);
        bool friendsTargetFound = false;
        foreach (Tile t in reachableArea)
        {
            if (t.isOccupied)
            {
                if (t.tileCharacter.isAlive)
                {
                    if (t.tileCharacter.isAi)
                    {
                        if (!t.tileCharacter.characterName.Equals(currentCharacter.characterName))
                        {
                            if (!(t.tileCharacter.GetComponent<AiModule>().targetCharacter == null) && !(t.tileCharacter.GetComponent<AiModule>().targetCharacter.isAi))
                            {
                                friendsTargetFound = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("TeamPlayer = "+friendsTargetFound);
        return friendsTargetFound;
    }

    public override Tile GetTarget(Character currentCharacter)
    {
        List<Tile> reachableArea = Pathfinding.GetPossibleRange(currentCharacter.characterPosition, currentCharacter.characterRangeEnergy, true);
        List<Character> friendsTargets = new List<Character>();
        foreach (Tile t in reachableArea)
        {
            if (t.isOccupied)
            {
                if (t.tileCharacter.isAlive)
                {
                    if (t.tileCharacter.isAi)
                    {
                        if (!t.tileCharacter.characterName.Equals(currentCharacter.characterName))
                        {
                            if (!(t.tileCharacter.GetComponent<AiModule>().targetCharacter == null) && !(t.tileCharacter.GetComponent<AiModule>().targetCharacter.isAi))
                            {
                                friendsTargets.Add(t.tileCharacter.GetComponent<AiModule>().targetCharacter);
                            }
                        }
                    }
                }
            }
        }

        Character closestToMe = null;
     
        foreach (Character chara in friendsTargets)
        {
                if (closestToMe == null)
                {
                    closestToMe = chara;
                }
                else if (!(chara == null) && Pathfinding.GetHeuristic(currentCharacter.characterPosition, chara.characterPosition) < Pathfinding.GetHeuristic(currentCharacter.characterPosition, closestToMe.characterPosition))
                {
                    closestToMe = chara;
                }
        }

        if(closestToMe == null)
        {
            Debug.Log("null in closesttome");
            return null;
        }
        else
        {
            return closestToMe.characterPosition;
        }      
    }

    public override void SetAvailableAttacks(Character currentCharacter)
    {
        List<AttackBase> temp = new List<AttackBase>();
        temp.Add(new MeeleeAttack());
        temp.Add(new ShootArrow());
        currentCharacter.GetComponent<AiModule>().availableAttacks = temp;
    }
}
