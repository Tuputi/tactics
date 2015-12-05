using UnityEngine;
using System.Collections;

public class HealWhenHurt : BehaviourModuleBase {

    public double TriggerValue;

    public override bool CheckConditions(Character currentCharacter)
    {
        double healthPercentage = currentCharacter.characterHealth / currentCharacter.characterMaxHealth;
       // Debug.Log("HealthPercentage = " + healthPercentage);
        if (healthPercentage <= TriggerValue)
        {
            Debug.Log("HealWhenHurt = " + true);
            currentCharacter.characterHealth += 2;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override Tile GetTarget(Character currentCharacter)
    {
        return currentCharacter.position;
    }

}
