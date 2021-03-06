using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSpawner : MonoBehaviour {

	public GameObject[] Shooters;
	public bool IsSpawned = false;
	public bool IsDetected;
	public bool CanTrigger;
	// Use this for initialization
	void Start () {
		//IsSpawned = false;
		if (!IsSpawned) {
			for (int i = 0; i < Shooters.Length; i++) {
				Shooters [i].gameObject.SetActive (false);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (CanTrigger) {
			if (other.gameObject.tag == "MainCharater" || other.gameObject.tag == "AlliedShooter") {
				if (!IsSpawned) {
					SpawnShooter ();
				}
			}
		}
	}

	public void SpawnShooter()
	{
		if (!IsSpawned) {
			IsSpawned = true;
			for (int i = 0; i < Shooters.Length; i++) {
				Shooters [i].gameObject.SetActive (true);

				if (IsDetected) {
					if (Shooters [i].GetComponent<ShooterAi> ()) {
						Shooters [i].GetComponent<ShooterAi> ().State = "Detected";
					}

					if (Shooters [i].GetComponent<CombatShooterAi> ()) {
						Shooters [i].GetComponent<CombatShooterAi> ().State = "Detected";
					}
					if (Shooters [i].GetComponent<CamShooter> ()) {
						Shooters [i].GetComponent<CamShooter> ().State = "Detected";
					}

					if (Shooters [i].GetComponent<AI2> ()) {
						Shooters [i].GetComponent<AI2> ().state = "Detected";
					}
				}
			}
		}

		if (GetComponent<Sentence> ()) {
			GetComponent<Sentence> ().StartSentence ();
		}
		if (GetComponent<Tasks> ()) {
			GetComponent<Tasks> ().TriggerEvent ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
