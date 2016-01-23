using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[CreateAssetMenu]
public class AttackBase : ActionBaseClass{

    public double attackBaseProbability; //100 = 100%
    public int minDamage = 0;
    public int maxDamage = 0;
    public ActionType actionType = ActionType.MeeleeAttack;

    public virtual ActionType GetActionType()
    {
        actionType = ActionType.MeeleeAttack;
        return actionType;
    }

	public virtual bool CalculateHitChance()
    {
        //true if hit and false if miss
        double chance = Random.Range(20, 30);
        if(chance+attackBaseProbability > 100)
        {
            return true;
        }
        return false;
    }

    

}
