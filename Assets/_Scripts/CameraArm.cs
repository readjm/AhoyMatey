using UnityEngine;
using System.Collections;

public class CameraArm : MonoBehaviour {

    public Camera playerCamera;
    //public float panSpeed = 1f;
    public float scrollSpeed = 2f;
    
    //private Transform target;
    private Player player;
   // private Vector3 armRotation;

    //private float mouseX_LastFrame = 0;
    //private float mouseY_LastFrame = 0;

    private Vector2 firstpoint; //change type on Vector3
    private Vector2 secondpoint;
    private float xAngle = 0.0f; //angle for axes x for rotation
    private float yAngle = 0.0f;
    private float xAngTemp = 0.0f; //temp variable for angle
    private float yAngTemp = 0.0f;
    private float perspectiveZoomSpeed = 0.05f;        // The rate of change of the field of view in perspective mode.
    //private float lastPinchDistance; 

    // Use this for initialization
    void Start()
    {
        //target = GetComponentInParent<Player>().transform;
        //armRotation = transform.rotation.eulerAngles;
        //target = GameObject.FindObjectOfType<Player>().transform.position;

        foreach (Player p in GameObject.FindObjectsOfType<Player>())
        {
            if (p.isLocalPlayer)
            {
                player = p;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && player.isLocalPlayer)
        {
            //Check count touches
            if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                //change the field of view based on the change in distance between the touches.
                this.transform.localScale += new Vector3(0, deltaMagnitudeDiff * perspectiveZoomSpeed, 0);

                // Clamp the field of view to make sure it's between 0 and 180.
                this.transform.localScale = new Vector3(0, Mathf.Clamp(this.transform.localScale.y, 1f, 10f), 0);
            }

            else if (Input.touchCount > 0)
            {
                //Touch began, save position
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    firstpoint = Input.GetTouch(0).position;
                    xAngTemp = xAngle;
                    yAngTemp = yAngle;
                }
                //Move finger by screen
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    secondpoint = Input.GetTouch(0).position;
                    //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
                    xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
                    yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;
                    yAngle = Mathf.Clamp(yAngle, -92f, -1f);
                    //Rotate camera
                    this.transform.localRotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                firstpoint = Input.mousePosition;
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                secondpoint = Input.mousePosition;
                xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
                yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;
                yAngle = Mathf.Clamp(yAngle, -92f, -1f);
                this.transform.localRotation = Quaternion.Euler(yAngle, xAngle, 0.0f);

                firstpoint = secondpoint;
                xAngTemp = xAngle;
                yAngTemp = yAngle;
            }
            else
            {
                this.transform.localScale += new Vector3(0f, Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, 0f);
            }

            //if (playerCamera.transform.position.y < 1f)
            //{
            //    Debug.Log("Low Camera Detected");

            //    firstpoint = Input.GetTouch(0).position;
            //    xAngTemp = xAngle;
            //    yAngTemp = 86f;
            //    this.transform.localRotation = Quaternion.Euler(yAngTemp, xAngTemp, 0.0f);
            //}

            //if (playerCamera.transform.position.y < 0.1f)
            //{
            //    this.transform.localRotation
            //}

            //playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, (Mathf.Clamp(playerCamera.transform.position.y, 0.1f, 100f)), playerCamera.transform.position.z);

        }

    }

}

