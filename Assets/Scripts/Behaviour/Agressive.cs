using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agressive : BehaviourModuleBase {

    public override bool CheckConditions(Character currentCharacter)
    {
        Debug.Log("Agressive = true" );
        return true;
    }

    public override Tile GetTarget(Character currentCharacter)
    {
        return PathFinding.FindTarget(currentCharacter.position, currentCharacter.team);
    }
}
