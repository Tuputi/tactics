using UnityEngine;
using System.Collections.Generic;

public class Howl : AttackBase {

    public override ActionType GetActionType()
    {
        actionType = ActionType.ShootArrow;
        return actionType;
    }

    public override string GetName()
    {
        actionName = "Howl";
        return actionName;
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
                int randoR = Random.Range(1, 2);
                int damageR = randoR * -10;
                if (t.tileCharacter.facing == chara.facing)
                {
                    damageR *= 2;
                    Debug.Log("backstab");
                }
                if (System.Math.Abs(t.height - chara.characterPosition.height) > 1 && chara.characterPosition.height > t.height)
                {
                    damageR *= 2;
                    Debug.Log("heightAdvantage");
                }
                Debug.Log("Did " + damageR + " to " + t.tileCharacter.characterName);
                t.tileCharacter.Hp += damageR;
                //t.tileCharacter.DisplayEffect(damageR, DisplayTexts.none);
            }
        }
        attackArea.Clear();
        return 1;
    }


    public override List<Tile> CalculateActionRange(Tile startTile)
    {
        float range = 3f;
        return Pathfinding.GetPossibleRange(startTile, range, true);
    }

    public override List<Tile> DrawTargetArea(Tile targetTile)
    {
        List<Tile> temp = new List<Tile>();
        temp.Add(targetTile);
        attackArea = temp;
        return temp;
    }

    public override void CompleteAction(Tile TargetTile)
    {
        CalculateEffect(TargetTile);
    }

    public override bool CompatibleItem(ItemBase ib)
    {
        foreach (ItemType it in compatibleItems)
        {
            if (ib.itemCategories.Contains(it))
            {
                return true;
            }
        }

        return false;
    }
}
