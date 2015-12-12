using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

    List<Tile> neighbours;

    public void SetNeighbour(List<Tile> tiles)
    {
        neighbours = tiles;
    }
}
