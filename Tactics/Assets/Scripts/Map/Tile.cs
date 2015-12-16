using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BonaJson;


//enums
public enum Facing { Up, Right, Down, Left };
public enum TileType { None, Grass, Rock };
public enum OverlayType { None, Selected};

public class Tile : MonoBehaviour
{

    //lists
    public Dictionary<Facing, Tile> neighbours;


    //tileType/visuals
    public TileType tileType = TileType.Grass;
    public Facing rotation = Facing.Up;
    public float height;
    private GameObject prefab;
    public GameObject tileObject;
    public int tileObjectId;

    public OverlayType overlayType = OverlayType.None;
    private GameObject overlayPrefab;


    public Tile()
    {
        tileType = TileType.Grass;
    }

    public void SetNeighbours(Dictionary<Facing, Tile> tiles)
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

    public void SetRotation(Facing rotate)
    {
        rotation = rotate;
        int rotationInt = -1;
        switch (rotation)
        {
            case Facing.Up:
                rotationInt = 0;
                break;
            case Facing.Right:
                rotationInt = 270;
                break;
            case Facing.Down:
                rotationInt = 180;
                break;
            case Facing.Left:
                rotationInt = 90;
                break;
            default:
                break;
        }
        gameObject.transform.rotation = Quaternion.Euler(0, rotationInt, 0);
    }

    public void SetOverlayType(OverlayType type)
    {
        overlayType = type;
        switch (type)
        {
            case OverlayType.None:
                overlayPrefab = PrefabHolder.instance.Overlay_Empty_Prefab;
                break;
            case OverlayType.Selected:
                overlayPrefab = PrefabHolder.instance.Overlay_Selection_Prefab;
                break;
            default:
                break;
        }
        GenerateOverlay();
    }

    public void SetTileObject(int objectID)
    {
        foreach(TileObject to in PrefabHolder.instance.tileObjects)
        {
            if(to.id == objectID)
            {
                
                tileObjectId = objectID;
                Vector3 pos = to.gameObject.transform.position;
                pos += this.transform.position;
                GameObject go = (GameObject)Instantiate(to.gameObject, pos, Quaternion.identity);
                tileObject = go;
                go.transform.SetParent(this.gameObject.transform);
                return;
            }
        }
    }

    public void RemoveTileObject()
    {
        if (tileObject != null)
        {
            Destroy(tileObject);
            tileObjectId = 0;
        }
    }

    public void GenerateVisuals()
    {
        GameObject container = transform.FindChild("Visuals").gameObject;

        for (int i = 0; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }
        GameObject newVisual = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
        newVisual.transform.parent = container.transform;
    }

    public void GenerateOverlay()
    {
        GameObject container = transform.FindChild("Overlay").gameObject;

        for (int i = 0; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }
        GameObject newVisual = (GameObject)Instantiate(overlayPrefab, transform.position + new Vector3(0,1,0), Quaternion.identity);
        newVisual.transform.parent = container.transform;
    }


    void OnMouseDown()
    {
        SelectionScript.SetSelectedTile(this);
    }

}

public class TileSave
{

    public TileType tileType;
    public int row;
    public int column;
    public float height;
    public Facing rotation;
    public int objectId;

    public TileSave(TileType tiletype, int Row, int Column, float TileHeight, Facing Rotation, int ObjectId)
    {
        tileType = tiletype;
        row = Row;
        column = Column;
        height = TileHeight;
        rotation = Rotation;
        objectId = ObjectId;
    }

    public TileSave()
    {

    }
   
    public JObject JsonSave(int row, int column)
    {
        var tile = new JObjectCollection();
        tile.Add("TileType", (int)tileType);
        tile.Add("Row", row);
        tile.Add("Column", column);
        tile.Add("Height", height);
        tile.Add("Rotation", (int)rotation);
        tile.Add("ObjectId", objectId);
        return tile;
    }

    public void JsonLoad(JObject jObject)
    {
        this.tileType = jObject["TileType"].Value<TileType>();
        this.row = jObject["Row"].Value<int>();
        this.column = jObject["Column"].Value<int>();
        this.height = jObject["Height"].GetAsFloat();
        this.rotation = jObject["Rotation"].Value<Facing>();
        this.objectId = jObject["ObjectId"].Value<int>();
    }
}
