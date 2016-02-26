using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpellForm : MonoBehaviour {

    public GameObject SpellInfo;
    private Text spellName;
    private ItemInfoAreaDisplay areaInfo;
    public List<IncredientSlot> IncredientSlots;
    public GameObject Elementholder;

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
        float area = DisplaySpellArea();
        float range = CalculateAttackRange();
        List<Elements> elements = CalculateElementEffects();

        AttackBase currentAttack = TurnManager.instance.CurrentlyTakingTurn.AvailableActionDictionary[UIManager.instance.PendingActionType];

        Spell newSpell = ScriptableObject.CreateInstance<Spell>();
        newSpell.SpellInit("TempSpell", area - currentAttack.TargetAreaSize, range - currentAttack.BasicRange, elements);

        UpdateElementDisplay(newSpell);
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

    public List<Elements> CalculateElementEffects()
    {
        List<Elements> tempList = TurnManager.instance.CurrentlyTakingTurn.AvailableActionDictionary[UIManager.instance.PendingActionType].ElementalAttributes;
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
        foreach (IncredientSlot slot in IncredientSlots)
        {
            if (!slot.isEmpty)
            {
                ItemBase incredient = slot.MyItem;
                areaRange += incredient.EffectToTArgetArea;
            }
        }

        areaInfo.SlackLights();
        areaInfo.LightUpRange(TargetAreaType.line, areaRange);
        return areaRange;
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
            ItemBase currentSpell = CreateASpell();
            Debug.Log(currentSpell.EffectToRange);
            TurnManager.instance.Action(UIManager.instance.PendingActionType, currentSpell);
        }
        else {
            SelectionScript.ClearSelection();
            TurnManager.instance.CurrentlyTakingTurn.currentItem = null;
        }
    }
}
