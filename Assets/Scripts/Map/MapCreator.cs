using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour {

	public GameObject TilePrefab;
	public int rows;
	public int columns;

	private List<List<Tile>> map;


	// Use this for initialization
	void Start () {
		GenerateMap ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GenerateMap(){
		map = new List<List<Tile>> ();
		for (int i = 0; i <= rows; i++) {
			List<Tile> row = new List<Tile>();
			for(int j = 0; j <=columns; j++){
				Tile tile = ((GameObject)Instantiate(TilePrefab, new Vector3(i,0,j),Quaternion.identity)).GetComponent<Tile>();
				tile.transform.SetParent(GameObject.Find("MapGeneration").transform);
				tile.name = "tile"+i+"-"+j;
				tile.SetType(TileType.Grass);
				tile.positionRow = i;
				tile.positionColumn = j;
				tile.transform.position = new Vector3(tile.positionRow,tile.height,tile.positionColumn);
				row.Add(tile);
			}
			map.Add(row);
		}
	}
}
