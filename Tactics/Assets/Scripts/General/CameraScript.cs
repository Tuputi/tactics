using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
    
    float speed = 1f;
    bool MoveToTargetBool = false;
    public GameObject target = null;
    public static CameraScript instance;

    void Awake()
    {
        instance = this;
    }


    void Update()
    {
        if (MoveToTargetBool)
        {

            StartCoroutine(MoveToTarget(target.transform));
        }
    }

    public void SetMoveTarget(GameObject Target)
    {
        target = Target;
        MoveToTargetBool = true;
    }

    float viewDistance = 5f;
    IEnumerator MoveToTarget(Transform target)
    {
        Vector3 targetPos = new Vector3(target.transform.position.x, this.gameObject.transform.position.y, target.transform.position.z);
        Vector3 sourcePos = this.gameObject.transform.position;
        Vector3 destPos = targetPos - transform.forward * viewDistance;
        float i = 0.0f;
        while (i < 1.0f)
        {
            transform.position = Vector3.Lerp(sourcePos, destPos, Mathf.SmoothStep(0, 1f, i));
            i += Time.deltaTime;
            yield return 0;
            MoveToTargetBool = false;
        }
    }
}
