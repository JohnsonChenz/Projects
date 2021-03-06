using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpanwer : MonoBehaviour {

	public GameObject Monster1;
	public GameObject Monster2;
	public bool isTriggered;
	public Transform[] Pos;
	public int BonusSpawnTimes;
	public bool BornDetected;
	public bool CannotTriggeredbyCollider;
	public Transform Scene;
	// Use this for initialization
	void Start () {
		isTriggered = false;
		Scene = GameObject.FindGameObjectWithTag ("Scene").transform;
	}


	void OnTriggerEnter(Collider other)
	{
		if (isTriggered || CannotTriggeredbyCollider)
			return;

		if (other.CompareTag ("MainCharater")) {
			Spawn ();
		}
	}

	public void Spawn()
	{
		isTriggered = true;
		for(int i = 0 ; i < Pos.Length ; i++)
		{
			GameObject Obj = Instantiate (Monster1, Pos [i].position, Pos [i].rotation,Scene);
			Obj.SetActive (true);
			if (BornDetected) {
				Obj.GetComponent<AI2> ().player = GameObject.Find ("ybot").transform;
			}
			if (Monster2) {
				GameObject Obj2 = Instantiate (Monster2, Pos [i].position, Pos [i].rotation);
				Obj2.SetActive (true);
				if (BornDetected) {
					Obj2.GetComponent<AI2> ().player = GameObject.Find ("ybot").transform;
				}
			}
		}
		if (BonusSpawnTimes > 0) {
			BonusSpawnTimes--;
			Invoke ("Spawn", 0.5f);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
