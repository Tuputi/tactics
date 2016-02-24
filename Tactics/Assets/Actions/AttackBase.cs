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
    public List<Elements> ElementalAttributes;

    private DisplayTexts displayText = DisplayTexts.none;



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
                    if (!temp.Contains(t))
                    {
                        temp.Add(t);
                    }
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
               /* if(Random.Range(1,100) < GetHitChance(t))
                {
                    Debug.Log("Missed this character");
                   t.tileCharacter.DisplayEffect(0, DisplayTexts.miss);
                }
                else
                {*/
                    //base damage
                    int damage = Random.Range(minDamage, maxDamage);
                    damage *= -1;

                    //back attack
                    if (t.tileCharacter.facing == chara.facing)
                    {
                        damage *= 2;
                        Debug.Log("backstab");
                    }
                    
                    //height extra
                    if (System.Math.Abs(t.height - chara.characterPosition.height) > 1 && chara.characterPosition.height > t.height)
                    {
                        damage *= 2;
                        Debug.Log("heightAdvantage");
                    }

                    if (TurnManager.instance.CurrentlyTakingTurn.currentItem != null)
                    {
                        if(ElementalAttributes == null)
                        {
                            ElementalAttributes = new List<Elements>();
                        }
                        foreach(Elements element in TurnManager.instance.CurrentlyTakingTurn.currentItem.addElement)
                        {
                            if (!ElementalAttributes.Contains(element))
                            {
                                ElementalAttributes.Add(element);
                                Debug.Log("Added " + element);
                            }
                        }
                    }

                    //elemental effects
                    int finalDamage = GetElementalEffects(t.tileCharacter, damage);

                    if (finalDamage == 0 && !(displayText == DisplayTexts.immune))
                    {
                        displayText = DisplayTexts.miss;
                    }

                    Debug.Log("Did " + finalDamage + " to " + t.tileCharacter.characterName);
                    t.tileCharacter.Hp += finalDamage;
                    t.tileCharacter.DisplayEffect(finalDamage, displayText);
                    displayText = DisplayTexts.none;
                }

           // }
        }
        attackArea.Clear();
        return 1;
    }

    private int GetElementalEffects(Character TargetCharacter, float damage)
    {
        bool absorb = false;
        float tempDamage = damage;
        foreach (Elements element in ElementalAttributes)
        {
            Resistance res = TargetCharacter.elementalResistances[element];
            switch (res)
            {
                case Resistance.Normal:
                    break;
                case Resistance.Absorb:
                    absorb = true;
                    break;
                case Resistance.Immune:
                    tempDamage = 0;
                    displayText = DisplayTexts.immune;
                    break;
                case Resistance.Resistant:
                    tempDamage *= 0.5f;
                    break;
                case Resistance.Weak:
                    tempDamage *= 1.5f;
                    Debug.Log("Enemy is weak against " + element);
                    break;
                default:
                    break;
            }
        }

        if (absorb)
        {
            damage = Mathf.Abs(tempDamage);
            return (int)damage;
        }

        return (int)tempDamage;
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
                    //tempHitChange -= tempHitChange * 0.1f;
                }
            }
        }
        return (int)tempHitChange;
    }
}
