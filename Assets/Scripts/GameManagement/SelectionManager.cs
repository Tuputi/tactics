using UnityEngine;
using System.Collections;

public class SelectionManager : MonoBehaviour {

    public static Tile selectedTile = null;
    public static Tile targetTile = null;
    public static Character selectedCharacter = null;
    public static Character targetCharacter = null;

  

    public static void SetSelection(Tile tile)
    {
        if (selectedTile != null)
        {
            GameObject overlayContainer = selectedTile.transform.FindChild("Overlay").gameObject;
            for (int h = 0; h < overlayContainer.transform.childCount; h++)
            {
                Destroy(overlayContainer.transform.GetChild(h).gameObject);
            }
        }
        selectedTile = tile;
        selectedTile.SetOverlayType(OverlayType.Path,selectedTile);

    }

    public static void SetTarget(Tile tile)
    {
        if (selectedTile != null)
        {
            targetTile = tile;
           
        }
        else
        {
            Debug.Log("Please select something first");
        }
    }

    public static bool SelectionReady(){
        return (selectedTile != null && targetTile != null);
    }

    public static void ClearSelection()
    {
        selectedTile = null;
        targetTile = null;
        selectedCharacter = null;
        targetCharacter = null;
    }

}
