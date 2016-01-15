using UnityEngine;
using System.Collections;

public class DestroyThis : MonoBehaviour {

	public void Destory()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
