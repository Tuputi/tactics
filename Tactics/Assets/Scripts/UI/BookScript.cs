using UnityEngine;
using System.Collections.Generic;

public class BookScript : MonoBehaviour {

    public List<GameObject> InteractableObjectsWithinBook;
    bool open = false;

	public void OpenBook()
    {
        this.gameObject.GetComponent<Animator>().SetBool("Open", true);
    }

    public void SetInteractable()
    {
        foreach (GameObject g in InteractableObjectsWithinBook)
        {
            g.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    public void SetUninteractable()
    {
        foreach (GameObject g in InteractableObjectsWithinBook)
        {
            g.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    public void CloseBook()
    {
        this.gameObject.GetComponent<Animator>().SetBool("Open", false);
    }

    void OnMouseDown()
    {

        if (open)
        {
            CloseBook();
            open = false;
        }
        else
        {
            OpenBook();
            open = true;
        }
    }
}
