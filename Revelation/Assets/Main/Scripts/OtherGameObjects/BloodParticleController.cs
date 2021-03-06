using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
		gameObject.SetActive (true);
		Destroy (gameObject, 1f);
	}
		
	// Update is called once per frame
	void Update () {
		
	}
}
