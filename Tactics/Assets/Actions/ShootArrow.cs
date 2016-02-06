﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootArrow : AttackBase {

    public override ActionType GetActionType()
    {
        actionType = ActionType.ShootArrow;
        return actionType;
    }

    public override string GetName()
    {
        actionName = "Shoot Arrow";
        return actionName;
    }

    public override bool CalculateHitChance()
    {
        return base.CalculateHitChance();
    }

    public override int CalculateEffect(Tile targetTile)
    {
        bool foundTarget = false;
        foreach(Tile t in attackArea)
        {
            if (t.isOccupied)
            {
                foundTarget = true;
                break;
            }
        }
        if (!foundTarget)
        {
            return 0;
        }

        Character chara = TurnManager.instance.CurrentlyTakingTurn;

        foreach(Tile t in attackArea)
        {
            if (t.isOccupied)
            {
                int randoR = Random.Range(1, 2);
                int damageR = randoR * -2;
                if (t.tileCharacter.facing == chara.facing)
                {
                    damageR *= 2;
                    Debug.Log("backstab");
                }
                if (System.Math.Abs(t.height - chara.characterPosition.height) > 1 && chara.characterPosition.height > t.height)
                {
                    damageR *= 2;
                    Debug.Log("heightAdvantage");
                }
                Debug.Log("Did " + damageR + " to " + t.tileCharacter.characterName);
                t.tileCharacter.hp += damageR;
                CharacterLogic.instance.DisplayEffect(t.tileCharacter, damageR);
            }
        }
        attackArea.Clear();
        return 1;
    }

   /* public override List<Tile> CalculateActionRange(Tile startTile)
    {
        return Pathfinding.GetPossibleRange(startTile, TurnManager.instance.CurrentlyTakingTurn.rangedRange, true);
    }*/

    public override List<Tile> CalculateActionRange(Tile startTile)
    {
        float range = TurnManager.instance.CurrentlyTakingTurn.rangedRange;
        if (TurnManager.instance.CurrentlyTakingTurn.currentItem != null)
        {
            range += TurnManager.instance.CurrentlyTakingTurn.currentItem.GetRangeEffect();
        }
        return Pathfinding.GetPossibleRange(startTile, range, true);
    }

    public override List<Tile> DrawTargetArea(Tile targetTile)
    {
        List<Tile> temp = new List<Tile>();
        temp.Add(targetTile);

        if (TurnManager.instance.CurrentlyTakingTurn.currentItem)
        {
            ItemBase ib = TurnManager.instance.CurrentlyTakingTurn.currentItem;
            if (ib.EffectToTArgetArea > 0)
            {
                List<Tile> tempList = Pathfinding.GetPossibleRange(targetTile, ib.EffectToTArgetArea, true);
                foreach (Tile t in tempList)
                {
                    temp.Add(t);
                }
            }
        }
        attackArea = temp;
        return temp;
    }

    public override void CompleteAction(Tile TargetTile)
    {
        CalculateEffect(TargetTile);
    }

    public override bool CompatibleItem(ItemBase ib)
    {
        foreach(ItemType it in compatibleItems)
        {
            if (ib.itemCategories.Contains(it))
            {
                return true;
            }
        }

        return false;
    }
}