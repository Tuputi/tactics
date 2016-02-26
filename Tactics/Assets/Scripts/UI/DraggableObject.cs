using UnityEngine;
using System.Collections;

public class DraggableObject : MonoBehaviour {

    public bool selected;
    bool dragging = false;
    public Vector3 origPosition;


    public void Init(Vector3 orgPos)
    {
        origPosition = orgPos;
    }

    void Update()
    {
        if (!selected)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            origPosition = GetComponent<RectTransform>().localPosition;
        }
        if (Input.GetMouseButton(0) && dragging)
        {

            GetComponent<RectTransform>().position = Input.mousePosition;
            Debug.Log(Input.mousePosition);
            //Input.GetTouch(0).position;
        }
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            ReturnToOrigLocation();
        }
    }

    public void ReturnToOrigLocation()
    {
        transform.localPosition = origPosition;
    }
}
