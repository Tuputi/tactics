using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {

    public void DestoryThis()
    {
        Destroy(this.gameObject.transform.parent.gameObject);
    }
}
