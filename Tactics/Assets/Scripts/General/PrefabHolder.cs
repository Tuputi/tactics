using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabHolder : MonoBehaviour {

    public static PrefabHolder instance;

    //ui-elements
    [Header("UI-elemnts")]
    public GameObject DamageText;

    //Tiles
    [Header("Tiles")]
    public GameObject tile_base;
    public GameObject Tile_Grass_Prefab;
    public GameObject Tile_Rock_Prefab;
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
    public Dictionary<ItemType, ItemBase> itemDictionary;


    //sprite fix't
    [Header("Sprites")]
    public Sprite PotionSprite;


    void Awake()
    {
        instance = this;
        actionDictionary = new Dictionary<ActionType, AttackBase>();
        itemDictionary = new Dictionary<ItemType, ItemBase>();
        foreach(AttackBase ab in actions)
        {
            actionDictionary.Add(ab.GetActionType(), ab);
        }
        foreach (var item in items)
        {
            itemDictionary.Add(item.GetItemType(),item);
        }
    }
}


//enums

public enum Facing { Up, Right, Down, Left };
public enum TileType { None, Grass, Rock };
public enum OverlayType { None, Selected, Arrow };
public enum BehaviourType { agressive, teamPlayer, healWhenHurt}
public enum ConfirmationType { action, move};
public enum GameMode { Editor, Game };

//Names of different actions, used to reference to a dictionary of Actions
public enum ActionType { MeeleeAttack, FireSpell, ShootArrow}

public enum ItemType { Potion, Bomb};