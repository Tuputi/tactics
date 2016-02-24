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
        List<AttackBase> temp = new List<AttackBase>();
        temp.Add(PrefabHolder.instance.actionDictionary[ActionType.Howl]);
       // temp.Add(PrefabHolder.instance.actionDictionary[ActionType.ShootArrow]);
        currentCharacter.GetComponent<Character>().availableAttacks = temp;
    }
}
