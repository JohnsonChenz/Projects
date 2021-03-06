using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRigidbody : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.K)) {
			this.gameObject.AddComponent<Rigidbody> ();
			this.GetComponent<Rigidbody> ().AddForce ((transform.forward + -transform.up) * 500f);
		}
	}
}
