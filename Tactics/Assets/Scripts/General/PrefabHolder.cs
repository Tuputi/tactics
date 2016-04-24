using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabHolder : MonoBehaviour {

    public static PrefabHolder instance;

    //ui-elements
    [Header("UI-elemnts")]
    public GameObject DamageText;
    public List<ButtonScript> basicActions;
    public GameObject ConfirmationTemplate;

    //Tiles
    [Header("Tiles")]
    public GameObject tile_base;
    public GameObject Tile_Grass_Prefab;
    public GameObject Tile_Rock_Prefab;
    public GameObject Tile_Water_Prefab;
    public GameObject Tile_Flower_Prefab;
    public GameObject Tile_Empty_Prefab;

    //overlay
    [Header("Overlays")]
    public GameObject Overlay_Empty_Prefab;
    public GameObject Overlay_Selection_Prefab;
    public GameObject Overlay_Arrow_Prefab;
    public GameObject Overlay_Attack_Prefab;
    public GameObject Overlay_inTurn_Prefab;


    //lists
    [Header("Lists")]
    public List<TileObject> tileObjects;
    public List<Character> characters;

    [Header("Misc")]
    public GameObject Bow;


    void Awake()
    {
        instance = this;
    }
}


//enums

public enum Facing { Up, Right, Down, Left };
public enum TileType { None, Grass, Rock, Water, Flower };
public enum OverlayType { None, Selected, Arrow, inTurn, Attack };
public enum BehaviourType { agressive, teamPlayer, healWhenHurt}
public enum ConfirmationType { action, move};
public enum GameMode { Editor, Game };
public enum TargetAreaType { none, self, circular, croshair, line}

public enum InventoryType { archer, mage}

public enum Elements { Fire, Water, Earth, Wind, None}
public enum Resistance { Normal, Absorb, Immune, Resistant, Weak} //No change, damage = hp, no damage, 50% damage, 150% damage
public enum DisplayTexts { none, miss, immune};
public enum NameType { Rat, Animal};

//Names of different actions, used to reference to a dictionary of Actions
public enum ActionType { MeeleeAttack, FireSpell, ShootArrow, Howl, Stab, CastSpell, Bite}

public enum ItemType { None, Potion, Bomb, Arrow, Spell, Metal, Wood};