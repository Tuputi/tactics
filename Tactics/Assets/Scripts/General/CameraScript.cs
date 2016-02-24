using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {
    
    float viewDistance = 7.5f;
    public bool MoveToTargetActive = false;
    public static CameraScript instance;
    List<GameObject> moveTargets;

    private GameObject shell;

    void Awake()
    {
        instance = this;
        moveTargets = new List<GameObject>();
    }

    void Update()
    {
        if(moveTargets.Count > 0)
        {
            if (!MoveToTargetActive)
            {
                GameObject go = moveTargets[0];
                SetMoveTarget(go);
                moveTargets.Remove(go);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(shell == null)
        {
            shell = new GameObject();
        }
        shell.transform.position = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z + 0.5f);
        //Debug.Log(collision.gameObject.transform.position);
        SetMoveCameraFromCollision(shell);
    }

    public void SetMoveTarget(GameObject Target)
    {

        if (MoveToTargetActive)
        {
            if(moveTargets.Count > 3)
            {
                moveTargets.Remove(moveTargets[0]);
            }
            moveTargets.Add(Target);
            return;
        }
        MoveToTargetActive = true;
        StartCoroutine(MoveToTarget(Target.transform));
    }

    public void SetMoveCameraFromCollision(GameObject Target)
    {
        if (MoveToTargetActive)
        {
            if (moveTargets.Count > 3)
            {
                moveTargets.Remove(moveTargets[0]);
            }
            moveTargets.Add(Target);
            return;
        }
        MoveToTargetActive = true;
        StartCoroutine(MoveCameraFromCollision(Target.transform));
    }


    //code by aldonaletto 
    IEnumerator MoveToTarget(Transform myTarget)
    {

        Vector3 heading = myTarget.position - this.gameObject.transform.position;
        float distance = Vector3.Dot(heading, this.gameObject.transform.forward);

        Vector3 targetPos = new Vector3(myTarget.transform.position.x, this.gameObject.transform.position.y, myTarget.transform.position.z);
        Vector3 sourcePos = this.gameObject.transform.position;
        Vector3 destPos = targetPos - transform.forward * distance;


       // destPos += transform.right * (viewDistance);
        float i = 0.0f;
        while (i < 1.0f)
        {
            transform.position = Vector3.Lerp(sourcePos, destPos, Mathf.SmoothStep(0, 1f, i));
            i += Time.deltaTime;
            yield return 0;
        }
        MoveToTargetActive = false;
    }

    IEnumerator MoveCameraFromCollision(Transform myTarget)
    {
        Vector3 targetPos = new Vector3(gameObject.transform.position.x, myTarget.gameObject.transform.position.y, gameObject.transform.position.z);
        Vector3 sourcePos = this.gameObject.transform.position;
        Vector3 destPos = targetPos;

        float i = 0.0f;
        while (i < 1.0f)
        {
            transform.position = Vector3.Lerp(sourcePos, destPos, Mathf.SmoothStep(0, 1f, i));
            i += Time.deltaTime;
            yield return 0;
        }
        MoveToTargetActive = false;
    }
}
