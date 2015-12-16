using UnityEngine;
using System.Collections.Generic;
using System.IO;
using BonaJson;

public class SaveLoad : MonoBehaviour {


	public static void SaveMap(string MapName, List<List<Tile>> mapTiles)
    {
        Map map = new Map(MapName, mapTiles);
        var mapData = map.JsonSave();
        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        if (!Directory.Exists(path + "/Tactics/Maps/"))
        {
            System.IO.Directory.CreateDirectory(path + "/Tactics/Maps/");
        }
        File.WriteAllText(path + "/Tactics/Maps/" + map.MapName + ".json", mapData.PrettyPrint());
        Debug.Log("Saved file " + map.MapName + ".json");
    }

    public static Map LoadMap(string filename)
    {
        if (!Path.HasExtension(filename))
        {
            filename += ".json";
        }
        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        var fileContent = File.ReadAllText(path + "/Tactics/Maps/" + filename);
        var mapJson = JObject.Parse(fileContent);
        var map = new Map();
        map.JsonLoad(mapJson);
        return map;
    }
}
