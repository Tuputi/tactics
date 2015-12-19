using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System.Collections.Generic;

public class EditorButtonScript : MonoBehaviour {

    //references
    public InputField mapnamefield;
    public Dropdown loadMapDropdown;

    void Start()
    {
        UpdateMapList();
    }

    public void FindPathButton()
    {
        if(SelectionScript.selectedTiles.Count == 2)
        {
            List<Tile> path = Pathfinding.GetPath(SelectionScript.selectedTiles[0], SelectionScript.selectedTiles[1]);
            foreach(Tile t in path)
            {
                t.SetOverlayType(OverlayType.Selected);
            }
        }
    }

    public void RotationButton()
    {
        if (SelectionScript.selectedTiles.Count > 0)
        {
            foreach (Tile t in SelectionScript.selectedTiles)
            {
                int currentRotation = (int)t.rotation;
                if (++currentRotation > 3)
                {
                    t.SetRotation((Facing)0);
                }
                else
                {
                    t.SetRotation((Facing)(currentRotation));
                }
            }
        }
    }

    public void HigherButton()
    {

        if (SelectionScript.selectedTiles.Count > 0)
        {
            foreach (Tile t in SelectionScript.selectedTiles)
            {
                t.height += 0.5f;
                t.transform.position += new Vector3(0, +0.5f, 0);
            }
        }
    }

    public void LowerButton()
    {
        if (SelectionScript.selectedTiles.Count > 0)
        {
            foreach (Tile t in SelectionScript.selectedTiles)
            {
                t.height -= 0.5f;
                t.transform.position += new Vector3(0, -0.5f, 0);
            }
        }
    }

    public void AddObjectButton(int objectId)
    {

        if (SelectionScript.selectedTiles.Count > 0)
        {
            foreach (Tile t in SelectionScript.selectedTiles)
            {
                if (t.tileObject == null)
                {
                    t.SetTileObject(objectId);
                }
                else
                {
                    t.RemoveTileObject();
                }
            }
        }
    }

    public void RockButton()
    {
        if (SelectionScript.selectedTiles.Count > 0)
        {
            foreach (Tile t in SelectionScript.selectedTiles)
            {
                t.SetTileType(TileType.Rock);
            }
        }
    }

    public void GrassButton()
    {
        if (SelectionScript.selectedTiles.Count > 0)
        {
            foreach (Tile t in SelectionScript.selectedTiles)
            {
                t.SetTileType(TileType.Grass);
            }
        }
    }

    public void Save()
    {
        if (!mapnamefield.text.Equals(""))
        {
            MapCreator.instance.SaveMap(mapnamefield.text);
            mapnamefield.text = "";
            UpdateMapList();
        }
    }

    public void Load()
    {
        string mapname = loadMapDropdown.captionText.text;
        UpdateMapList();
        if (File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Tactics/Maps/" + mapname + ".json"))
        {
            MapCreator.instance.LoadMap(mapname);
        }
        else
        {
            Debug.Log(mapname+" not found");
        }
        
    }

    public void UpdateMapList()
    {
        loadMapDropdown.options.Clear();
        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Tactics/Maps/";
        string[] mapnames = System.IO.Directory.GetFiles(path, "*.json");

        foreach (var p in mapnames)
        {
            string name = Path.GetFileNameWithoutExtension(p);
            loadMapDropdown.options.Add(new Dropdown.OptionData(name));
        }

    }
}
