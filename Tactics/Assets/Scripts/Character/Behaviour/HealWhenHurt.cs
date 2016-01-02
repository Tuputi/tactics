using UnityEngine;
using System.Collections;

public class HealWhenHurt : BehaviourModuleBase {

    public double TriggerValue;

    public override bool CheckConditions(Character currentCharacter)
    {
        double healthPercentage = currentCharacter.hp / currentCharacter.hp;
       // Debug.Log("HealthPercentage = " + healthPercentage);
        if (healthPercentage <= TriggerValue)
        {
            Debug.Log("HealWhenHurt = " + true);
            currentCharacter.hp += 2;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override Tile GetTarget(Character currentCharacter)
    {
        return currentCharacter.characterPosition;
    }

}
