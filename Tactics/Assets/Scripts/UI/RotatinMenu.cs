using UnityEngine;
using System.Collections;

public class RotatinMenu : MonoBehaviour {

    public GameObject RotationPoint;
    public float speed = 5f;
    public float RotationAmount;

    void Update()
    {
       // if(TouchInput.state == TouchInput.TouchState.sDown)
       if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            RotationPoint.transform.Rotate(new Vector3(0, 0, RotationAmount), Space.Self);
            for(int i = 0; i < RotationPoint.transform.childCount; i++)
            {
                RotationPoint.transform.GetChild(i).GetComponent<RotatingMenuSlot>().ChangeRotation(-RotationAmount);
            }  
            TouchInput.state = TouchInput.TouchState.still;
        }
       // if (TouchInput.state == TouchInput.TouchState.sUp)
       if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotationPoint.transform.Rotate(new Vector3(0, 0, -RotationAmount), Space.Self);
            for (int i = 0; i < RotationPoint.transform.childCount; i++)
            {
                RotationPoint.transform.GetChild(i).GetComponent<RotatingMenuSlot>().ChangeRotation(RotationAmount);
            }
            TouchInput.state = TouchInput.TouchState.still;
        }
    }

}
