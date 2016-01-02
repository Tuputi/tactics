using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootArrow : AttackBase {

    void Awake()
    {
        attackName = "ShootArrow";
    }


    public override bool CalculateHitChance()
    {
        return base.CalculateHitChance();
    }

    public override int CalculateDamage(Tile targetTile, Character chara)
    {
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
        return Pathfinding.GetPossibleRange(startTile, startTile.tileCharacter.characterRangeEnergy,true);
    }

    public override void DrawTargetArea(Tile targetTile)
    {
        targetTile.SetOverlayType(OverlayType.Selected);
    }



}
