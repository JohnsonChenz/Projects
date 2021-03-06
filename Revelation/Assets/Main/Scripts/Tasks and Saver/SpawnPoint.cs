using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	public bool isUsed = false;
	// Use this for initialization
	void Start () {
		isUsed = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (isUsed)
			return;
		
		if (other.gameObject.tag == "MainCharater") {
			other.gameObject.GetComponent<ybotDamage> ().SpawnPoint = this.transform;
			isUsed = true;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
