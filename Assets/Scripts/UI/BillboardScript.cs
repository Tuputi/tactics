using UnityEngine;
using System.Collections;

public class BillboardScript : MonoBehaviour {

    public Camera camera;

    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void Update()
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.back,
            camera.transform.rotation * Vector3.up);
    }
}
