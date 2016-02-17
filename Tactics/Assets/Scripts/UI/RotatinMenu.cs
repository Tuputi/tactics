using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RotatinMenu : MonoBehaviour {

    public GameObject RotationPoint;
    public List<GameObject> ButtonObjects;
    public float speed = 5f;
    public float RotationAmount;
    public static RotatinMenu instance;
    private GameObject rotationHelper;

    void Start()
    {
        PlaceEvenly();
        instance = this;
        rotationHelper = new GameObject();
    }

    public void RotateSlots(float amount)
    {
        for (int i = 0; i < RotationPoint.transform.childCount; i++)
        {
            RotationPoint.transform.GetChild(i).GetComponent<RotatingMenuSlot>().ChangeRotation(amount);
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
        float accumulatedAngle = 360.0f;
        //foreach (GameObject go in ButtonObjects )
        for(int i = 0; i < ButtonObjects.Count-1; i++)
        {
            GameObject newG = Instantiate(ButtonObjects[i].gameObject);
            newG.AddComponent<RotatingMenuSlot>();
            newG.transform.SetParent(RotationPoint.transform);
            newG.transform.localScale = new Vector3(1, 1, 1);
            double cosResult = System.Math.Cos(accumulatedAngle) * distanceFromCenter;
            double sinResult = System.Math.Sin(accumulatedAngle) * distanceFromCenter;
            float cosFloat = (float)cosResult;
            float sinFloat = (float)sinResult;
            newG.transform.localPosition = new Vector3(cosFloat,sinFloat, 0);
            accumulatedAngle -= angleBetweenObjects;
        }

    }

    public void AddActionButtons(List<GameObject> buttons)
    {
        if(RotationPoint.transform.childCount >= buttons.Count)
        {
            for(int i = 0; i < buttons.Count; i++)
            {
                buttons[i].transform.SetParent((RotationPoint.transform.GetChild(i).transform));
                buttons[i].transform.localPosition = new Vector3(0, 0, 0);
                buttons[i].transform.localScale = new Vector3(1, 1, 1);
            }
        }

        RotateTo(180f);
    }

    public void RotateBy(float amount)
    {
        Debug.Log("Rotate by");
        rotationHelper.transform.eulerAngles = RotationPoint.transform.eulerAngles + new Vector3(0, 0, amount);
        StartCoroutine(RotateDial(rotationHelper.transform));
    }

    public void RotateTo(float degree)
    {
        rotationHelper.transform.eulerAngles = new Vector3(0, 0, degree);
        StartCoroutine(RotateDial(rotationHelper.transform));
    }

    IEnumerator RotateDial(Transform myTarget)
    {
        Vector3 sourceRot = RotationPoint.transform.eulerAngles;
        Vector3 targetRot = myTarget.eulerAngles;
        Debug.Log(sourceRot);
        Debug.Log(targetRot);

        float i = 0.0f;
        while (i < 1.0f)
        {
            float previousRotation = RotationPoint.transform.eulerAngles.z;
            RotationPoint.transform.eulerAngles = Vector3.Lerp(sourceRot, targetRot, Mathf.SmoothStep(0, 1f, i));
            float currentRotation = RotationPoint.transform.eulerAngles.z;
            RotateSlots(previousRotation - currentRotation);
            i += Time.deltaTime;
            yield return 0;
        }
        Debug.Log("Done");
    }
}
