using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Monster") {
			other.gameObject.GetComponent<EnemyStat> ().Dead ();
		}
	}

	void OnTriggerExit(Collider other)
	{
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}
