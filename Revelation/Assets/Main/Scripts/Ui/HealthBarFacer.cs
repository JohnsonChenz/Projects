using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarFacer : MonoBehaviour {

    public Camera my_camera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(transform.position + my_camera.transform.rotation * Vector3.back, my_camera.transform.rotation * Vector3.up);

    }
}
