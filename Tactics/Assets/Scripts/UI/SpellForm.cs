using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpellForm : MonoBehaviour {

    public GameObject SpellInfo;
    private Text spellName;
    private ItemInfoAreaDisplay areaInfo;
    public List<IncredientSlot> IncredientSlots;
    public GameObject Elementholder;
    Spell currentSpell;
    public MageInventory parentInventory;

    void Awake()
    {
        foreach(IncredientSlot slot in IncredientSlots)
        {
            slot.Init(IncredientSlots, this);
        }
        spellName = SpellInfo.transform.FindChild("SpellName").GetComponent<Text>();
        areaInfo = SpellInfo.transform.FindChild("AreaInfo").GetComponent<ItemInfoAreaDisplay>();
    }


    public void SelectASlot(IncredientSlot slot)
    {
        foreach(IncredientSlot s in IncredientSlots)
        {
            s.UnselectSlot();
        }
        slot.SelectSlot();
    }

    public void AddIncredient(ItemBase item)
    {
        foreach(IncredientSlot slot in IncredientSlots)
        {
            if (slot.slotSelected)
            {
                slot.AddItem(item);
                slot.UnselectSlot();
            }
        }
    }

    public ItemBase CreateASpell()
    {
        AttackBase currentAttack = TurnManager.instance.CurrentlyTakingTurn.AvailableActionDictionary[UIManager.instance.PendingActionType];

        float area = DisplaySpellArea();
        float range = CalculateAttackRange();
        List<Elements> elements = CalculateElementEffects(currentAttack);


        Spell newSpell = ScriptableObject.CreateInstance<Spell>();
        newSpell.SpellInit("TempSpell", area - currentAttack.TargetAreaSize, range - currentAttack.BasicRange, elements);
        newSpell.ItemName = Spellinterpreter();
        newSpell.ItemId = -1;
        UpdateElementDisplay(newSpell);
        currentSpell = newSpell;
        UpdateSpellDisplay();
        return newSpell;
    }

    //targetarea
    //attackrange
    //elements

    public float CalculateAttackRange()
    {
        float range = TurnManager.instance.CurrentlyTakingTurn.AvailableActionDictionary[UIManager.instance.PendingActionType].BasicRange;
        foreach (IncredientSlot slot in IncredientSlots)
        {
            if (!slot.isEmpty)
            {
                ItemBase incredient = slot.MyItem;
                range += incredient.EffectToRange;
            }
        }
        return range;
    }

    public List<Elements> CalculateElementEffects(AttackBase currentAtt)
    {
        List<Elements> tempList = new List<Elements>(currentAtt.ElementalAttributes);
        foreach (IncredientSlot slot in IncredientSlots)
        {
            if (!slot.isEmpty)
            {
                ItemBase incredient = slot.MyItem;
                foreach(Elements element in incredient.addElement)
                {
                    if (!tempList.Contains(element))
                    {
                        tempList.Add(element);
                    }
                }
            }
        }
        return tempList;
    }

    public float DisplaySpellArea()
    {
        float areaRange = TurnManager.instance.CurrentlyTakingTurn.AvailableActionDictionary[UIManager.instance.PendingActionType].TargetAreaSize;
        float tempRange = 0;
        tempRange = tempRange + areaRange;
        foreach (IncredientSlot slot in IncredientSlots)
        {
            if (!slot.isEmpty)
            {
                ItemBase incredient = slot.MyItem;
                tempRange += incredient.EffectToTArgetArea;
            }
        }

        areaInfo.SlackLights();
        areaInfo.LightUpRange(TargetAreaType.line, tempRange);
        return tempRange;
    }

    public void UpdateElementDisplay(ItemBase item)
    {
        UIManager.instance.CreateElementDisplay(item, Elementholder);
    }

    public bool AnyIncredientSlotSlected()
    {
        foreach(IncredientSlot slot in IncredientSlots)
        {
            if (slot.slotSelected)
            {
                return true;
            }
        }
        return false;
    }

    public bool AnyIncredientSlotOccupied()
    {
        foreach (IncredientSlot slot in IncredientSlots)
        {
            if (!slot.isEmpty)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateSpell()
    {
        if (AnyIncredientSlotOccupied())
        {
            CreateASpell();
            //delete this line in order not to have instance action available
           // TurnManager.instance.Action(UIManager.instance.PendingActionType, currentSpell);
        }
        else {
            SelectionScript.ClearSelection();
            TurnManager.instance.CurrentlyTakingTurn.currentItem = null;
            ClearSpellInfo();
        }
    }


    private void ClearSpellInfo()
    {
        spellName.text = "???";
        currentSpell = null;
        areaInfo.SlackLights();
    }

    public void UpdateSpellDisplay()
    {
        spellName.text = currentSpell.ItemName;
    }

    public string Spellinterpreter()
    {
        string tempName = "???";

        int FireCount = 0;
        int WaterCount = 0;
        int WindCount = 0;
        int EarthCount = 0;

        foreach (IncredientSlot slot in IncredientSlots)
        {
            if (!slot.isEmpty)
            {
                ItemBase incredient = slot.MyItem;
                foreach(Elements element in incredient.addElement)
                {
                    switch (element)
                    {
                        case Elements.Fire:
                            FireCount++;
                            break;
                        case Elements.Water:
                            WaterCount++;
                            break;
                        case Elements.Earth:
                            EarthCount++;
                            break;
                        case Elements.Wind:
                            WindCount++;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        
       
        if(FireCount == 1)
        {
            tempName = "Flame";
        }
        if (FireCount > 1)
        {
            tempName = "Fire Burst";
        }
        if (FireCount > 2)
        {
            tempName = "Fire Storm";
        }


        if (WaterCount == 1)
        {
            tempName = "Frostbite";
        } 
        if(WaterCount > 1)
        {
            tempName = "Ice Rain";
        }
        if (WaterCount > 2)
        {
            tempName = "Tidal Wave";
        }


       
        if(WindCount == 1)
        {
            tempName = "Wind Whip";
        }
        if (WindCount > 1)
        {
            tempName = "Gust of Wind";
        }
        if (WindCount > 2)
        {
            tempName = "Tornado";
        }

        
        if(EarthCount == 1)
        {
            tempName = "Throw Rock";
        }
        if (EarthCount > 1)
        {
            tempName = "Earth Rumble";
        }
        if (EarthCount > 2)
        {
            tempName = "Earth quake";
        }


        //combinations
        if(EarthCount == 2 && WindCount == 1)
        {
            tempName = "Sand storm";
        }
        if(EarthCount == 1 && WindCount == 2)
        {
            tempName = "Corrode";
        }
        if(EarthCount == 1 && WindCount == 1)
        {
            tempName = "Pebble swarm";
        }

        if(WaterCount == 2 && WindCount == 1)
        {
            tempName = "Wave";
        }
        if(WaterCount == 1 && WindCount == 2)
        {
            tempName = "Hail storm";
        }
        if(WaterCount == 1 && WindCount == 1)
        {
            tempName = "Rain";
        }
        if(WaterCount == 2 && EarthCount == 1)
        {
            tempName = "Marsh";
        }
        if(WaterCount == 1 && EarthCount == 2)
        {
            tempName = "Quicksand";
        }
        if(WaterCount == 1 && EarthCount == 1)
        {
            tempName = "Mud";
        }
        if(FireCount == 2 && WindCount == 1)
        {
            tempName = "Fire Whip";
        }
        if(FireCount == 1 && WindCount == 1)
        {
            tempName = "Spark";
        }
        if(FireCount == 2 && EarthCount == 1)
        {
            tempName = "Fireball";
        }
        if(FireCount == 1 && EarthCount == 2)
        {
            tempName = "Magma";
        }
        if(FireCount == 1 && EarthCount == 1)
        {
            tempName = "Molten";
        }
        if(FireCount == 2 && WaterCount == 1)
        {
            tempName = "Steam";
        }
        if(FireCount == 1 && WaterCount == 2)
        {
            tempName = "Fog";
        }
        if(FireCount == 1 && WaterCount == 1)
        {
            tempName = "Mist";
        }

        return tempName;
    }

    public void SelecteSpellFromCentre()
    {

        if (currentSpell == null)
        {
            return;
        }
        Debug.Log("Spell selected");

        TurnManager.instance.Action(UIManager.instance.PendingActionType, currentSpell);
    }
}
