using UnityEngine;
using System.Collections;

public class OverlayArrow : MonoBehaviour {

    public Tile myTile;

    public void SetTile(Tile t)
    {
        myTile = t;
        Debug.Log(myTile);
    }

    void OnMouseDown()
    {
        Debug.Log("In on mouse down in overlay" + myTile);
        UIManager.instance.CompleteFacing(myTile);
    }
}
