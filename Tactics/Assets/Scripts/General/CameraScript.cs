using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {
    
    float viewDistance = 4.5f;
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

    void OnCollisionEnter(Collision collision)
    {
       /* GameObject shell = new GameObject();
        shell.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y + 2f, collision.gameObject.transform.position.z);
        Debug.Log(collision.gameObject.transform.position);
        setMovevTargetAffectOnlyHeight(shell);*/
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
        StartCoroutine(MoveToTarget(Target.transform, false));
    }

    public void setMovevTargetAffectOnlyHeight(GameObject Target)
    {
        if (MoveToTargetBool)
        {
            if (moveTargets.Count > 3)
            {
                moveTargets.Remove(moveTargets[0]);
            }
            moveTargets.Add(Target);
            return;
        }
        MoveToTargetBool = true;
        StartCoroutine(MoveToTarget(Target.transform, true));
    }


    //code by aldonaletto 
    IEnumerator MoveToTarget(Transform myTarget, bool changeY)
    {
        Vector3 targetPos = new Vector3(0, 0, 0);
        if (!changeY)
        {
             targetPos = new Vector3(myTarget.transform.position.x, this.gameObject.transform.position.y, myTarget.transform.position.z);
        }
        else
        {
             targetPos = new Vector3(myTarget.transform.position.x, myTarget.gameObject.transform.position.y, myTarget.transform.position.z);
        }
        Vector3 sourcePos = this.gameObject.transform.position;
        Vector3 destPos = targetPos - transform.forward * viewDistance;
        destPos += transform.right * (viewDistance);
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
