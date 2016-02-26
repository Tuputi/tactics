using UnityEngine;
using System.Collections;

public class DraggableObject : MonoBehaviour {

    public bool selected;
    bool dragging = false;
    public Vector3 origPosition;
    Vector3 offset = new Vector2(5, 5);


    public void Init(Vector3 orgPos)
    {
        origPosition = orgPos;
        gameObject.AddComponent<Collider>();
    }

    void Update()
    {
        if (!gameObject.GetComponent<InventorySlot>().slotSelected)
        {
            return;
        }

        if (!UIManager.instance.IsPointerOverUIObject())
        {
            transform.localPosition = origPosition;
            return;
        }

        //if (TouchInput.touchActive && !dragging)
        if(Input.GetMouseButtonDown(0) && !dragging)
        {
            dragging = true;
            origPosition = GetComponent<RectTransform>().localPosition;
        }
        //if (TouchInput.touchActive && dragging)
        if(dragging)
        {
            GetComponent<RectTransform>().position = Input.mousePosition + offset;
            //Input.GetTouch(0).position;
            //;
        }
        //if (!TouchInput.touchActive)
        if(Input.GetMouseButtonUp(0))
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
