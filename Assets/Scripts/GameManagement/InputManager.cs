using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    public static Tile CurrentTile = null;
    public GameObject CentrePoint;



    Tile up = null;
    Tile down = null;
    Tile left = null;
    Tile right = null;
    CameraRotationState camState = CameraRotationState.forward;

    enum CameraRotationState { forward, right, back, left}

    //touch
    private Vector2 touchStartPos;

    private float minZoom = 0.5f;
    private float maxZoom = 5f;
    private float zoomSpeed = 0.05f;

    //touch test
    public UnityEngine.UI.Image testImage;


    void Start()
    {
        CurrentTile = MapCreatorManager.instance.map[0][0];
    }

    void Update()
    {

        
        float rotation = CentrePoint.transform.eulerAngles.y;
        
        if (rotation < 337.5f && rotation > 202.5f)
        {
            camState = CameraRotationState.right;
           
        }
        else if (rotation < 202.5f && rotation > 157.5f)
        {
            camState = CameraRotationState.back;
          
        }
        else if (rotation < 157.5f && rotation > 22.5f)
        {
            camState = CameraRotationState.left;
           
        }
        else if (rotation < 22.5f || rotation > 337.5f)
        {
            camState = CameraRotationState.forward;
            
        }

        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        touchStartPos = Input.GetTouch(0).position;
                    }
                    else if (touch.phase == TouchPhase.Moved && ((Mathf.Abs(touchStartPos.magnitude - touch.position.magnitude)) > 10f))
                    {
                        Vector2 touchDeltaPos = Input.GetTouch(0).deltaPosition;
                        switch (camState)
                        {
                            case CameraRotationState.forward:
                                CentrePoint.gameObject.transform.position += new Vector3(-touchDeltaPos.x * 0.05f, 0, -touchDeltaPos.y * 0.05f);
                                break;
                            case CameraRotationState.right:
                                CentrePoint.gameObject.transform.position += new Vector3(+touchDeltaPos.y * 0.05f, 0, -touchDeltaPos.x * 0.05f);
                                break;
                            case CameraRotationState.back:
                                CentrePoint.gameObject.transform.position += new Vector3(+touchDeltaPos.x * 0.05f, 0, +touchDeltaPos.y * 0.05f);
                                break;
                            case CameraRotationState.left:
                                CentrePoint.gameObject.transform.position += new Vector3(-touchDeltaPos.y * 0.05f, 0, +touchDeltaPos.x * 0.05f);
                                break;
                            default:
                                break;
                        }

                        Vector3 pos = CentrePoint.gameObject.transform.position;
                        pos.x = Mathf.Clamp(pos.x, 0, 15);
                        // CentrePoint.gameObject.transform.position = pos;
                        pos.z = Mathf.Clamp(pos.z, 0, 15);
                        CentrePoint.gameObject.transform.position = pos;

                    }
                }
                if (Input.touchCount == 2)
                {
                    Touch touch1 = Input.GetTouch(0);
                    Touch touch2 = Input.GetTouch(1);

                    if (touch1.deltaPosition.magnitude < 0.4f)
                    {
                        testImage.color = Color.blue;
                        CentrePoint.gameObject.transform.RotateAround(CentrePoint.gameObject.transform.position, transform.up, touch2.deltaPosition.y * 0.5f);
                        //this.gameObject.transform.GetChild(0).gameObject.transform.RotateAround(this.gameObject.transform.GetChild(0).gameObject.transform.position, transform.right, touch2.deltaPosition.x * 0.5f);
                    }
                    else
                    {
                        //UnityEngine.UI.Image image = GameObject.Find("MyImage").GetComponent<UnityEngine.UI.Image>();
                        testImage.color = Color.red;

                        Vector2 touch1PreviousPOs = touch1.position - touch1.deltaPosition;
                        Vector2 touch2PreviousPOs = touch2.position - touch2.deltaPosition;

                        float prevTouchMagnitude = (touch1PreviousPOs - touch2PreviousPOs).magnitude;
                        float touchDeltaMagnitude = (touch1.position - touch2.position).magnitude;

                        float deltaMagDif = prevTouchMagnitude - touchDeltaMagnitude;

                        CentrePoint.gameObject.transform.position += new Vector3(0, deltaMagDif * zoomSpeed, 0);
                        Vector3 pos = CentrePoint.gameObject.transform.position;
                        pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);
                        CentrePoint.gameObject.transform.position = pos;

                    }
                }
            }
        }
        else
        {

            //Find out which tile is to which direction
                    if (!(CurrentTile == null))
                    {
                        foreach (Tile t in CurrentTile.neighbours)
                        {
                            if (t.positionColumn > CurrentTile.positionColumn)
                            {
                                switch (camState)
                                {
                                    case CameraRotationState.forward:
                                        up = t;
                                        break;
                                    case CameraRotationState.right:
                                        right = t;
                                        break;
                                    case CameraRotationState.back:
                                        down = t;
                                        break;
                                    case CameraRotationState.left:
                                        left = t;
                                        break;
                                    default:
                                        break;
                                }

                            }
                            else if (t.positionColumn < CurrentTile.positionColumn)
                            {
                                switch (camState)
                                {
                                    case CameraRotationState.forward:
                                        down = t;
                                        break;
                                    case CameraRotationState.right:
                                        left = t;
                                        break;
                                    case CameraRotationState.back:
                                        up = t;
                                        break;
                                    case CameraRotationState.left:
                                        right = t;
                                        break;
                                    default:
                                        break;
                                }

                            }
                            else if (t.positionRow > CurrentTile.positionRow)
                            {
                                switch (camState)
                                {
                                    case CameraRotationState.forward:
                                        right = t;
                                        break;
                                    case CameraRotationState.right:
                                        down = t;
                                        break;
                                    case CameraRotationState.back:
                                        left = t;
                                        break;
                                    case CameraRotationState.left:
                                        up = t;
                                        break;
                                    default:
                                        break;
                                }

                            }
                            else
                            {
                                switch (camState)
                                {
                                    case CameraRotationState.forward:
                                        left = t;
                                        break;
                                    case CameraRotationState.right:
                                        up = t;
                                        break;
                                    case CameraRotationState.back:
                                        right = t;
                                        break;
                                    case CameraRotationState.left:
                                        down = t;
                                        break;
                                    default:
                                        break;
                                }

                            }
                        }
                    }



                    //react to key input

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        CurrentTile.SetOverlayType(CurrentTile.previousType, CurrentTile);
                        CurrentTile = up;
                        CurrentTile.previousType = CurrentTile.currentType;
                        CurrentTile.SetOverlayType(OverlayType.Path, CurrentTile);
                        PrefabHolder.instance.gameCamera.transform.SetParent(CurrentTile.gameObject.transform);
                        PrefabHolder.instance.gameCamera.transform.localPosition = new Vector3(0, 0, 0);

                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        CurrentTile.SetOverlayType(CurrentTile.previousType, CurrentTile);
                        CurrentTile = down;
                        CurrentTile.previousType = CurrentTile.currentType;
                        CurrentTile.SetOverlayType(OverlayType.Path, CurrentTile);
                        PrefabHolder.instance.gameCamera.transform.SetParent(CurrentTile.gameObject.transform);
                        PrefabHolder.instance.gameCamera.transform.localPosition = new Vector3(0, 0, 0);

                    }
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        CurrentTile.SetOverlayType(CurrentTile.previousType, CurrentTile);
                        CurrentTile = left;
                        CurrentTile.previousType = CurrentTile.currentType;
                        CurrentTile.SetOverlayType(OverlayType.Path, CurrentTile);
                        PrefabHolder.instance.gameCamera.transform.SetParent(CurrentTile.gameObject.transform);
                        PrefabHolder.instance.gameCamera.transform.localPosition = new Vector3(0, 0, 0);
                    }
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        CurrentTile.SetOverlayType(CurrentTile.previousType, CurrentTile);
                        CurrentTile = right;
                        CurrentTile.previousType = CurrentTile.currentType;
                        CurrentTile.SetOverlayType(OverlayType.Path, CurrentTile);
                        PrefabHolder.instance.gameCamera.transform.SetParent(CurrentTile.gameObject.transform);
                        PrefabHolder.instance.gameCamera.transform.localPosition = new Vector3(0, 0, 0);
                    }

                  
                    //react to enter press

                    if (Input.GetKeyDown(KeyCode.Return))
                    {

                        if (TurnManager.turnState == TurnState.Attack)
                        {
                            if (CurrentTile.isOccupied)
                            {
                                CurrentTile.SetOverlayType(CurrentTile.previousType, CurrentTile);
                                TurnManager.targetTile = CurrentTile;
                                CurrentTile.previousType = CurrentTile.currentType;
                                TurnManager.currentlytakingTurn.characterAttackList[TurnManager.currentlytakingTurn.currentAttack].DrawTargetArea(CurrentTile);
                                GameUI.AttackInformation();
                            }
                            else
                            {
                                bool permissionToAttack = false;
                                foreach (Tile t in CurrentTile.neighbours)
                                {
                                    if (t.isOccupied)
                                    {
                                        permissionToAttack = true;
                                    }
                                }

                                if (permissionToAttack)
                                {
                                    CurrentTile.SetOverlayType(CurrentTile.previousType, CurrentTile);
                                    TurnManager.targetTile = CurrentTile;
                                    CurrentTile.previousType = CurrentTile.currentType;
                                    TurnManager.currentlytakingTurn.characterAttackList[TurnManager.currentlytakingTurn.currentAttack].DrawTargetArea(CurrentTile);
                                    GameUI.AttackInformation();

                                }
                            }
                        }


                        if (TurnManager.turnState == TurnState.Move)
                        {
                            if (!CurrentTile.isOccupied)
                            {
                                TurnManager.currentlytakingTurn.CompleteMove(CurrentTile);
                                MapCreatorManager.instance.ClearButton();
                                Debug.Log("move");
                  }
               }
            }
        }
        //open stat info if occupied

        /*if (CurrentTile.isOccupied)
        {
            GameUI.UpdateStatUI(CurrentTile.character);
        }
        else
        {
            GameUI.ShowStatUI(false);
        }*/
    }
}
