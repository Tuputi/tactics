using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlidingMenu : MonoBehaviour {

    public GameObject panel;
    public bool visible;

    
    public void Show()
    {
        if (visible)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        RectTransform r = panel.GetComponent<RectTransform>();
        panel.transform.localPosition -= new Vector3(0, r.rect.height, 0);
        visible = true;
    }

    public void Close()
    {
        visible = false;
        RectTransform r = panel.GetComponent<RectTransform>();
        panel.transform.localPosition += new Vector3(0, r.rect.height, 0);
    }
    
    
    
     
}
