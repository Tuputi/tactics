using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabHolder : MonoBehaviour {

    public static PrefabHolder instance;


    //Tiles
    public GameObject tile_base;
    public GameObject Tile_Grass_Prefab;
    public GameObject Tile_Rock_Prefab;
    public GameObject Tile_Empty_Prefab;

    //overlay
    public GameObject Overlay_Empty_Prefab;
    public GameObject Overlay_Selection_Prefab;


    //lists
    public List<TileObject> tileObjects;
    public List<Character> characters;

    void Awake()
    {
        instance = this;
    }
}
