using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireSpell : ActionBase {


    public override ActionType GetActionType()
    {
        actionType = ActionType.FireSpell;
        return actionType;
    }

    public override string GetName()
    {
        attackName = "Fire Spell";
        return attackName;
    }

    public override List<Tile> CalculateAttackRange(Tile startTile)
    {
        return Pathfinding.GetPossibleRange(startTile, 4f, true);
    }

    public override int CalculateDamage(Tile targetTile, Character chara)
    {
        List<Tile> hitsOnTiles = new List<Tile>(targetTile.neighbours);
        hitsOnTiles.Add(targetTile);
        int damageA = 0;
        foreach (Tile t in hitsOnTiles)
        {
            t.SetOverlayType(OverlayType.Selected);
            int randoA = Random.Range(1, 3);
            damageA = randoA * -2;
            if (t.isOccupied)
            {
                Debug.Log("Did " + damageA + " to " + t.tileCharacter.characterName);
                t.tileCharacter.hp += damageA;
               // t.tileCharacter.CheckIfAlive(); 
            }
        }
        return damageA;
    }

    public override void DrawTargetArea(Tile targetTile)
    {
        List<Tile> tempList = new List<Tile>(targetTile.neighbours);
        tempList.Add(targetTile);
        foreach(Tile t in tempList)
        {
            t.SetOverlayType(OverlayType.Selected);
        }

    }
}
