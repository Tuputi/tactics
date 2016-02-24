using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourModuleBase : MonoBehaviour {

    public string behaviourName;
    public BehaviourType behaviorType;

    virtual public bool  CheckConditions(Character currentCharacter)
    {
        return true;
    }

    virtual public Tile GetTarget(Character currentCharacter)
    {
        return currentCharacter.characterPosition;
    }

    virtual public void SetAvailableAttacks(Character currentCharacter)
    {
        CharacterLogic.instance.CreateAttackList(currentCharacter);
    }
}
