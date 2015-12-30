using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BonaJson;
using System;

public class Map {

    public string MapName;
    public int rows;
    public int columns;
    public TileSave[,] MapTiles;

    public Map(string name, Tile[,] mapTiles)
    {
        MapName = name;
        rows = mapTiles.GetLength(0);
        columns = mapTiles.GetLength(1);
        MapTiles = new TileSave[rows, columns];
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                TileSave ts = new TileSave(mapTiles[i,j].tileType, i, j, mapTiles[i,j].height, mapTiles[i,j].rotation, mapTiles[i,j].tileObjectId, mapTiles[i,j].tileCharacter);
                MapTiles[i, j] = ts;
            }
        }
    }

    public Map(int mrows,int mcolumns)
    {
        MapTiles = new TileSave[mrows,mcolumns];
    }

    public JObject JsonSave()
    {
        var map = new JObjectCollection();
        map.Add("MapName", MapName);

        var tileJObject = new JObjectArray();
        map.Add("Tiles",tileJObject);

        rows = MapTiles.GetLength(0);
        columns = MapTiles.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                tileJObject.Add(MapTiles[i,j].JsonSave(i,j));
            }
        }
        map.Add("Rows", rows);
        map.Add("Columns", columns);
        return map;
    }

    public void JsonLoad(JObject jObject)
    {
        this.MapName = jObject["MapName"].Value<string>();
        this.rows = (int)jObject["Rows"].Value<Int64>();
        this.columns = (int)jObject["Columns"].Value<Int64>();

        MapTiles = new TileSave[this.rows, this.columns]; 

        foreach(var tile in jObject["Tiles"])
        {
            TileSave newTile = new TileSave();
            newTile.JsonLoad(tile);
            MapTiles[newTile.row, newTile.column] = newTile;
        }
    }


}
