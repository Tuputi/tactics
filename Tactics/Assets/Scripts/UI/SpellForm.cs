using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpellForm : MonoBehaviour {

    public GameObject SpellInfo;
    private Text spellName;
    private Image spellImage;


    public List<IncredientSlot> IncredientSlots;

    void Awake()
    {
        foreach(IncredientSlot slot in IncredientSlots)
        {
            slot.Init(IncredientSlots);
        }
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



}
