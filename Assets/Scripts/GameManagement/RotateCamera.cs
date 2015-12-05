using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

    public Transform centrePoint;
    //	public Camera camera;

    // Use this for initialization
    void Start() {
        centrePoint = GameObject.Find("CenterPoint").transform;
        //centrePoint.localPosition = new Vector3 (MapCreatorManager.instance.mapRows/2, 0, MapCreatorManager.instance.mapColumns/2);
    }


    private float degree;
    private float tiltDegree;
    private float angle;
    private int speed = 5;
    //	bool lerpingTime = true;
    float stopped;
    public float zoom;


    //touch camera
   /* private Vector2 touchStartPos;

    private float minZoom = 0.5f;
    private float maxZoom = 5f;
    private float zoomSpeed = 0.05f;

    public UnityEngine.UI.Image testImage;
    */

void Update() 
	{
        //touch module
       /* if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPos = Input.GetTouch(0).position;
                }
                else if (touch.phase == TouchPhase.Moved && ((Mathf.Abs(touchStartPos.magnitude - touch.position.magnitude)) > 10f))
                {
                    Vector2 touchDeltaPos = Input.GetTouch(0).deltaPosition;
                    //this.gameObject.transform.Translate(-touchDeltaPos.x * 0.1f, 0, -touchDeltaPos.y * 0.1f);
                    this.gameObject.transform.position += new Vector3(-touchDeltaPos.x * 0.05f, 0, -touchDeltaPos.y * 0.05f);
                    Vector3 pos = this.gameObject.transform.position;
                    pos.x = Mathf.Clamp(pos.x, 0, 15);
                    this.gameObject.transform.position = pos;
                    Vector3 posZ = this.gameObject.transform.position;
                    posZ.z = Mathf.Clamp(pos.z, 0, 15);
                    this.gameObject.transform.position = pos;

                }
            }
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (touch1.deltaPosition.magnitude < 0.4f)
                {
                    testImage.color = Color.blue;
                    float rotationChange = gameObject.transform.rotation.y + touch2.deltaPosition.y;
                    this.gameObject.transform.RotateAround(this.gameObject.transform.position, transform.up, touch2.deltaPosition.y * 0.5f);
                    //this.gameObject.transform.GetChild(0).gameObject.transform.RotateAround(this.gameObject.transform.GetChild(0).gameObject.transform.position, transform.right, touch2.deltaPosition.x * 0.5f);
                }
                else
                {
                    //UnityEngine.UI.Image image = GameObject.Find("MyImage").GetComponent<UnityEngine.UI.Image>();
                    testImage.color = Color.red;

                    Vector2 touch1PreviousPOs = touch1.position - touch1.deltaPosition;
                    Vector2 touch2PreviousPOs = touch2.position - touch2.deltaPosition;

                    float prevTouchMagnitude = (touch1PreviousPOs - touch2PreviousPOs).magnitude;
                    float touchDeltaMagnitude = (touch1.position - touch2.position).magnitude;

                    float deltaMagDif = prevTouchMagnitude - touchDeltaMagnitude;

                    this.gameObject.transform.position += new Vector3(0, deltaMagDif * zoomSpeed, 0);
                    Vector3 pos = this.gameObject.transform.position;
                    pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);
                    this.gameObject.transform.position = pos;

                    //this.GetComponentInChildren<Camera>().orthographicSize += deltaMagDif * zoomSpeed;
                  //  this.GetComponentInChildren<Camera>().orthographicSize = Mathf.Clamp(this.GetComponent<Camera>().orthographicSize, minZoom, maxZoom);
                    
                }
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.A)){
			RotateLeft();
		}

		if(Input.GetKeyDown(KeyCode.D)){
			RotateRight();
		}

		if(Input.GetKeyDown(KeyCode.W)){
			TiltUp();
		}

		if(Input.GetKeyDown(KeyCode.S)){
			TiltDown();
		}

		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
//			zoom += 1f;
//			centrePoint.localPosition = new Vector3(0,6,zoom);
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			transform.Translate(Vector3.back * speed * Time.deltaTime);
//			zoom -= 1f;
//			centrePoint.localPosition = new Vector3(0,6,zoom);
		}


//		if (Input.GetMouseButtonDown (2)) {
//			Debug.Log("Rotate");
//			if(!lerpingTime){
//			}
//			degree += 45;
//			if(!lerpingTime){
//				degree+=stopped;
//				stopped = 0;
//			}
//			lerpingTime = true;
//			transform.rotation = Quaternion.Euler(tiltDegree,degree,0);
//		}
//		
//
//		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
//			if(!(tiltDegree>55f)){ 
//				tiltDegree +=10;
//			}
//			transform.rotation = Quaternion.Euler(tiltDegree,degree,0);
//		}
//		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
//			if(!(tiltDegree<0f)){ 
//				tiltDegree -=10;
//			}
//			transform.rotation = Quaternion.Euler(tiltDegree,degree,0);
//		}
	
	}

	public void RotateRight(){
		degree -= 22.5f;
		transform.rotation = Quaternion.Euler(tiltDegree,degree,0);
	}

	public void RotateLeft(){
		degree += 22.5f;
		transform.rotation = Quaternion.Euler(tiltDegree,degree,0);
	}

	public void TiltUp(){
		if(!(tiltDegree>55f)){ 
			tiltDegree +=10;
		}
		transform.rotation = Quaternion.Euler(tiltDegree,degree,0);
	}

	public void TiltDown(){
		//if(!(tiltDegree>55f)){ 
			tiltDegree -=10;
		//}
		transform.rotation = Quaternion.Euler(tiltDegree,degree,0);
	}


}
