using UnityEngine;
using System.Collections;
using System.Collections.Generic;



//enums
public enum Facing { Up, Right, Down, Left };
public enum TileType { None, Grass, Rock };

public class Tile : MonoBehaviour {

    //lists
    public Dictionary<Facing,Tile> neighbours;

    public TileType tileType = TileType.Grass;
    private GameObject prefab;



    public void SetNeighbours(Dictionary<Facing,Tile> tiles)
    {
        neighbours = new Dictionary<Facing, Tile>(tiles);
    }

    public void SetTileType(TileType type)
    {
        tileType = type;
        switch (type)
        {
            case TileType.None:
                prefab = PrefabHolder.instance.Tile_Empty_Prefab;
                break;
            case TileType.Grass:
                prefab = PrefabHolder.instance.Tile_Grass_Prefab;
                break;
            case TileType.Rock:
                prefab = PrefabHolder.instance.Tile_Rock_Prefab;
                break;
            default:
                break;
        }
        GenerateVisuals();
    }

    public void GenerateVisuals()
    {
        GameObject container = transform.FindChild("Visuals").gameObject;
        //remove children
        for (int i = 0; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }
        GameObject newVisual = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
        newVisual.transform.parent = container.transform;
    }


}
