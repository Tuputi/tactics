using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamPlayer : BehaviourModuleBase {

    private List<AttackBase> activeAttacks;

	void Awake()
    {

    }

    public override bool CheckConditions(Character currentCharacter)
    {
        List<Character> charas = new List<Character>();

        //create a list of all characters of the same team with a target
        foreach(Character cha in TurnManager.characters)
        {
            if (!cha == TurnManager.instance.CurrentlyTakingTurn)
            {
                if (cha.isAi)
                {
                    if (cha.GetComponent<AiModule>().targetCharacter != null)
                    {
                        charas.Add(cha);
                    }
                }
            }
        }

        foreach(Character c in charas)
        {
           float distance = Pathfinding.GetHeuristic(c.GetComponent<AiModule>().targetCharacter.characterPosition, currentCharacter.characterPosition);
            if (distance <= currentCharacter.movementRange + 1)
            {
                Debug.Log("Teamplayer active");
                return true;
            }
        }

        return false;
    }

    public override Tile GetTarget(Character currentCharacter)
    {
        Character closestToMe = null;
        List<Character> charas = new List<Character>();
        foreach (Character cha in TurnManager.characters)
        {
            if (cha != currentCharacter)
            {
                if (cha.isAi)
                {
                    if (cha.GetComponent<AiModule>().targetCharacter != null)
                    {
                        charas.Add(cha);
                    }
                }
            }
        }

        foreach (Character chara in charas)
        {
            float distance = Pathfinding.GetHeuristic(chara.GetComponent<AiModule>().targetCharacter.characterPosition, currentCharacter.characterPosition);
            if (distance <= currentCharacter.movementRange + 1)
            {
                if(closestToMe == null)
                {
                    closestToMe = chara.GetComponent<AiModule>().targetCharacter;
                }
                else
                {
                    if(distance < Pathfinding.GetHeuristic(closestToMe.characterPosition, currentCharacter.characterPosition))
                    {
                        closestToMe = chara.GetComponent<AiModule>().targetCharacter;
                    }
                }
            }
        }

        Debug.Log("Closest to me is " + closestToMe);
        return closestToMe.characterPosition;

    }

    public override void SetAvailableAttacks(Character currentCharacter)
    {
        CharacterLogic.instance.CreateAttackList(currentCharacter);
    }
}
