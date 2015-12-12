using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

    List<Tile> neighbours;

    public void SetNeighbours(List<Tile> tiles)
    {
        neighbours = tiles;
    }

    void Update()
    {

    }

}
