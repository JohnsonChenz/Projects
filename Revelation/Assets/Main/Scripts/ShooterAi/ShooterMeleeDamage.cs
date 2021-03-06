using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterMeleeDamage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Collider> ().enabled = false;
	}

	public void DamageOn()
	{
		this.GetComponent<Collider> ().enabled = true;
	}

	public void DamageOff()
	{
		this.GetComponent<Collider> ().enabled = false;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
