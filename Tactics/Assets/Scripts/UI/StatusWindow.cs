using UnityEngine;
using System.Collections;

public class StatusWindow : MonoBehaviour {

    public GameObject elementWindow;
    public GameObject miscWindow;


    public void ElementWindowButton()
    {
        miscWindow.SetActive(false);
        elementWindow.SetActive(!elementWindow.activeSelf);
    }

    public void MiscWindowButton()
    {
        elementWindow.SetActive(false);
        miscWindow.SetActive(!miscWindow.activeSelf);
    }

    public void CloseAllTabs() {
        elementWindow.SetActive(false);
        miscWindow.SetActive(false); 
    }

}
