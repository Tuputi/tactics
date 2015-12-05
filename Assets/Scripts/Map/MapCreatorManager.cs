using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
public class MapCreatorManager : MonoBehaviour {


    public GameObject characterContainer;
	public static MapCreatorManager instance;

	public int mapRows;
	public int mapColumns;
	public List <List<Tile>> map = new List<List<Tile>> ();

	public TileType paletteSelection = TileType.Grass;
	public InputField mapNameField;
    public string mapName;
	
	void Awake(){
		instance = this;

        if (mapName.Equals(""))
        {
            generateBlankMap(mapRows, mapColumns);
        }
        else
        {
            LoadMapFromXML(mapName);
            Debug.Log("Map loaded from existing xml");
        }
       // characterContainer = GameObject.Find("Characters").gameObject;
	}

	void generateBlankMap(int rows, int columns){

		for (int i = 0; i < this.transform.FindChild("Map").childCount; i++) {
			Destroy(this.transform.FindChild("Map").transform.GetChild(i).gameObject);
		}


		map = new List<List<Tile>> ();
		for (int i = 0; i <= rows; i++) {
			List<Tile> row = new List<Tile>();
			for(int j = 0; j <=columns; j++){
				//Debug.Log(PrefabHolder.instance);
				Tile tile = ((GameObject)Instantiate(PrefabHolder.instance.tile_base, new Vector3(i,0,j),Quaternion.identity)).GetComponent<Tile>();
				tile.transform.SetParent(this.transform.FindChild("Map").transform);
				tile.name = "tile"+i+"-"+j;
				tile.SetType(TileType.Grass);
				tile.positionRow = i;
				tile.positionColumn = j;
				tile.transform.position = new Vector3(tile.positionRow,tile.height,tile.positionColumn);
				row.Add(tile);
			}
			map.Add(row);
		}
		GoThroughNeighbours ();
	}


	void GoThroughNeighbours(){
		List<Tile> tmpList = new List<Tile> ();
		for (int i = 0; i <= mapRows; i++) {
			for(int j = 0;j<=mapColumns;j++){
				Tile currentTile = map[i][j];
				if(i-1>=0){
					if(!(map[i-1][j]==null)){
						tmpList.Add(map[i-1][j]);
					}
				}
				if(i+1<=map.Count-1){
					if(!(map[i+1][j]==null)){
						tmpList.Add(map[i+1][j]);
					}
				}
				if(j+1<=map[i].Count-1){
					if(!(map[i][j+1]==null)){
						tmpList.Add(map[i][j+1]);
					}
				}
				if(j-1>=0){
					if(!(map[i][j-1]==null)){
						tmpList.Add(map[i][j-1]);
					}
				}
				currentTile.SetNeighbours(tmpList);
				tmpList.Clear();
			}
		}
	}


    public void ClearButton()
    {
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                map[i][j].SetOverlayType(OverlayType.Empty, map[i][j]);
            }
        }
    }

    public void CreateCharacter(int characterID)
    {
        if (SelectionManager.selectedTile != null)
        {
            Tile tempTile = SelectionManager.selectedTile;
            if (!tempTile.isOccupied)
            {

                GameObject characterBase = null;
                foreach(Character chara in PrefabHolder.instance.characters)
                {
                    if(chara.characterID == characterID)
                    {
                        characterBase = chara.gameObject;
                        break;
                    }
                }
                if(characterBase == null)
                {
                    Debug.Log("CharacterId not found. Looking for ID"+characterID);
                }
                GameObject newCharacter = (GameObject)Instantiate(characterBase);
                tempTile.SetCharacter(newCharacter.GetComponent<Character>());
                newCharacter.transform.position = new Vector3(tempTile.positionRow, tempTile.height, tempTile.positionColumn);


                //numbers if similar names

                string newName = newCharacter.GetComponent<Character>().characterName;
                int instancesOfName = 0;
                for (int i = 0; i < characterContainer.transform.childCount; i++)
                {
                    string charaName = characterContainer.transform.GetChild(i).GetComponent<Character>().characterName;
                    if (charaName.Equals(newCharacter.GetComponent<Character>().characterName))
                    {
                        instancesOfName++;
                        if (instancesOfName > 0)
                        {
                            newCharacter.GetComponent<Character>().characterName = newName + " " + instancesOfName;
                        }
                    }
                }
                newCharacter.transform.SetParent(characterContainer.transform);
                Debug.Log("Character created");
            }
            else
            {
                Debug.Log("Tile already occupied");
            }
        }
        else
        {
            Debug.Log("No tile selected");
        }
    }

    public void createObject(int objectID){
        if (SelectionManager.selectedTile != null)
        {
            Tile tempTile = SelectionManager.selectedTile;
          
                GameObject objectBase = null;
                foreach (MapObject obj in PrefabHolder.instance.MapObjects)
                {
                    if (obj.ObjectId == objectID)
                    {
                        objectBase = obj.gameObject;
                        break;
                    }
                }
                if (objectBase == null)
                {
                    Debug.Log("ObjectID not found. Looking for ID" + objectID);
                }
                GameObject newObject = (GameObject)Instantiate(objectBase);
                tempTile.objectId = newObject.GetComponent<MapObject>().ObjectId;
                newObject.transform.position = new Vector3(tempTile.positionRow, tempTile.height+newObject.transform.position.y, tempTile.positionColumn);
                Debug.Log("object created");
       
        }
        else
        {
            Debug.Log("No tile selected");
    }
}

	public void LoadMapFromXML(string loadMapName){
		if (!(loadMapName.Equals(""))) {
			MapXmlContainer container = MapSaveLoad.Load (loadMapName);
		if(!(container == null)){
				int rows = container.rows;
				int columns = container.columns;

				for (int i = 0; i < this.transform.FindChild("Map").childCount; i++) {
					Destroy (this.transform.FindChild ("Map").transform.GetChild (i).gameObject);
				}

				map = new List<List<Tile>> ();
				for (int i = 0; i <= rows-1; i++) {
					List<Tile> row = new List<Tile> ();
					for (int j = 0; j <=columns-1; j++) {
						
						Tile tile = ((GameObject)Instantiate (PrefabHolder.instance.tile_base, new Vector3 (i, 0, j), Quaternion.identity)).GetComponent<Tile> ();
                                       

                        tile.name = "tile" + i + "-" + j;
						tile.SetType ((TileType)container.tiles.Where (x => x.locX == i && x.locY == j).First ().id);
						tile.positionRow = i;
						tile.positionColumn = j;
						tile.height = container.tiles.Where (x => x.locX == i && x.locY == j).First ().height;
                        tile.tileStat = container.tiles.Where(x => x.locX == i && x.locY == j).First().tileStat;
                        tile.charaId = container.tiles.Where(x => x.locX == i && x.locY == j).First().characterID;
                        tile.objectId = container.tiles.Where(x => x.locX == i && x.locY == j).First().objectID;
                        tile.characterFacing = (Facing)container.tiles.Where(x => x.locX == i && x.locY == j).First().characterFacing;
                        tile.transform.position = new Vector3 (tile.positionRow, tile.height, tile.positionColumn);
						tile.transform.parent = this.transform.FindChild ("Map").transform;
						row.Add (tile);
					}
					map.Add (row);
				}
                GoThroughNeighbours();
			}

            foreach (List<Tile> column in map)
            {
                foreach (Tile tile in column)
                {
                    switch (tile.tileStat)
                    {
                        case 0:
                            break;
                        case 1:
                            SelectionManager.SetSelection(tile);
                            CreateCharacter(tile.charaId);
                            tile.character.ChangeFacing(tile.characterFacing);
                            break;
                        default: break;
                    }
                    if (tile.objectId > 0)
                    {
                        SelectionManager.SetSelection(tile);
                        createObject(tile.objectId);
                    }

                    if(tile.tileType == TileType.None || tile.objectId > 0)
                    {
                        foreach(Tile t in tile.neighbours)
                        {
                            t.neighbours.Remove(tile);
                        }
                    }

                }
            }
		}
		else {
			Debug.Log("Enter map name");
		}
	}

	public void SaveMapToXml(){
		if (!(mapNameField.text.Equals(""))) {
            foreach(Character chara in characterContainer.GetComponentsInChildren<Character>())
            {
                Tile t = chara.position;
                t.characterFacing = chara.facing;
            }

			MapSaveLoad.Save (MapSaveLoad.CreateMapContainer (map), mapNameField.text+".xml");
		} else {
			Debug.Log("Enter map name");
		}
	}

}
