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
        chara.ChangeFacing(chara.position, targetTile);
        if (targetTile.character.facing == chara.facing)
        {
            damage *= 2;
            Debug.Log("backstab");
        }


        Debug.Log("Did " + damage + " to " + targetTile.character.characterName);
        targetTile.character.characterHealth += damage;
        GameUI.showDamage(targetTile.character, damage);
        if (targetTile.character.characterHealth <= 0)
        {
            Debug.Log(targetTile.character + " is dead");
            targetTile.character.isAlive = false;
        }
        GameUI.UpdateStatUI(targetTile.character);
        return damage;
    }

    public override void DrawTargetArea(Tile targetTile)
    {
        targetTile.SetOverlayType(OverlayType.Path, targetTile);
    }
}
