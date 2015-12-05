using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabHolder : MonoBehaviour {
	public static PrefabHolder instance;
	public GameObject tile_base;
	
	public GameObject Tile_Grass_Prefab;
	public GameObject Tile_Rock_Prefab;
	public GameObject Tile_Sand_Prefab;
	public GameObject Tile_Path_Prefab;
    public GameObject Tile_Empty_Prefab;
    public GameObject Tile_Inpassable_Prefab;

  
    public GameObject Overlay_Path_Prefab;
    public GameObject Overlay_Attack_Prefab;
    public GameObject Overlay_Empty_Prefab;

    public GameObject marker_prefab;

    public GameObject character_base;
    public GameObject AI_character_base;

    public GameObject gameCamera;

    public GameObject damagePrefab;

    [Tooltip("Script will search for each individual ID from these characters")]
    [Header ("All Characters")]
    public List<Character> characters;

    public List<MapObject> MapObjects;



    void Awake() {
		instance = this;
	}
}
