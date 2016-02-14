using UnityEngine;
using System.Collections.Generic;
using System.IO;
using BonaJson;

public class SaveLoad : MonoBehaviour {


	public static void SaveMap(string MapName, Tile[,] mapTiles)
    {
        Map map = new Map(MapName, mapTiles);
        var mapData = map.JsonSave();
        string path = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath;
        }
        else
        {
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        }
        
        if (!Directory.Exists(path + "/Tactics/Maps/"))
        {
            System.IO.Directory.CreateDirectory(path + "/Tactics/Maps/");
        }
        File.WriteAllText(path + "/Tactics/Maps/" + map.MapName + ".json", mapData.PrettyPrint());
        Debug.Log("Saved file " + map.MapName + ".json at location "+ path + "/Tactics/Maps/");
    }

    public static Map LoadMap(string filename)
    {
        if (!Path.HasExtension(filename))
        {
            filename += ".json";
        }
        string path = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            //path = Application.persistentDataPath;
            var file = Resources.Load(filename) as TextAsset;
            var mJson = JObject.Parse(file.text);
            var m = new Map(0, 0);
            m.JsonLoad(mJson);
            return m;
        }
        else
        {
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        }
        var fileContent = File.ReadAllText(path + "/Tactics/Maps/" + filename);
        var mapJson = JObject.Parse(fileContent);
        var map = new Map(0, 0);
        map.JsonLoad(mapJson);
        return map;
    }
}
