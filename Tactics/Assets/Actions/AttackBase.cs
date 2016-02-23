using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[CreateAssetMenu]
public class AttackBase : ActionBaseClass{

    public string AttackName;
    public int minDamage = 0;
    public int maxDamage = 0;
    public int MPCost = 0;
    public int hitChance;
    public ActionType actionType = ActionType.MeeleeAttack;
    public List<ItemType> compatibleItems;



    public virtual ActionType GetActionType()
    {
        return actionType;
    }

    public override string GetName()
    {
        return AttackName;
    }

    public virtual bool CompatibleItem(ItemBase ib)
    {
        return true;
    }

    public override List<Tile> CalculateActionRange(Tile startTile)
    {
        return base.CalculateActionRange(startTile);
    }

    public override List<Tile> CalculateActionRange(Tile startTile, ItemBase ib)
    {
        return base.CalculateActionRange(startTile, ib);
    }

    public override List<Tile> DrawTargetArea(Tile targetTile)
    {
        List<Tile> temp = new List<Tile>();
        temp.Add(targetTile);

        if (TurnManager.instance.CurrentlyTakingTurn.currentItem)
        {
            ItemBase ib = TurnManager.instance.CurrentlyTakingTurn.currentItem;
            if (ib.EffectToTArgetArea > 0)
            {
                List<Tile> tempList = Pathfinding.GetPossibleRange(targetTile, ib.EffectToTArgetArea, true);
                foreach (Tile t in tempList)
                {
                    temp.Add(t);
                }
            }
        }
        attackArea = temp;
        return temp;
    }

    public override int CalculateEffect(Tile targetTile)
    {
        bool foundTarget = false;
        foreach (Tile t in attackArea)
        {
            if (t.isOccupied)
            {
                foundTarget = true;
                break;
            }
        }
        if (!foundTarget)
        {
            return 0;
        }

        Character chara = TurnManager.instance.CurrentlyTakingTurn;

        foreach (Tile t in attackArea)
        {
            if (t.isOccupied)
            {
                int damage = Random.Range(minDamage, maxDamage);
                damage *= -1;
                if (t.tileCharacter.facing == chara.facing)
                {
                    damage *= 2;
                    Debug.Log("backstab");
                }
                if (System.Math.Abs(t.height - chara.characterPosition.height) > 1 && chara.characterPosition.height > t.height)
                {
                    damage *= 2;
                    Debug.Log("heightAdvantage");
                }
                Debug.Log("Did " + damage + " to " + t.tileCharacter.characterName);
                t.tileCharacter.Hp += damage;
                CharacterLogic.instance.DisplayEffect(t.tileCharacter, damage);
            }
        }
        attackArea.Clear();
        return 1;
    }

    public override int GetHitChance(Tile targetTile)
    {
        float tempHitChange = hitChance;
        foreach (Tile t in attackArea)
        {
            if (t.isOccupied)
            {
                if (TurnManager.instance.CurrentlyTakingTurn.facing != t.tileCharacter.facing)
                {
                    tempHitChange -= tempHitChange * 0.3f;
                }
            }
        }
        return (int)tempHitChange;
    }
}
