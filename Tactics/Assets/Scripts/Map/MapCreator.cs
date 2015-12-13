using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour {


    public List<List<Tile>> map;
    public int mapRows;
    public int mapColumns;
    public string MapName;
    GameObject mapContainer;

    void Start()
    {
        mapContainer = GameObject.Find("Map").gameObject;
        if (!MapName.Equals(""))
        {
            LoadMap(MapName);
        }
        else
        {
            generateBlankMap(mapRows, mapColumns);
        }
        SaveLoad.SaveMap("TestMap", map);
    }

    void generateBlankMap(int rows, int columns)
    {

        for (int i = 0; i < mapContainer.transform.childCount; i++)
        {
            Destroy(mapContainer.transform.GetChild(i).gameObject);
        }


        map = new List<List<Tile>>();
        for (int i = 0; i <= rows; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j <= columns; j++)
            {
                Tile tile = ((GameObject)Instantiate(PrefabHolder.instance.tile_base, new Vector3(i, 0, j), Quaternion.identity)).GetComponent<Tile>();
                tile.transform.SetParent(mapContainer.transform);
                tile.name = "tile" + i + "-" + j;
                tile.SetTileType(TileType.Grass);
                tile.transform.position = new Vector3(i, 1, j);
                row.Add(tile);
            }
            map.Add(row);
        }
        GoThroughNeighbours();
    }

    void LoadMap(string mapName)
    {
        
        Map loadedMap = SaveLoad.LoadMap(MapName);
        mapRows = loadedMap.rows-1;
        mapColumns = loadedMap.columns-1;

        map = new List<List<Tile>>();
        for (int i = 0; i <= mapRows; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j <= mapColumns; j++)
            {
                Tile t = ((GameObject)Instantiate(PrefabHolder.instance.tile_base, new Vector3(i, 0, j), Quaternion.identity)).GetComponent<Tile>();
                t.transform.SetParent(mapContainer.transform);
                t.name = "Tile" + i + "-" + j;
                t.SetTileType(loadedMap.MapTiles[i][j].tileType);
                t.transform.position = new Vector3(i, 1, j);
                row.Add(t);
            }
            map.Add(row);
       }
       GoThroughNeighbours();
    }

    void GoThroughNeighbours()
    {
        Dictionary<Facing, Tile> tmpList = new Dictionary<Facing, Tile>();
        for (int i = 0; i <= mapRows; i++)
        {
            for (int j = 0; j <= mapColumns; j++)
            {
                Tile currentTile = map[i][j];
                if (i - 1 >= 0)
                {
                   tmpList.Add(Facing.Up,map[i - 1][j]);
                }
                if (i + 1 <= map.Count - 1)
                {
                    tmpList.Add(Facing.Down, map[i + 1][j]);
                }
                if (j + 1 <= map[i].Count - 1)
                {
                    tmpList.Add(Facing.Right, map[i][j + 1]);
                }
                if (j - 1 >= 0)
                {
                    tmpList.Add(Facing.Left, map[i][j - 1]);
                }
                currentTile.SetNeighbours(tmpList);
                tmpList.Clear();
            }
        }
    }
}
