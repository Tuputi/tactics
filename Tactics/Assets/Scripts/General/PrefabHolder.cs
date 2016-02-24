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
    public GameObject Tile_Empty_Prefab;

    //overlay
    [Header("Overlays")]
    public GameObject Overlay_Empty_Prefab;
    public GameObject Overlay_Selection_Prefab;
    public GameObject Overlay_Arrow_Prefab;


    //lists
    [Header("Lists")]
    public List<TileObject> tileObjects;
    public List<Character> characters;
    public List<AttackBase> actions;
    public List<ItemBase> items;

    //dictionaries
    [HideInInspector]
    public Dictionary<ActionType, AttackBase> actionDictionary;
    //public Dictionary<ItemType, ItemBase> itemDictionary;

    [Header("Misc")]
    public GameObject Bow;


    void Awake()
    {
        instance = this;
        actionDictionary = new Dictionary<ActionType, AttackBase>();
        //itemDictionary = new Dictionary<ItemType, ItemBase>();
        foreach(AttackBase ab in actions)
        {
            actionDictionary.Add(ab.GetActionType(), ab);
        }
        /*foreach (var item in ItemList.)
        {
            itemDictionary.Add(item.GetItemType(),item);
        }*/
    }
}


//enums

public enum Facing { Up, Right, Down, Left };
public enum TileType { None, Grass, Rock, Water };
public enum OverlayType { None, Selected, Arrow };
public enum BehaviourType { agressive, teamPlayer, healWhenHurt}
public enum ConfirmationType { action, move};
public enum GameMode { Editor, Game };
public enum TargetAreaType { none, self, circular, line}
public enum Elements { Fire, Water, Earth, Wind}

//Names of different actions, used to reference to a dictionary of Actions
public enum ActionType { MeeleeAttack, FireSpell, ShootArrow, Howl}

public enum ItemType { Potion, Bomb, Arrow, ArrowWideHit};