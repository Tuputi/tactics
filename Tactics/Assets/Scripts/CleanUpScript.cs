using UnityEngine;
using System.Collections;

public class CleanUpScript : MonoBehaviour {

    public float timer;

	void Start()
    {
        Destroy(this.gameObject, timer);
    }
}
