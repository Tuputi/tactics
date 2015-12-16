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
    public int height;
    private GameObject prefab;

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
        Debug.Log("Current rotation is " + rotation);
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
    public int height;
    public Facing rotation;

    public TileSave(TileType tiletype, int Row, int Column, int TileHeight, Facing Rotation)
    {
        tileType = tiletype;
        row = Row;
        column = Column;
        height = TileHeight;
        rotation = Rotation;
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
        return tile;
    }

    public void JsonLoad(JObject jObject)
    {
        this.tileType = jObject["TileType"].Value<TileType>();
        this.row = jObject["Row"].Value<int>();
        this.column = jObject["Column"].Value<int>();
        this.height = jObject["Height"].Value<int>();
        this.rotation = jObject["Rotation"].Value<Facing>();
    }
}
