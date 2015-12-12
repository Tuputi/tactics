using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour {


    public List<List<Tile>> map = new List<List<Tile>>();
    public int mapRows;
    public int mapColumns;
    public static GameObject mapContainer;

    void Start()
    {

        mapContainer = this.transform.FindChild("Map").gameObject;
        generateBlankMap(mapRows, mapColumns);
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
                //tile.SetType(TileType.Grass);
                tile.transform.position = new Vector3(i, 1, j);
                row.Add(tile);
            }
            map.Add(row);
        }
        GoThroughNeighbours();
    }


    void GoThroughNeighbours()
    {
        List<Tile> tmpList = new List<Tile>();
        for (int i = 0; i <= mapRows; i++)
        {
            for (int j = 0; j <= mapColumns; j++)
            {
                Tile currentTile = map[i][j];
                if (i - 1 >= 0)
                {
                    if (!(map[i - 1][j] == null))
                    {
                        tmpList.Add(map[i - 1][j]);
                    }
                }
                if (i + 1 <= map.Count - 1)
                {
                    if (!(map[i + 1][j] == null))
                    {
                        tmpList.Add(map[i + 1][j]);
                    }
                }
                if (j + 1 <= map[i].Count - 1)
                {
                    if (!(map[i][j + 1] == null))
                    {
                        tmpList.Add(map[i][j + 1]);
                    }
                }
                if (j - 1 >= 0)
                {
                    if (!(map[i][j - 1] == null))
                    {
                        tmpList.Add(map[i][j - 1]);
                    }
                }
                currentTile.SetNeighbours(tmpList);
                tmpList.Clear();
            }
        }
    }
}
