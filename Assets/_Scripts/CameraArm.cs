using UnityEngine;
using System.Collections;

public class CameraArm : MonoBehaviour {

    public float panSpeed = 10f;
    public float scrollSpeed = 2f;

    //private Transform target;
    private Player player;
    private Vector3 armRotation;

    // Use this for initialization
    void Start()
    {
        //target = GetComponentInParent<Player>().transform;
        armRotation = transform.rotation.eulerAngles;
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
        if (player.isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.Mouse0) && player.isLocalPlayer)
            {
                armRotation.y += Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime;
                armRotation.z += Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime;
                //transform.position = target.position;
                transform.rotation = Quaternion.Euler(armRotation);
            }
            GetComponentInParent<Transform>().localScale += new Vector3(0f, Input.GetAxis("Mouse ScrollWheel")*scrollSpeed, 0f);
        }
    }
}
