using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour {

	public float RotAdjust;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("MainCharater"))
		{
			other.GetComponent<MoveControl> ().ClimbTarget = this.transform;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("MainCharater"))
		{
			if (!other.GetComponent<MoveControl> ().charaterstatus.isDoAction) {
				other.GetComponent<MoveControl> ().ClimbTarget = null;
			}
		}
	}
}
