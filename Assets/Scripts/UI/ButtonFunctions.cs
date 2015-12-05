using UnityEngine;
using System.Collections;

public class ButtonFunctions : MonoBehaviour {

   public MapCreatorManager mcm;

    void Awake()
    {
      //  mcm = this.gameObject.GetComponent<MapCreatorManager>();
    }

    public void TargetButton()
    {
        Tile.mode = ModeType.Target;
        mcm.paletteSelection = TileType.None;
    }

    public void GrassButton()
    {
        Tile.mode = ModeType.ChangeTileType;
        mcm.paletteSelection = TileType.Grass;
    }
    public void RockButton()
    {
        Tile.mode = ModeType.ChangeTileType;
        mcm.paletteSelection = TileType.Rock;
    }
    public void InpassableButton()
    {
        Tile.mode = ModeType.ChangeTileType;
        mcm.paletteSelection = TileType.Inpassable;
    }

    public void LowerHeightButton()
    {
        mcm.paletteSelection = TileType.None;
        Tile.mode = ModeType.ChangeHeight;
        Tile.changeHeightAmount = -0.2f;
    }

    public void RaiseHeightButton()
    {
        mcm.paletteSelection = TileType.None;
        Tile.mode = ModeType.ChangeHeight;
        Tile.changeHeightAmount = 0.2f;
    }

    public void RemoveTile()
    {
        Tile.mode = ModeType.Remove;
    }

    public void Load()
    {
        mcm.LoadMapFromXML(mcm.mapNameField.text);
    }

    public void Save()
    {
        mcm.SaveMapToXml();
    }
}
