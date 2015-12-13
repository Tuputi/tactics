using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BonaJson;

public class Map {

    public string MapName;
    public int rows;
    public int columns;
    public List<List<TileSave>> MapTiles = new List<List<TileSave>>();

    public Map(string name, List<List<Tile>> mapTiles)
    {
        MapName = name;
        for(int i = 0; i <= rows; i++)
        {
            List<TileSave> TileRow = new List<TileSave>();
            for(int j = 0; j <= columns; j++)
            {
                Debug.Log(i + "-" + j);
                TileSave ts = new TileSave(mapTiles[i][j].tileType, i, j);
                TileRow.Add(ts);
            }
            MapTiles.Add(TileRow);
        }
    }

    public Map()
    {
        MapTiles = new List<List<TileSave>>();
    }

    public JObject JsonSave()
    {
        var map = new JObjectCollection();
        map.Add("MapName", MapName);

        var tileJObject = new JObjectArray();
        map.Add("Tiles",tileJObject);

        rows = MapTiles.Count;
        for(int i = 0; i < rows; i++)
        {
            List<TileSave> column = new List<TileSave>(MapTiles[i]);
            columns = column.Count;
            for(int j = 0; j < columns; j++)
            {
                tileJObject.Add(column[j].JsonSave(i,j));
            }
        }
        map.Add("Rows", rows);
        map.Add("Columns", columns);
        return map;
    }

    public void JsonLoad(JObject jObject)
    {
        this.MapName = jObject["MapName"].Value<string>();
        //this.rows = jObject["Rows"].Value<int>();
        //this.rows = jObject["Columns"].Value<int>();
        for (int i = 0; i <= rows; i++)
        {
            MapTiles.Add(new List<TileSave>());
        }

        foreach(var tile in jObject["Tiles"])
        {
            TileSave newTile = new TileSave();
            newTile.JsonLoad(tile);
            MapTiles[newTile.row].Add(newTile);
        }
    }


}
