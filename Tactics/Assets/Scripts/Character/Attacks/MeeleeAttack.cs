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
        //currentChara.ChangeFacing(currentChara.characterPosition, targetTile);
        CharacterLogic.instance.ChangeFacing(currentChara, currentChara.characterPosition, targetTile);
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
        CharacterLogic.instance.DisplayEffect(targetTile.tileCharacter, -damage);
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

    public override int GetHitChance(Tile TargetTile)
    {
        int hitChance = 100;
        if (TargetTile.isOccupied)
        {
            if (TurnManager.instance.CurrentlyTakingTurn.facing != TargetTile.tileCharacter.facing)
            {
                hitChance -= 30;
            }
        }
        return hitChance;
    }
}
