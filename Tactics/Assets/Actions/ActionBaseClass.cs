using UnityEngine;
using System.Collections.Generic;

public class ActionBaseClass : ScriptableObject {

    protected string actionName = "ActionBaseClass";
    public float BasicRange = 2f;
    public float TargetAreaSize = 1;
    public float EnergyCost = 5f;
    public string AnimationName;
    public bool UsedWithItems = false;
    public List<Tile> attackArea;
    

    public virtual string GetName()
    {
        return actionName;
    }

    public virtual List<Tile> CalculateActionRange(Tile startTile)
    {
        float range = BasicRange;
        if (TurnManager.instance.CurrentlyTakingTurn.currentItem && UsedWithItems)
        {
            range += TurnManager.instance.CurrentlyTakingTurn.currentItem.GetRangeEffect();
        }
        return Pathfinding.GetPossibleRange(startTile, range, true);
    }

    public virtual List<Tile> CalculateActionRange(Tile startTile, Item ib)
    {
       float range = BasicRange + ib.GetRangeEffect();
       return Pathfinding.GetPossibleRange(startTile, range, true);
    }

    public virtual List<Tile> DrawTargetArea(Tile targetTile)
    {
        List<Tile> temp = new List<Tile>();
        temp.Add(targetTile);
        return temp;
    }

    public virtual void PlayAnimation(Character chara)
    {
        chara.GetComponent<Animator>().Play(AnimationName);
    }

    public virtual void CompleteAction(Tile TargetTile)
    {
        CalculateEffect(TargetTile);
    }

    public virtual int CalculateEffect(Tile targetTile)
    {
        int damage = 10;
        return damage;
    }

    public virtual int GetHitChance(Tile targetTile)
    {
        int chance = 100;
        return chance;
    }

   
}
