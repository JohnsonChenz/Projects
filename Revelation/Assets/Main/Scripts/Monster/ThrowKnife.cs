using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : MonoBehaviour {
	
	public int Speed;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 3f);
	}

	void OnEnable()
	{
		Destroy (this.gameObject, 3f);
	}
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * Speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "MainCharater")
		{
			Destroy (this.gameObject);
		}
	}
}
