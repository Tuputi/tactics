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
            SceneManager.LoadScene("GameMode");
        }
        else
        {
            MapName = EditorButtonScript.instance.mapnamefield.text;
            SceneManager.LoadScene("MapCreator");
        }
    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }
}
