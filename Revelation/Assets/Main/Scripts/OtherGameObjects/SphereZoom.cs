using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereZoom : MonoBehaviour {

	bool inout = true;
	bool inside = false;
	// Use this for initialization
	void Start () {
		//this.transform.localScale = new Vector3 (1f, 1f, 1f) ;
		inout=true;
		Invoke ("stopout", 0.8f);
	}
	
	// Update is called once per frame
	void Update () {
		if (inout == true) {

			this.transform.localScale += new Vector3 (0.5f, 0.5f, 0.5f);

		} 
		if (inside == true) {
			this.transform.localScale -= new Vector3 (0.8f, 0.8f, 0.8f);
		}
		//* Time.deltaTime
	}


	void stopout(){
		inout=false;
		Invoke ("goinside", 5f);
	}

	void goinside(){
		inside = true;
		Invoke ("diee", 0.8f);
	}


	void diee(){
		Destroy (this.gameObject);
		//this.gameObject.SetActive(false);
	}
}
