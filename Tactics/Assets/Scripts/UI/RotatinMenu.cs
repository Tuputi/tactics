﻿using UnityEngine;
using System.Collections.Generic;

public class RotatinMenu : MonoBehaviour {

    public GameObject RotationPoint;
    public List<GameObject> ButtonObjects;
    public float speed = 5f;
    public float RotationAmount;

    void Start()
    {
        PlaceEvenly();
    }

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
                RotatingMenuSlot tempSlot = RotationPoint.transform.GetChild(i).GetComponent<RotatingMenuSlot>();
                tempSlot.ChangeRotation(RotationAmount);
            }
            TouchInput.state = TouchInput.TouchState.still;
        }
    }

    public void PlaceEvenly()
    {
        float angleBetweenObjects = 360 / ButtonObjects.Count+1;
        RotationAmount = angleBetweenObjects+3f;
        float distanceBetweenObjects = 400f;
        double tempDouble = System.Math.Cos(angleBetweenObjects * 0.5);
        float tempFloat = (float)tempDouble;
        float distanceFromCenter = (distanceBetweenObjects * 0.5f) /tempFloat;
        float accumulatedAngle = 0.0f;
        //foreach (GameObject go in ButtonObjects )
        for(int i = 0; i < ButtonObjects.Count-1; i++)
        {
            GameObject newG = Instantiate(ButtonObjects[i].gameObject);
            newG.transform.SetParent(RotationPoint.transform);
            double cosResult = System.Math.Cos(accumulatedAngle) * distanceFromCenter;
            double sinResult = System.Math.Sin(accumulatedAngle) * distanceFromCenter;
            float cosFloat = (float)cosResult;
            float sinFloat = (float)sinResult;
            newG.transform.localPosition = new Vector3(cosFloat,sinFloat, 0);
            accumulatedAngle += angleBetweenObjects;
        }
    }
}
