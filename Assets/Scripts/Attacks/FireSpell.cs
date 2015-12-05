using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireSpell : AttackBase {

    public override List<Tile> CalculateAttackRange(Tile startTile)
    {
        return PathFinding.GetAttackRange(startTile, 4f);
    }

    public override int CalculateDamage(Tile targetTile, Character chara)
    {
        List<Tile> hitsOnTiles = new List<Tile>(targetTile.neighbours);
        hitsOnTiles.Add(targetTile);
        int damageA = 0;
        foreach (Tile t in hitsOnTiles)
        {
            t.SetOverlayType(OverlayType.Attack, t);
            int randoA = Random.Range(1, 3);
            damageA = randoA * -2;
            if (t.isOccupied)
            {
                Debug.Log("Did " + damageA + " to " + t.character.characterName);
                t.character.characterHealth += damageA;
                GameUI.showDamage(t.character, damageA);
                GameUI.UpdateStatUI(t.character);
                if (t.character.characterHealth <= 0)
                {
                    Debug.Log(t.character + " is dead");
                    t.character.isAlive = false;
//                    t.character.gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
                }
                
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
            t.SetOverlayType(OverlayType.Path, t);
        }

    }
}
