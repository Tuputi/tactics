﻿using UnityEngine;
using System.Collections;

public class DraggableObject : MonoBehaviour {

    public bool selected;
    bool dragging = false;
    public Vector3 origPosition;
    Vector3 offset = new Vector2(5, 5);
    Vector2 offset2D = new Vector2(5, 5);



    public void Init(Vector3 orgPos)
    {
        origPosition = orgPos;
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

        if((Input.GetMouseButtonDown(0) && !dragging) || (TouchInput.touchActive && !dragging))
        {
            dragging = true;
            origPosition = GetComponent<RectTransform>().localPosition;
        }
        if(dragging)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                GetComponent<RectTransform>().position = Input.GetTouch(0).position + offset2D;
            }
            else
            {
                GetComponent<RectTransform>().position = Input.mousePosition + offset;
            }

        }
        if(Input.GetMouseButtonUp(0) || !TouchInput.touchActive)
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
