using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			this.gameObject.AddComponent<Rigidbody> ();
			this.gameObject.GetComponent<Rigidbody> ().AddForce (-transform.right * 1000f);
		}
	}
}
