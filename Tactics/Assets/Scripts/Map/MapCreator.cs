using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour {


    public Tile[,] map;
    public int mapRows;
    public int mapColumns;
    public string MapName;
    public static MapCreator instance;
    public GameObject mapContainer;


    void Awake(){
        instance = this;
    }

    void Start()
    {
        mapContainer = GameObject.Find("Map").gameObject;

        if (Application.platform == RuntimePlatform.Android)
        {
            MapName = ChangeSceneScript.MapName;
        }

        if (!MapName.Equals(""))
        {
           LoadMap(MapName);
        }
        else
        {
            generateBlankMap(mapRows, mapColumns);
        }
    }

    public void SaveMap(string mapname)
    {
        SaveLoad.SaveMap(mapname, map);
    }
    
    void cleanMap()
    {
        for (int i = 0; i < mapContainer.transform.childCount; i++)
        {
            Destroy(mapContainer.transform.GetChild(i).gameObject);
        }
        GameObject characterContainer = GameObject.Find("Characters");
        for(int i = 0; i < characterContainer.transform.childCount; i++)
        {
            GameObject chara = characterContainer.transform.GetChild(i).gameObject;
            Destroy(chara);
        }
    }

    void generateBlankMap(int rows, int columns)
    {

        cleanMap();
        map = new Tile[rows,columns];
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                Tile tile = ((GameObject)Instantiate(PrefabHolder.instance.tile_base, new Vector3(x, 0, y), Quaternion.identity)).GetComponent<Tile>();
                tile.transform.SetParent(mapContainer.transform);
                tile.name = "tile" + x + "-" + y;
                tile.SetTileType(TileType.Grass);
                tile.height = 1;
                tile.xPos = x;
                tile.yPos = y;
                tile.transform.position = new Vector3(x, tile.height, y);
                map[x, y] = tile;
            }
        }
        GoThroughNeighbours();
    }

    public void LoadMap(string mapName)
    {
        cleanMap();
        Map loadedMap = SaveLoad.LoadMap(mapName);
        mapRows = loadedMap.rows;
        mapColumns = loadedMap.columns;

        map = new Tile[mapRows,mapColumns];
        for (int i = 0; i < mapRows; i++)
        {
            for (int j = 0; j < mapColumns; j++)
            {
                Tile t = ((GameObject)Instantiate(PrefabHolder.instance.tile_base, new Vector3(i, 0, j), Quaternion.identity)).GetComponent<Tile>();
                t.transform.SetParent(mapContainer.transform);
                t.name = "Tile" + i + "-" + j;
                t.SetTileType(loadedMap.MapTiles[i,j].tileType);
                t.height = loadedMap.MapTiles[i,j].height;
                t.SetRotation(loadedMap.MapTiles[i,j].rotation);
                t.SetTileObject(loadedMap.MapTiles[i,j].objectId);
                t.transform.position = new Vector3(i, t.height, j);
                CharacterLogic.CreateCharacter(loadedMap.MapTiles[i,j].characterId, t);
                t.xPos = i;
                t.yPos = j;
                map[i, j] = t;
            }
       }
       GoThroughNeighbours();
    }

    void GoThroughNeighbours()
    {
       List<Tile> tmpList = new List<Tile>();
        for (int i = 0; i < mapRows; i++)
        {
            for (int j = 0; j < mapColumns; j++)
            {
                Tile currentTile = map[i,j];
                if (i - 1 >= 0)
                {
                    if (map[i - 1,j] != null)
                    {
                        tmpList.Add(map[i - 1,j]);
                    }
                }
                if (i + 1 <= map.GetLength(0) - 1)
                {
                    if (map[i + 1,j] != null)
                    {
                        tmpList.Add(map[i + 1,j]);
                    }
                }
                if (j + 1 <= map.GetLength(0) - 1)
                {
                    if (map[i,j + 1] != null)
                    {
                        tmpList.Add(map[i,j + 1]);
                    }
                }
                if (j - 1 >= 0)
                {
                    if (map[i,j - 1] != null)
                    {
                        tmpList.Add(map[i,j - 1]);
                    }
                }
                currentTile.SetNeighbours(tmpList);
                tmpList.Clear();
            }
        }
    }
}
