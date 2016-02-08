using UnityEngine;
using System.Collections;

public class RotatingMenuSlot : MonoBehaviour {

	public void ChangeRotation(float amount)
    {
        
        this.gameObject.transform.Rotate(new Vector3(0, 0, amount), Space.Self);    
    }
    public void SetOnTop()
    {
       this.gameObject.transform.SetAsLastSibling();
    }
}
