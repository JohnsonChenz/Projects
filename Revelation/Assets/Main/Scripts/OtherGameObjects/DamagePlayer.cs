using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {

	public float Multiple;
	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "MainCharater") {
			other.gameObject.GetComponent<ybotDamage> ().Multiple = Multiple;
			other.gameObject.GetComponent<ybotDamage> ().lostHP = true;
			other.gameObject.GetComponent<ybotDamage> ().bloodscreen.GiveBloodScreen (1f);
			other.gameObject.GetComponent<ybotDamage> ().bloodscreen.FadeSpeed = 0.1f;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "MainCharater") {
			other.gameObject.GetComponent<ybotDamage> ().Multiple = 1f;
			other.gameObject.GetComponent<ybotDamage> ().lostHP = false;
			other.gameObject.GetComponent<ybotDamage> ().bloodscreen.FadeSpeed = 0.3f;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
