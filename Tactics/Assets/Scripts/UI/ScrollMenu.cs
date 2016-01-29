using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollMenu : MonoBehaviour {

    public List<GameObject> ListItems;
    public List<GameObject> ListPositions;
    public GameObject MenuObject;
    public GameObject ScrollParent;

    void Awake()
    {
        SetUp();
    }

    public void SetUp()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject ob = Instantiate(MenuObject);
            ListItems.Add(ob);
        }
        int pos = 0;
        foreach(GameObject go in ListItems)
        {
            go.transform.SetParent(ListPositions[pos].transform);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.gameObject.GetComponent<Image>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            pos++;
        }
    }


    void Update(){
        if(TouchInput.state == TouchInput.TouchState.sDown)
        {
            RotateList(true);
        }
        if(TouchInput.state == TouchInput.TouchState.sUp)
        {
            RotateList(false);
        }
    }

    int currentFirst = 0;
    void RotateList(bool directionDown)
    {
        if (directionDown)
        {
            currentFirst++;
        }
        else
        {
            currentFirst--;
        }

        int howManyPos = 5 - currentFirst;

        for(int i = 0; i < howManyPos; i++)
        {
            ListItems[i].transform.SetParent(ListPositions[currentFirst].transform);
            ListItems[i].transform.localPosition = new Vector3(0, 0, 0);
            currentFirst++;
        }

    }
}
