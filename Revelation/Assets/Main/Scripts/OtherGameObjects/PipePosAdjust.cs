using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePosAdjust : MonoBehaviour {

	public Transform Tar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = Tar.position;
	}
}
