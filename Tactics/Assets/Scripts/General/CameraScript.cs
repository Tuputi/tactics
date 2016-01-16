using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {
    
    float viewDistance = 5f;
    bool MoveToTargetBool = false;
    public static CameraScript instance;
    List<GameObject> moveTargets;

    void Awake()
    {
        instance = this;
        moveTargets = new List<GameObject>();
    }

    void Update()
    {
        if(moveTargets.Count > 0)
        {
            if (!MoveToTargetBool)
            {
                GameObject go = moveTargets[0];
                SetMoveTarget(go);
                moveTargets.Remove(go);
            }
        }
    }

    public void SetMoveTarget(GameObject Target)
    {
        if (MoveToTargetBool)
        {
            if(moveTargets.Count > 3)
            {
                moveTargets.Remove(moveTargets[0]);
            }
            moveTargets.Add(Target);
            return;
        }
        MoveToTargetBool = true;
        StartCoroutine(MoveToTarget(Target.transform));
    }

    
    //code by aldonaletto 
    IEnumerator MoveToTarget(Transform myTarget)
    {
        Vector3 targetPos = new Vector3(myTarget.transform.position.x, this.gameObject.transform.position.y, myTarget.transform.position.z);
        Vector3 sourcePos = this.gameObject.transform.position;
        Vector3 destPos = targetPos - transform.forward * viewDistance;
        float i = 0.0f;
        while (i < 1.0f)
        {
            transform.position = Vector3.Lerp(sourcePos, destPos, Mathf.SmoothStep(0, 1f, i));
            i += Time.deltaTime;
            yield return 0;
        }
        MoveToTargetBool = false;
    }
}
