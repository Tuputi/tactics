using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditorButtonScript : MonoBehaviour {

    //references
    public InputField mapnamefield;


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
        }
    }

    public void Load()
    {
        if (!mapnamefield.text.Equals(""))
        {
            MapCreator.instance.LoadMap(mapnamefield.text);
        }
    }
}
