using UnityEngine;
using System.Collections.Generic;

public class SelectionScript : MonoBehaviour {

    public static List<Tile> selectedTiles;

    private static bool selectMultiple = false;
    private static bool shiftClick = false;


    void Start()
    {
        selectedTiles = new List<Tile>();   
    }

    void Update()
    {
        if(TurnManager.gameMode != TurnManager.GameMode.Editor)
        {
            return;
        }


        selectMultiple = Input.GetKey(KeyCode.LeftControl);
        shiftClick = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }


    public static void BoxSelect(Tile tile)
    {
        if (selectedTiles.Count != 1)
        {
            return;
        }

        if (selectedTiles[0] == tile)
        {
            return;
        }

        var selectedTile = selectedTiles[0];
        var maps = MapCreator.instance.map;

        var minX = Mathf.Min(selectedTile.xPos, tile.xPos);
        var minY = Mathf.Min(selectedTile.yPos, tile.yPos);

        var countX = minX + Mathf.Abs(selectedTile.xPos - tile.xPos);
        var countY = minY+ Mathf.Abs(selectedTile.xPos - tile.xPos);

        for (int x = minX; x <= countX; x++)
        {
            for (int y = minY; y <= countY; y++)
            {
                if(!selectedTiles.Contains(maps[x, y])) {
                    selectedTiles.Add(maps[x, y]);
                    maps[x, y].SetOverlayType(OverlayType.Selected);
                }
            }
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
            if (shiftClick)
            {
                BoxSelect(tile);
            }
            else if (selectMultiple)
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
       foreach(Tile t in selectedTiles)
       {
           t.SetOverlayType(OverlayType.None);
       }
       selectedTiles.Clear();
    }
}
