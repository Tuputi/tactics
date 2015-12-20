using UnityEngine;
using System.Collections.Generic;

public class SelectionScript : MonoBehaviour {

    public static List<Tile> selectedTiles;
    public static bool selectMultiple = false;

    void Start()
    {
        selectedTiles = new List<Tile>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && !selectMultiple)
        {
            selectMultiple = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            selectMultiple = false;
        }
    }

    public static void SetSelectedTile(Tile tile)
    {
        if (TurnManager.mode == TurnManager.TurnMode.move || TurnManager.mode == TurnManager.TurnMode.action)
        {
            SetSingleSelectedTile(tile);
            if (TurnManager.instance.CurrentlyTakingTurn.possibleRange.Contains(tile))
            {
                ClearAll();
                SetSingleSelectedTile(tile);
                if (TurnManager.mode == TurnManager.TurnMode.move)
                {
                    TurnManager.instance.CurrentlyTakingTurn.CompleteMove(selectedTiles[0]);
                }
                if(TurnManager.mode == TurnManager.TurnMode.action)
                {
                    TurnManager.instance.CurrentlyTakingTurn.CompleteAction(selectedTiles[0]);
                }
            }
        }
        else
        {
            if (selectMultiple)
            {
                SetMultipleSelectedTile(tile);
            }
            else
            {
                SetSingleSelectedTile(tile);
            }
        }
    }

    static void SetMultipleSelectedTile(Tile tile)
    {
        if (selectedTiles.Contains(tile))
        {
            tile.SetOverlayType(OverlayType.None);
            selectedTiles.Remove(tile);
        }
        else
        {
            selectedTiles.Add(tile);
            tile.SetOverlayType(OverlayType.Selected);
        }
    }

    static void SetSingleSelectedTile(Tile tile)
    {
        foreach(Tile t in selectedTiles)
        {
            t.SetOverlayType(OverlayType.None);
        }
        selectedTiles.Clear();

        selectedTiles.Add(tile);
        tile.SetOverlayType(OverlayType.Selected);
    }

    public static void ClearSelection()
    {
        if(selectedTiles.Count > 0)
        {
            foreach(Tile t in selectedTiles)
            {
                t.SetOverlayType(OverlayType.None);
            }
            selectedTiles.Clear();
        }
    }

    public static void ClearAll()
    {
        selectedTiles.Clear();
       int rows = MapCreator.instance.map.Count;
       for(int i = 0; i < rows; i++)
       {
            List<Tile> tempList = MapCreator.instance.map[i];
            foreach(Tile t in tempList)
            {
                t.SetOverlayType(OverlayType.None);
            }
       }
    }
}
