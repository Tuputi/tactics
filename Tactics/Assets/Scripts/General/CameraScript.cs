using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
    
    float speed = 1f;
    bool MoveToTarget = false;
    public GameObject target = null;
    public static CameraScript instance;

    void Awake()
    {
        instance = this;
    }


    void Update()
    {
        if (MoveToTarget)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
            if(this.gameObject.transform.position.magnitude - target.transform.position.magnitude < 1f)
            {
                Debug.Log("Arrived");
                MoveToTarget = false;
                target = null;
            }
        }
    }

    public void SetMoveTarget(GameObject Target)
    {
        target = Target;
        MoveToTarget = true;
    }
}
