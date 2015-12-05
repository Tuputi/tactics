using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamPlayer : BehaviourModuleBase {

	void Awake()
    {

    }

    public override bool CheckConditions(Character currentCharacter)
    {
        List<Tile> reachableArea = PathFinding.GetPossibleRange(currentCharacter.position, currentCharacter.characterRangeEnergy);
        bool friendsTargetFound = false;
        foreach (Tile t in reachableArea)
        {
            if (t.isOccupied)
            {
                if (t.character.isAlive)
                {
                    if (t.character.team == currentCharacter.team)
                    {
                        Debug.Log(t.character.characterName);
                        if (!t.character.characterName.Equals(currentCharacter.characterName))
                        {
                            if (!(t.character.GetComponent<CharacterAi>().targetCharacter == null) && !(t.character.GetComponent<CharacterAi>().targetCharacter.team == currentCharacter.team))
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
        List<Tile> reachableArea = PathFinding.GetPossibleRange(currentCharacter.position, currentCharacter.characterRangeEnergy);
        List<Character> friendsTargets = new List<Character>();
        foreach (Tile t in reachableArea)
        {
            if (t.isOccupied)
            {
                if (t.character.isAlive)
                {
                    if (t.character.team == currentCharacter.team)
                    {
                        if (!t.character.characterName.Equals(currentCharacter.characterName))
                        {
                            if (!(t.character.GetComponent<CharacterAi>().targetCharacter == null) && !(t.character.GetComponent<CharacterAi>().targetCharacter.team == currentCharacter.team))
                            {
                                friendsTargets.Add(t.character.GetComponent<CharacterAi>().targetCharacter);
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
                else if (!(chara == null) && PathFinding.GetHeuristic(currentCharacter.position, chara.position) < PathFinding.GetHeuristic(currentCharacter.position, closestToMe.position))
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
            return closestToMe.position;
        }      
    }
}
