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
        return Pathfinding.FindTarget(currentCharacter.characterPosition,false);
    }

    public override void SetAvailableAttacks(Character currentCharacter)
    {
        List<ActionBase> temp = new List<ActionBase>();
        temp.Add(PrefabHolder.instance.actionDictionary[ActionType.MeeleeAttack]);
        temp.Add(PrefabHolder.instance.actionDictionary[ActionType.ShootArrow]);
        currentCharacter.GetComponent<AiModule>().availableAttacks = temp;
    }
}
