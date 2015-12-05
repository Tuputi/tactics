using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteScript : MonoBehaviour {

    public Dictionary<string,GameObject> notes;


    void Start()
    {
        notes = new Dictionary<string,GameObject>();
        GameObject noteHolder = GameObject.Find("Notes").gameObject;
        for(int i = 0; i < noteHolder.transform.childCount; i++)
        {
            notes.Add(noteHolder.transform.GetChild(i).gameObject.name, noteHolder.transform.GetChild(i).gameObject);
        }
    }

    public void SetFront(string key)
    {
        notes[key].transform.SetAsLastSibling();
    }

   


}
