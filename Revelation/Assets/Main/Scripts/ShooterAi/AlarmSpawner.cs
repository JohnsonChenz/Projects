using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSpawner : MonoBehaviour {

	public GameObject[] Shooters;
	public bool IsSpawned;
	// Use this for initialization
	void Start () {
		IsSpawned = false;
		for (int i = 0; i < Shooters.Length; i++) {
			Shooters [i].gameObject.SetActive (false);
		}
	}

	public void SpawnShooter()
	{
		if (!IsSpawned) {
			IsSpawned = true;
			for (int i = 0; i < Shooters.Length; i++) {
				Shooters [i].gameObject.SetActive (true);

				if (Shooters [i].GetComponent<ShooterAi> ()) {
					Shooters [i].GetComponent<ShooterAi> ().State = "Detected";
				}

				if (Shooters [i].GetComponent<CombatShooterAi> ()) {
					Shooters [i].GetComponent<CombatShooterAi> ().State = "Detected";
				}
				if (Shooters [i].GetComponent<CamShooter> ()) {
					Shooters [i].GetComponent<CamShooter> ().State = "Detected";
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
