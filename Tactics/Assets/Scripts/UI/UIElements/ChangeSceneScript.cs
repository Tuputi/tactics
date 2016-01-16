using UnityEngine;
using System.Collections;

public class ChangeSceneScript : MonoBehaviour {

    public static string MapName;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadScene(){
        if(Application.loadedLevel == 1)
        {
            Application.LoadLevel(0);
        }
        else
        {
            MapName = EditorButtonScript.instance.mapnamefield.text;
            Application.LoadLevel(1);
        }
    }
}
