using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackBase : MonoBehaviour {

    public double attackBaseProbability; //100 = 100%
    public string attackName;
    public int likelyDamageLow = 0;
    public int likelyDamageHigh = 0;
    public AttackType attackType;

	public virtual bool CalculateHitChance()
    {
        //true if hit and false if miss
        double chance = Random.Range(20, 30);
        if(chance+attackBaseProbability > 100)
        {
            return true;
        }
        return false;
    }

    public virtual int CalculateDamage(Tile t, Character chara)
    {
        int damage = 10;
        return damage;
    }

    public virtual List<Tile> CalculateAttackRange(Tile startTile)
    {
        return PathFinding.GetAttackRange(startTile, attackType);
    }

    public virtual void DrawTargetArea(Tile targetTile)
    {
        targetTile.SetOverlayType(OverlayType.Path, targetTile);
    }

    public virtual void PlayAnimation(Character chara)
    {
        chara.GetComponent<Animator>().Play("ShootArrow");
    }

}
