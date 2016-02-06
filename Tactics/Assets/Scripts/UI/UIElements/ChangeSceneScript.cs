using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour {

    public static string MapName = "testMap";
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(){
        if(SceneManager.GetActiveScene().name == "MapCreator")
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
