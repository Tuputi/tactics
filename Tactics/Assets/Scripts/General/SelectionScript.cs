using UnityEngine;
using System.Collections.Generic;

public class SelectionScript : MonoBehaviour {

    public static List<Tile> selectedTiles;

    public static bool selectMultiple = false;
    private static bool shiftClick = false;
    private static bool noSelection = false;

    private static bool WaitingConfirmation = false;
    private static Tile tileInMemory = null;


    void Start()
    {
        selectedTiles = new List<Tile>();   
    }

    void Update()
    {
        if(TurnManager.gameMode == GameMode.Editor)
        {
            selectMultiple = Input.GetKey(KeyCode.LeftControl);
            shiftClick = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            return;
        }

        Debug.Log("WaitinConfirmation? "+ WaitingConfirmation);
        if (WaitingConfirmation)
        {
            Debug.Log("DoubleClick status: "+TacticsInput.instance.DoubleClick);
            if (TacticsInput.instance.TargetChanged)
            {
                tileInMemory = null;
                WaitingConfirmation = false;
                return;
            }
            if (TacticsInput.instance.DoubleClick)
            {
                if(TurnManager.mode == TurnManager.TurnMode.move)
                {
                    List<Tile> tempPath = Pathfinding.GetPath(TurnManager.instance.CurrentlyTakingTurn.characterPosition, tileInMemory);
                    CharacterLogic.instance.CompleteMove(TurnManager.instance.CurrentlyTakingTurn, tempPath);
                    tileInMemory = null;
                    WaitingConfirmation = false;
                    TacticsInput.instance.ResetDoubleClick();
                    //ConfirmationDialogue.instance.Show(ConfirmationType.move, tile);
                }
                if (TurnManager.mode == TurnManager.TurnMode.action)
                {
                    CharacterLogic.instance.CompleteAction(TurnManager.instance.CurrentlyTakingTurn, tileInMemory);
                    UIManager.instance.CloseInventoryAfterCompletedAttack();
                    tileInMemory = null;
                    WaitingConfirmation = false;
                    TacticsInput.instance.ResetDoubleClick();
                    //ConfirmationDialogue.instance.Show(ConfirmationType.action, tile);
                }
            }
        }
       
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
        TacticsInput.instance.RegisterClick(tile.gameObject);

        if (selectMultiple)
        {
            SetMultipleSelectedTile(tile);
            return;
        }
       
        //update statusUi
        UIManager.instance.UpdateStatusWindow(tile);


        if (TurnManager.mode == TurnManager.TurnMode.move)
        {            
            if (TurnManager.instance.CurrentlyTakingTurn.possibleRange.Contains(tile))
            {
                ClearSelection();
                SetSingleSelectedTile(tile);
                tileInMemory = tile;
                WaitingConfirmation = true;
            }
        }
        else if (TurnManager.mode == TurnManager.TurnMode.action)
        {
            if (TurnManager.instance.CurrentlyTakingTurn.possibleRange.Contains(tile))
            {
                ClearSelection();
                foreach(Tile ti in TurnManager.instance.CurrentlyTakingTurn.possibleRange)
                {
                    SetMultipleSelectedTile(ti, OverlayType.Selected);
                }
                tileInMemory = tile;
                WaitingConfirmation = true;
                List<Tile> tempList = TurnManager.instance.CurrentlyTakingTurn.currentAction.DrawTargetArea(tile);
                if (tempList.Count > 0)
                {
                    foreach (Tile t in tempList)
                    {
                        SetMultipleSelectedTile(t, OverlayType.Attack);
                        if (t.isOccupied)
                        {
                            UIManager.instance.UpdateStatusWindow(t.tileCharacter);
                        }
                    }
               }
            }
        }
        else
        {
            SetSingleSelectedTile(tile);
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

    //will not remove tiles, overwrite with something else if "removal" is desired
    //will ignore invisible tiles
    static void SetMultipleSelectedTile(Tile tile, OverlayType type)
    {
       selectedTiles.Add(tile);
       if (!(tile.tileType == TileType.None))
       {
          tile.SetOverlayType(type);
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
