using UnityEngine;
using System.Collections;

public class TacticsInput : MonoBehaviour{

    public static TacticsInput instance;

    public bool DoubleClick = false;
    public bool TargetChanged = false;
    private GameObject PreviouslyClicked = null;



    void Awake()
    {
        instance = this;
    }


    public void RegisterClick(GameObject gObject)
    {
        if (PreviouslyClicked != null)
        {
            if (PreviouslyClicked == gObject)
            {
                TargetChanged = false;
                DoubleClick = true;
                Debug.Log("DoubleClick on "+PreviouslyClicked.name);
            }
            else
            {
                PreviouslyClicked = gObject;
                TargetChanged = true;
                DoubleClick = false;
                return;
            }
        }
        else {
            PreviouslyClicked = gObject;
            TargetChanged = false;
        }
    }

    public void ResetDoubleClick()
    {
        DoubleClick = false;
        PreviouslyClicked = null;
        TargetChanged = false;
    }

    public void CreateDoubleClick()
    {
        DoubleClick = true;
    }

}
