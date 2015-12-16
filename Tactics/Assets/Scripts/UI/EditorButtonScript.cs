using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class EditorButtonScript : MonoBehaviour {

    //references
    public InputField mapnamefield;
    public Dropdown loadMapDropdown;

    void Start()
    {
        UpdateMapList();
    }

    public void RotationButton()
    {
        if (SelectionScript.selectedTile != null)
        {
            int currentRotation = (int)SelectionScript.selectedTile.rotation;
            if (++currentRotation > 3)
            {
                SelectionScript.selectedTile.SetRotation((Facing)0);
            }
            else
            {
                SelectionScript.selectedTile.SetRotation((Facing)(currentRotation));
            }
        }
    }

    public void HigherButton()
    {
        SelectionScript.selectedTile.height += 0.5f;
        SelectionScript.selectedTile.transform.position += new Vector3(0, +0.5f, 0);
    }

    public void LowerButton()
    {
        SelectionScript.selectedTile.height -= 0.5f;
        SelectionScript.selectedTile.transform.position += new Vector3(0, -0.5f, 0);
    }

    public void RockButton()
    {
        if (SelectionScript.selectedTile != null)
        {
            SelectionScript.selectedTile.SetTileType(TileType.Rock);
        }
    }

    public void GrassButton()
    {
        if (SelectionScript.selectedTile != null)
        {
            SelectionScript.selectedTile.SetTileType(TileType.Grass);
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
