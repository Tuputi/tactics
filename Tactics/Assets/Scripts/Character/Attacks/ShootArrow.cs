﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootArrow : ActionBase {

    public override ActionType GetActionType()
    {
        actionType = ActionType.ShootArrow;
        return actionType;
    }

    public override string GetName()
    {
        attackName = "Shoot Arrow";
        return attackName;
    }

    public override bool CalculateHitChance()
    {
        return base.CalculateHitChance();
    }

    public override int CalculateDamage(Tile targetTile)
    {
        Character chara = TurnManager.instance.CurrentlyTakingTurn;
        int randoR = Random.Range(1,2);
        int damageR = randoR * -2;
        if (targetTile.tileCharacter.facing == chara.facing)
        {
            damageR *= 2;
            Debug.Log("backstab");
        }
        if (System.Math.Abs(targetTile.height - chara.characterPosition.height) > 1 && chara.characterPosition.height > targetTile.height)
        {
            damageR *= 2;
            Debug.Log("heightAdvantage");
        }
        Debug.Log("Did " + damageR + " to " + targetTile.tileCharacter.characterName);
        targetTile.tileCharacter.hp += damageR;
        return damageR;
    }

    public override List<Tile> CalculateAttackRange(Tile startTile)
    {
        Debug.Log("Shoot arrow range is " + TurnManager.instance.CurrentlyTakingTurn.shootArrowsEnergy);
        return Pathfinding.GetPossibleRange(startTile, TurnManager.instance.CurrentlyTakingTurn.shootArrowsEnergy, true);
    }

    public override List<Tile> DrawTargetArea(Tile targetTile)
    {
        List<Tile> temp = new List<Tile>();
        temp.Add(targetTile);
        return temp;
    }

    public override void CompleteAction(Tile TargetTile)
    {
        CalculateDamage(TargetTile);
    }

}