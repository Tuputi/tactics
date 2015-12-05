using UnityEngine;
using System.Collections;

using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IComparable
{
    public bool walkable = true;
    public Character character;
    public MapObject mapObject;
    public bool isOccupied {
        get {
            return (!((character == null) && (mapObject == null)));
            }
    }

    
    
	public int positionRow;
	public int positionColumn;
    public float height;

	public List<Tile> neighbours;

	public float movementCost = 1;
	public float pathfindingCost = 0;

    public Tile cameFrom = null;
  
    public float gCost = 0;

    public OverlayType previousType = OverlayType.Empty;
    public OverlayType currentType = OverlayType.Empty;


    //editor
    public static float changeHeightAmount = 0.5f;
    public TileType tileType = TileType.Grass;
    private GameObject prefab;
    public static ModeType mode = ModeType.Target;

    //saving
    public int tileStat = 0; //0 - empty, 1 - occupied, 2 - ????
    public int charaId = 0;
    public int objectId = 0;
    public Facing characterFacing;
    public Facing objectFacing;

    // Use this for initialization
    void Start () {
		SetType (tileType);
	}
	
    bool Equals(Tile other)
    {
        if(other.positionRow == this.positionRow)
        {
            if(other.positionColumn == this.positionColumn)
            {
                return true;
            }
        }
        return false;
    }

    public int CompareTo(object other)
    {
        Tile otherTile = (Tile)other;
        if (otherTile != null)
        {
            if (otherTile.pathfindingCost > this.pathfindingCost)
            {
                return -1;
            }
            else if (otherTile.pathfindingCost < this.pathfindingCost)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            throw new ArgumentException("Object is not a Tile");
        }
    }

    void OnMouseEnter(){

        if (Input.GetMouseButton(0))
        {
            switch (mode)
            {
                case ModeType.ChangeHeight:
                    SetHeight(changeHeightAmount);
                    break;
                case ModeType.ChangeTileType:
                    SetType(MapCreatorManager.instance.paletteSelection);
                    break;
            }
        }
	}

    //code by mwk888
    private bool IsPointerOverUIObject()
    {
      
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    void OnMouseDown(){
        if(tileType == TileType.None)
        {
            return;
        }

        bool UiTouch = false;

        if (Input.GetMouseButton(0))
        {

            //stop clicks going through UI
            //!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || 
            /*if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                UiTouch = true;
            }*/
            
            UiTouch = IsPointerOverUIObject();

           /* foreach (Touch touch in Input.touches)
             {
                int pointerID = touch.fingerId;
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(pointerID) && !(touch.phase == TouchPhase.Ended) && !(touch.phase == TouchPhase.Canceled))
                 {
                     // at least on touch is over a canvas UI
                     UiTouch = true;
                 }
             }*/

            if (!UiTouch)
            {
                InputManager.CurrentTile.SetOverlayType(InputManager.CurrentTile.previousType, InputManager.CurrentTile); //back to the previous type
                InputManager.CurrentTile = this;
                InputManager.CurrentTile.previousType = InputManager.CurrentTile.currentType;
                InputManager.CurrentTile.SetOverlayType(OverlayType.Path, InputManager.CurrentTile);
               /* if (!Input.touchSupported)
                {
                    PrefabHolder.instance.gameCamera.transform.SetParent(InputManager.CurrentTile.gameObject.transform);
                    PrefabHolder.instance.gameCamera.transform.localPosition = new Vector3(0, PrefabHolder.instance.gameCamera.transform.localPosition.y, 0);
                }*/

                // TurnManager.currentlytakingTurn.ChangeFacing(TurnManager.currentlytakingTurn.position, this);
                switch (mode)
                {
                    case ModeType.Target:
                        if (isOccupied)
                        {
                            if (character != null)
                            {
                                GameUI.UpdateStatUI(this.character);
                            }
                          /* if(mapObject != null)
                            {
                                Debug.Log("rotate");
                                    int currentFacing = (int)this.mapObject.facing;
                                    if (++currentFacing > 3)
                                    {
                                        this.mapObject.ChangeFacing((Facing)0);
                                    }
                                    else
                                    {
                                        this.mapObject.ChangeFacing((Facing)currentFacing++);
                                    }
                                Debug.Log("currentFacing " + currentFacing);
                                
                            }*/

                            if (SelectionManager.selectedCharacter == null || TurnManager.currentlytakingTurn == null)
                            {
                                Debug.Log("No character selected!");
                            }
                            else if (!this.character.Equals(TurnManager.currentlytakingTurn))
                            {
                                if (TurnManager.turnState == TurnState.Attack)
                                {
                                    TurnManager.targetTile = this;
                                    TurnManager.currentlytakingTurn.characterAttackList[TurnManager.currentlytakingTurn.currentAttack].DrawTargetArea(this);
                                    GameUI.AttackInformation();
                                }
                            }
                            else
                            {
                                if (TurnManager.turnState == TurnState.Move)
                                {
                                    TurnManager.ClearMove();
                                }
                                else if (TurnManager.turnState == TurnState.Attack)
                                {
                                    TurnManager.ClearAttack();
                                }
                                MapCreatorManager.instance.ClearButton();
                            }

                        }
                        else if ((!isOccupied) && TurnManager.turnState == TurnState.Attack && TurnManager.currentlytakingTurn.currentAttack == AttackType.AreaOfEffect)
                        {
                            GameUI.ShowStatUI(false);
                            //restricts attacks so that they need to hit someone
                            bool permissionToAttack = false;
                            foreach (Tile t in this.neighbours)
                            {
                                if (t.isOccupied)
                                {
                                    permissionToAttack = true;
                                }
                            }

                            if (permissionToAttack)
                            {

                                TurnManager.targetTile = this;
                                TurnManager.currentlytakingTurn.characterAttackList[TurnManager.currentlytakingTurn.currentAttack].DrawTargetArea(this);
                                GameUI.AttackInformation();

                            }

                        }
                        else
                        {
                            GameUI.ShowStatUI(false);
                            if (SelectionManager.selectedCharacter != null)
                            {
                                if (TurnManager.turnState == TurnState.Move)
                                {
                                    PrefabHolder.instance.gameCamera.transform.SetParent(SelectionManager.selectedCharacter.transform);
                                    PrefabHolder.instance.gameCamera.transform.localPosition = new Vector3(0, PrefabHolder.instance.gameCamera.transform.localPosition.y, 0);
                                    SelectionManager.selectedCharacter.CompleteMove(this);
                                    MapCreatorManager.instance.ClearButton();
                                }
                            }
                            else
                            {
                                SelectionManager.SetSelection(this);
                            }
                        }
                        break;
                    case ModeType.ChangeHeight:
                        SetHeight(changeHeightAmount);
                        break;
                    case ModeType.ChangeTileType:
                        SetType(MapCreatorManager.instance.paletteSelection);
                        break;
                    case ModeType.Remove:
                        foreach (Tile t in neighbours)
                        {
                            t.neighbours.Remove(this);
                        }
                        SetType(TileType.None);
                        break;
                    default:
                        break;
                }
            }
        }
    }

   public void SetCharacter(Character chara)
    {
        if(chara.position != null)
        {
            chara.position.RemoveCharacter();
        }
        character = chara;
        chara.position = this;
        InputManager.CurrentTile = this;

        tileStat = 1;
        charaId = chara.characterID;
    }

    public void RemoveCharacter()
    {
        character = null;
        tileStat = 0;
    }


	public void SetHeight(float changeInHeight){
		height += changeInHeight;
		this.transform.position = new Vector3(positionRow, height, positionColumn);
	}

	public void SetType(TileType typeOfTile){
		tileType = typeOfTile;
		switch (typeOfTile) {
		case TileType.Grass:
			prefab = PrefabHolder.instance.Tile_Grass_Prefab;
			movementCost = 1;
                walkable = true;
			break;
		case TileType.Sand:
			prefab = PrefabHolder.instance.Tile_Sand_Prefab;
			movementCost = 1;
                walkable = true;
                break;
		case TileType.Rock:
			prefab = PrefabHolder.instance.Tile_Rock_Prefab;
			movementCost = 2;
            walkable = true;
                break;
        case TileType.Inpassable:
           prefab = PrefabHolder.instance.Tile_Inpassable_Prefab;
                walkable = false;
                break;
            /*case TileType.WaterShallow:
                prefab = PrefabHolder.instance.Tile_WaterShallow_Prefab;
                break;
            case TileType.WaterDeep:
                prefab = PrefabHolder.instance.Tile_WaterDeep_Prefab;
                break;*/
                
            case TileType.None:
                prefab = PrefabHolder.instance.Tile_Empty_Prefab;
                break;
            default:
			throw new System.ArgumentOutOfRangeException ();
		}
		GenerateVisuals ();
	}

    public void SetOverlayType(OverlayType type, Tile t)
    {
       // previousType = currentType;
        currentType = type;
        switch (type)
        {
            case OverlayType.Path:
                Overlay.instance.SetType(OverlayType.Path, t);
                break;
            case OverlayType.Attack:
                Overlay.instance.SetType(OverlayType.Attack, t);
                break;
            case OverlayType.Empty:
                Overlay.instance.SetType(OverlayType.Empty, t);
                break;
            default:
                break;
        }
       
    }


	public void SetNeighbours(List<Tile> tiles){
		neighbours = new List<Tile> (tiles);
	}
	
	public void GenerateVisuals (){
		GameObject container = transform.FindChild ("Visuals").gameObject;
		//remove children
		for (int i = 0; i < container.transform.childCount; i++) {
			Destroy(container.transform.GetChild(i).gameObject);
		}

		GameObject newVisual = (GameObject)Instantiate (prefab, transform.position, Quaternion.identity);
		newVisual.transform.parent = container.transform;

	}

}
