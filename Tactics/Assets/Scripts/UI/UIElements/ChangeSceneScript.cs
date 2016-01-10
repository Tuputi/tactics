using UnityEngine;
using System.Collections;

public class ChangeSceneScript : MonoBehaviour {

	public void LoadScene(){
        if(Application.loadedLevel == 1)
        {
            Application.LoadLevel(0);
        }
        else
        {
            Application.LoadLevel(1);
        }
    }
}
