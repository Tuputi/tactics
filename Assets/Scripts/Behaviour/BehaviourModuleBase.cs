using UnityEngine;
using System.Collections;

public enum BehaviourType { agressive, teamPlayer, healWhenHurt}

public class BehaviourModuleBase : MonoBehaviour {

    public string behaviourName;
    public BehaviourType behaviorType;

    virtual public bool  CheckConditions(Character currentCharacter)
    {
        return true;
    }

    virtual public Tile GetTarget(Character currentCharacter)
    {
        return currentCharacter.position;
    }
}
