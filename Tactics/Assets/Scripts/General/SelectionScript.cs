using UnityEngine;
using System.Collections.Generic;

public class SelectionScript : MonoBehaviour {

    public static List<Tile> selectedTiles;

    private static bool selectMultiple = false;
    private static bool shiftClick = false;
    private static bool noSelection = false;


    void Start()
    {
        selectedTiles = new List<Tile>();   
    }

    void Update()
    {
        if(TurnManager.gameMode != GameMode.Editor)
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

    public static void SetNoSelection(bool on)
    {
        noSelection = on;
    }

    public static void SetSelectedTile(Tile tile)
    {
        if (noSelection)
        {
            return;
        }

        if(TurnManager.gameMode == GameMode.Game)
        {
            SetSelectedTileGame(tile);
        }
        
        if(TurnManager.gameMode == GameMode.Editor)
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
    
    static void SetSelectedTileGame(Tile tile)
    {
        SetSingleSelectedTile(tile);
        if (tile.isOccupied)
        {
            UIManager.instance.UpdateStatus(tile.tileCharacter);
        }
        else
        {
            UIManager.instance.UpdateStatus(false);  
        }

        if (TurnManager.mode == TurnManager.TurnMode.move || TurnManager.mode == TurnManager.TurnMode.action)
        {
            ClearSelection();
            foreach (Tile t in TurnManager.instance.CurrentlyTakingTurn.possibleRange)
            {
                t.SetOverlayType(OverlayType.None);
            }
            if (TurnManager.instance.CurrentlyTakingTurn.possibleRange.Contains(tile))
            {
                ClearSelection();
                if (TurnManager.mode == TurnManager.TurnMode.move)
                {
                    SetSingleSelectedTile(tile);
                    ConfirmationDialogue.instance.Show(ConfirmationType.move, tile);
                }
                if (TurnManager.mode == TurnManager.TurnMode.action)
                {
                    List<Tile> tempList = TurnManager.instance.CurrentlyTakingTurn.currentAction.DrawTargetArea(tile);
                    if (tempList.Count > 0)
                    {
                        foreach (Tile t in tempList)
                        {
                            SetMultipleSelectedTile(t);
                            if (t.isOccupied)
                            {
                                UIManager.instance.UpdateStatus(t.tileCharacter);
                            }
                        }
                    }
                    ConfirmationDialogue.instance.Show(ConfirmationType.action, tile);
                }
            }
            else
            {
                TurnManager.mode = TurnManager.TurnMode.start;
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

}
