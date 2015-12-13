using UnityEngine;
using System.Collections;

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


    void Awake()
    {
        instance = this;
    }
}
