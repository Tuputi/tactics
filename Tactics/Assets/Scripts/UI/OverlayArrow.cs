using UnityEngine;
using System.Collections;

public class OverlayArrow : MonoBehaviour {

    public Tile myTile;

    public void SetTile(Tile t)
    {
        myTile = t;
    }

    public void RotateArrow()
    {
        Tile curLoc = TurnManager.instance.CurrentlyTakingTurn.characterPosition;
        if (myTile.xPos > curLoc.xPos)
        {
            this.gameObject.transform.Rotate(0, -90, 0, Space.Self);
        }
        else if(myTile.xPos < curLoc.xPos)
        {
            this.gameObject.transform.Rotate(0, 90, 0, Space.Self);
        }
        else if(myTile.yPos > curLoc.yPos)
        {
            this.gameObject.transform.Rotate(0, 180, 0, Space.Self);
        }
        else
        {
        }
    }

    void OnMouseDown()
    {
        UIManager.instance.CompleteFacing(myTile);
    }
}
