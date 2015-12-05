using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;





public class TileXml{
	[XmlAttribute("id")]
	 public int id;

	[XmlAttribute("locX")]
	public int locX;

	[XmlAttribute ("locY")]
	public int locY;

	[XmlAttribute("height")]
	public float height;

    [XmlAttribute("tileStatus")]
    public int tileStat;

    [XmlAttribute("tileCharacter")]
    public int characterID;

    [XmlAttribute("tileObject")]
    public int objectID;

    [XmlAttribute("tileCharacterFacing")]
    public int characterFacing;
}

[XmlRoot("MapCollection")]
public class MapXmlContainer{
	[XmlAttribute("rows")]
	public int rows;

	[XmlAttribute("columns")]
	public int columns;

	[XmlArray("Tiles")]
	[XmlArrayItem("Tile")]
	public List<TileXml> tiles = new List<TileXml> ();
}

public static class MapSaveLoad{
	public static MapXmlContainer CreateMapContainer(List<List<Tile>> map){

		List<TileXml> tiles = new List<TileXml>();
		for (int i = 0; i <map.Count; i++) {
			for(int j = 0; j <map[i].Count;j++){
				tiles.Add(MapSaveLoad.CreateTileXml(map[i][j]));
			}
		}

		return new MapXmlContainer(){
			rows = map.Count,
			columns = map[0].Count,
			tiles = tiles
		};
	}

	public static TileXml CreateTileXml(Tile tile){
        return new TileXml {
            id = (int)tile.tileType,
            locX = tile.positionRow,
            locY = tile.positionColumn,
            height = tile.height,
            tileStat = tile.tileStat,
            characterID = tile.charaId,
            objectID = tile.objectId,
            characterFacing = (int)tile.characterFacing
		};
	}

	public static void Save(MapXmlContainer mapContainer, string filename){
        if (!File.Exists(Application.persistentDataPath + "/Maps/" + filename))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Maps/");
        }
        var encoding = System.Text.Encoding.GetEncoding("UTF-8");
        Debug.Log (Application.persistentDataPath + "/Maps/"+filename);
		var serializer = new XmlSerializer (typeof(MapXmlContainer));
		using (StreamWriter stream = new StreamWriter(Application.persistentDataPath + "/Maps/"+filename, false, encoding)) {
			serializer.Serialize(stream, mapContainer);
		}
	}


    public static MapXmlContainer Load(string filename)
    {
        TextAsset ta = Resources.Load(filename) as TextAsset;
        Debug.Log(ta);
        var serializer = new XmlSerializer(typeof(MapXmlContainer));
        using (var reader = new System.IO.StringReader(ta.text))
        {
            return serializer.Deserialize(reader) as MapXmlContainer;
        }

    }

 }
