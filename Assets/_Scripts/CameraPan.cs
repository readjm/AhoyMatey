﻿using UnityEngine;
using System.Collections;

public class CameraPan : MonoBehaviour {

    public Transform target;

    // Use this for initialization
	void Start ()
    {
        //target = GetComponentInParent<Player>().transform;
        //target = GetComponentInParent<CameraFocus>().transform;
        //target = GameObject.FindObjectOfType<Player>().transform.position;
        //transform.LookAt(target.transform);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void LateUpdate()
    {
        transform.LookAt(target.transform);
    }
}
