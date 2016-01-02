using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeeleeAttack : AttackBase {

    void Awake()
    {
        attackName = "MeeleeAttack";
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

    public override void DrawTargetArea(Tile targetTile)
    {
        targetTile.SetOverlayType(OverlayType.Selected);
    }
}
