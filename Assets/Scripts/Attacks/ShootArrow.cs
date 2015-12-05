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
        if (targetTile.character.facing == chara.facing)
        {
            damageR *= 2;
            Debug.Log("backstab");
        }
        if (System.Math.Abs(targetTile.height - chara.position.height) > 1 && chara.position.height > targetTile.height)
        {
            damageR *= 2;
            Debug.Log("heightAdvantage");
        }
        Debug.Log("Did " + damageR + " to " + targetTile.character.characterName);
        targetTile.character.characterHealth += damageR;
        GameUI.showDamage(targetTile.character, damageR);
        if (targetTile.character.characterHealth <= 0)
        {
            Debug.Log(targetTile.character + " is dead");
            targetTile.character.isAlive = false;
        }
        return damageR;
    }

    public override List<Tile> CalculateAttackRange(Tile startTile)
    {
        return PathFinding.GetPossibleRange(startTile, startTile.character.characterRangeEnergy);
    }

    public override void DrawTargetArea(Tile targetTile)
    {
        targetTile.SetOverlayType(OverlayType.Path, targetTile);
    }



}
