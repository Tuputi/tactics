using UnityEngine;
using System.Collections;

public class SelectionScript : MonoBehaviour {

    public static Tile selectedTile = null;


    public static void SetSelectedTile(Tile tile)
    {
        if (selectedTile != null)
        {
            selectedTile.SetOverlayType(OverlayType.None);
        }
        selectedTile = tile;
        tile.SetOverlayType(OverlayType.Selected);
    }
}
