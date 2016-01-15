using UnityEngine;
using System.Collections;

public class BillboardScript : MonoBehaviour {

    public new Camera camera;

    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void Update()
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
            camera.transform.rotation * Vector3.up);
    }
}
