using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeeleeAttack : ActionBase {

    public override ActionType GetActionType()
    {
        actionType = ActionType.MeeleeAttack;
        return actionType;
    }

    public override string GetName()
    {
        attackName = "Meelee Attack";
        return attackName;
    }

    public override int CalculateDamage(Tile targetTile, Character chara)
    {
        int rando = Random.Range(1, 2);
        int damage = rando * -2;
        chara.ChangeFacing(chara.characterPosition, targetTile);
        if (targetTile.tileCharacter.facing == chara.facing)
        {
            damage *= 2;
            Debug.Log("backstab");
        }


        Debug.Log("Did " + damage + " to " + targetTile.tileCharacter.characterName);
        targetTile.tileCharacter.hp += damage;
        return damage;
    }

    /*public override List<Tile> DrawTargetArea(Tile targetTile)
    {
        List<Tile> temp = new List<Tile>();
        temp.Add(targetTile);
        return temp;
    }*/
}
