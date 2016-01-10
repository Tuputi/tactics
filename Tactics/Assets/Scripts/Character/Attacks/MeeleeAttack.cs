using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeeleeAttack : AttackBase {

    public override ActionType GetActionType()
    {
        actionType = ActionType.MeeleeAttack;
        return actionType;
    }

    public override string GetName()
    {
        actionName = "Meelee Attack";
        return actionName;
    }

    public override int CalculateEffect(Tile targetTile)
    {
        if (!targetTile.isOccupied)
        {
            return 0;
        }

        Character currentChara = TurnManager.instance.CurrentlyTakingTurn;
        int rando = Random.Range(1, 2);
        int damage = rando * minDamage;
        currentChara.ChangeFacing(currentChara.characterPosition, targetTile);
        if(damage > maxDamage)
        {
            damage = maxDamage;
        }
        if (targetTile.tileCharacter.facing == currentChara.facing)
        {
            damage *= 2;
            Debug.Log("backstab");
        }


        Debug.Log("Did " + damage + " to " + targetTile.tileCharacter.characterName);
        targetTile.tileCharacter.hp -= damage;
        return damage;
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
