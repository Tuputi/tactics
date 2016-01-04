﻿using UnityEngine;
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

    public virtual int CalculateDamage(Tile targetTile)
    {
        int damage = 10;
        return damage;
    }

    public virtual List<Tile> CalculateAttackRange(Tile startTile)
    {
        return Pathfinding.GetPossibleRange(startTile, 2f, true);
    }

    public virtual List<Tile> DrawTargetArea(Tile targetTile)
    {
        List<Tile> temp = new List<Tile>();
        temp.Add(targetTile);
        return temp;
    }

    public virtual void PlayAnimation(Character chara)
    {
        chara.GetComponent<Animator>().Play("ShootArrow");
    }

    public virtual void CompleteAction(Tile TargetTile)
    {
        CalculateDamage(TargetTile);
    }
}