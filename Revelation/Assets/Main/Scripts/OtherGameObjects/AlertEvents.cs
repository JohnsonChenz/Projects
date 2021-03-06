using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertEvents : MonoBehaviour {

	public float Delay;
	public GameObject[] Laser;
	// Use this for initialization
	void Start () {
		
	}

	public void AlertStart()
	{
		Invoke ("Alert", Delay);
		for (int i = 0; i < Laser.Length; i++) {
			Laser [i].SetActive (false);
		}
	}

	public void Alert()
	{
		this.GetComponent<MonsterSpanwer> ().Spawn ();

	}
	// Update is called once per frame
	void Update () {
		
	}
}
