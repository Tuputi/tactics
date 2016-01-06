using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireSpell : AttackBase {


    public override ActionType GetActionType()
    {
        actionType = ActionType.FireSpell;
        return actionType;
    }

    public override string GetName()
    {
        actionName = "Fire Spell";
        return actionName;
    }

    public override List<Tile> CalculateActionRange(Tile startTile)
    {
        return Pathfinding.GetPossibleRange(startTile, 4f, true);
    }

    public override int CalculateEffect(Tile targetTile)
    {
        List<Tile> hitsOnTiles = new List<Tile>(targetTile.neighbours);
        hitsOnTiles.Add(targetTile);
        int damageA = 0;
        foreach (Tile t in hitsOnTiles)
        {
            int randoA = Random.Range(1, 3);
            damageA = randoA * minDamage;
            if(damageA < maxDamage)
            {
                damageA = maxDamage;
            }
            if (t.isOccupied)
            {
                Debug.Log("Did " + damageA + " to " + t.tileCharacter.characterName);
                t.tileCharacter.hp -= damageA;
               // t.tileCharacter.CheckIfAlive(); 
            }
        }
        return damageA;
    }

    public override List<Tile> DrawTargetArea(Tile targetTile)
    {
        List<Tile> temp = new List<Tile>(targetTile.neighbours);
        temp.Add(targetTile);
        return temp;
    }

    public override void CompleteAction(Tile TargetTile)
    {
        CalculateEffect(TargetTile);
    }
}
