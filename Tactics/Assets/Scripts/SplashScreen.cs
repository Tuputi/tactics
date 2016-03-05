using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

    private float currentTime;
    public float WaitTime = 1f;

	void Start () {
	
	}
	
	void Update () {
        currentTime += Time.deltaTime;
        if(currentTime > WaitTime)
        {
            SceneManager.LoadScene("GameMode");
        }
    }
}
