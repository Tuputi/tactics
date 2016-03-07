using UnityEngine;
using System.Collections;

public class TileButton : MonoBehaviour {

    public TileType myType;


    public void SelectAction()
    {
        if (SelectionScript.selectedTiles.Count > 0)
        {
            foreach (Tile t in SelectionScript.selectedTiles)
            {
                t.SetTileType(myType);
            }
        }
    }
}
