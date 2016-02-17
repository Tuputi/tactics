using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

    public GameObject CentrePoint;
    public GameObject RotationPoint;
    public GameObject TiltPoint;
    public static bool rotationOn = false;
    public static bool tiltOn = false;

    //touch
    private Vector2 touchStartPos;

    private float minZoom = 5f;
    private float maxZoom = 15f;
    private float minTilt = 10f;
    private float maxTilt = 85f;
    private float moveSpeed = 0.05f;
    private float zoomSpeed = 0.05f;
    //private float mapRangeX = 10f;
    //private float mapRangeY = 10f;


    void Update()
    {      

        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                if (Input.touchCount == 1)
                {
                    if (rotationOn)
                    {
                        CentrePoint.gameObject.transform.RotateAround(CentrePoint.gameObject.transform.position, transform.up, Input.GetTouch(0).deltaPosition.y * 0.5f);
                        return;
                    }

                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        touchStartPos = Input.GetTouch(0).position;
                    }
                    else if (touch.phase == TouchPhase.Moved && ((Mathf.Abs(touchStartPos.magnitude - touch.position.magnitude)) > 10f))
                    {
                        Vector2 touchDeltaPos = Input.GetTouch(0).deltaPosition;
                        Vector3 move = new Vector3(-touchDeltaPos.x * moveSpeed, 0, -touchDeltaPos.y * moveSpeed);

                        if (!UIManager.instance.IsPointerOverUIObject())
                        {
                            CentrePoint.transform.Translate(move, Space.Self);
                        }

                    }
                }
                if (Input.touchCount == 2)
                {
                    Touch touch1 = Input.GetTouch(0);
                    Touch touch2 = Input.GetTouch(1);

                    if (tiltOn)
                    {
                        Vector2 touch1PreviousPOs = touch1.position - touch1.deltaPosition;
                        Vector2 touch2PreviousPOs = touch2.position - touch2.deltaPosition;

                        float prevTouchMagnitude = (touch1PreviousPOs - touch2PreviousPOs).magnitude;
                        float touchDeltaMagnitude = (touch1.position - touch2.position).magnitude;

                        float deltaMagDif = prevTouchMagnitude - touchDeltaMagnitude;
                        RotationPoint.transform.eulerAngles += new Vector3(deltaMagDif * 0.2f, 0, 0);
                        Vector3 rot = RotationPoint.transform.eulerAngles;
                        rot.x = Mathf.Clamp(rot.x, minTilt, maxTilt);
                        RotationPoint.gameObject.transform.eulerAngles = rot;
                        return;
                    }

                    if (!(touch1.deltaPosition.magnitude < 0.2f))
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

                        RotationPoint.transform.eulerAngles += new Vector3(deltaMagDif * 0.2f, 0, 0);
                        Vector3 rot = RotationPoint.transform.eulerAngles;
                        rot.x = Mathf.Clamp(rot.x, minTilt, maxTilt);
                        RotationPoint.gameObject.transform.eulerAngles = rot;

                        //RotationPoint.transform.Rotate(Vector3.right * deltaMagDif * 0.1f, Space.Self);

                    }
                }
            }
        }
        //touch not supported ie on computer
        else
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            CentrePoint.transform.position += move * moveSpeed;
        }
    }
}
