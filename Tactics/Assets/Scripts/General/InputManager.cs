using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

    public GameObject CentrePoint;
    CameraRotationState camState = CameraRotationState.forward;

    enum CameraRotationState { forward, right, back, left}

    //touch
    private Vector2 touchStartPos;

    private float minZoom = 0.5f;
    private float maxZoom = 5f;
    private float zoomSpeed = 0.05f;

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
                        CentrePoint.gameObject.transform.RotateAround(CentrePoint.gameObject.transform.position, transform.up, touch2.deltaPosition.y * 0.5f);
                        //this.gameObject.transform.GetChild(0).gameObject.transform.RotateAround(this.gameObject.transform.GetChild(0).gameObject.transform.position, transform.right, touch2.deltaPosition.x * 0.5f);
                    }
                    else
                    {
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
    }
}
