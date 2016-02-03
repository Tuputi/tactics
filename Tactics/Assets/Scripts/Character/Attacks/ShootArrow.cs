using UnityEngine;
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

        if (!targetTile.isOccupied)
        {
            return 0;
        }

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
        CharacterLogic.instance.DisplayEffect(targetTile.tileCharacter, damageR);
        return damageR;
    }

    public override List<Tile> CalculateActionRange(Tile startTile)
    {
        return Pathfinding.GetPossibleRange(startTile, TurnManager.instance.CurrentlyTakingTurn.rangedRange, true);
    }

    public override List<Tile> CalculateActionRange(Tile startTile, ItemBase ib)
    {
        float range = TurnManager.instance.CurrentlyTakingTurn.rangedRange + BasicRange + ib.GetRangeEffect();
        Debug.Log("Range is " + range);
        return Pathfinding.GetPossibleRange(startTile, range, true);
    }

    public override List<Tile> DrawTargetArea(Tile targetTile)
    {
        List<Tile> temp = new List<Tile>();
        temp.Add(targetTile);
        return temp;
    }

    public override void CompleteAction(Tile TargetTile)
    {
        CalculateEffect(TargetTile);
    }

}
