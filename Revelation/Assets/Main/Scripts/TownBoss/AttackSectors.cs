using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSectors : MonoBehaviour {

	public TownBoss townbossAi;

	public string sector;
	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "MainCharater") {
			townbossAi.sector = sector;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
