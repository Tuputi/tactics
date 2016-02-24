using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agressive : BehaviourModuleBase {

    public override bool CheckConditions(Character currentCharacter)
    {
        return true;
    }

    public override Tile GetTarget(Character currentCharacter)
    {
        return Pathfinding.FindClosestEnemy(currentCharacter.characterPosition,false);
    }

    public override void SetAvailableAttacks(Character currentCharacter)
    {
        List<ActionType> temp = new List<ActionType>();
        temp.Add(ActionType.Howl);
        CharacterLogic.instance.CreateAttackList(currentCharacter, temp);
    }
}
