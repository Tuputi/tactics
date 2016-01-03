using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionBase : ScriptableObject{

    public double attackBaseProbability; //100 = 100%
    protected string attackName = "BaseAttack";
    public int likelyDamageLow = 0;
    public int likelyDamageHigh = 0;
    public ActionType actionType = ActionType.MeeleeAttack;

    public virtual ActionType GetActionType()
    {
        actionType = ActionType.MeeleeAttack;
        return actionType;
    }

    public virtual string GetName()
    {
        return attackName;
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

    public virtual int CalculateDamage(Tile t, Character chara)
    {
        int damage = 10;
        return damage;
    }

    public virtual List<Tile> CalculateAttackRange(Tile startTile)
    {
        return Pathfinding.GetPossibleRange(startTile, 2f, true);
    }

    public virtual void DrawTargetArea(Tile targetTile)
    {
        targetTile.SetOverlayType(OverlayType.Selected);
    }

    public virtual void PlayAnimation(Character chara)
    {
        chara.GetComponent<Animator>().Play("ShootArrow");
    }

}
