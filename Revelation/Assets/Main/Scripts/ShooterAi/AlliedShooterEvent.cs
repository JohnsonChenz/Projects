using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedShooterEvent : MonoBehaviour {

	public ShooterAi AlliedShooter;
	public float ActivateTime;
	public Transform ShooterSpawnPos;
	public GameObject OriginObj;
	public int[] CheckPoint;
	public BoxCollider[] EnemyTriggers;
	public bool Enabled;
	// Use this for initialization
	void Start () {
		if (!Enabled) {
			AlliedShooter.gameObject.SetActive (false);
			EnemyTriggers [0].enabled = false;
			EnemyTriggers [1].enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.L)) {
			//StartShooterEvent ();
		}
		if (AlliedShooter.currentWP == CheckPoint [0]) {
			WeaponsFree (true);
		}
		if (AlliedShooter.currentWP == CheckPoint [1]) {
			WeaponsFree (false);
		}
		if (AlliedShooter.currentWP == CheckPoint [2]) {
			WeaponsFree (true);
		}
	}

	public void StartShooterEvent()
	{
		OriginObj.SetActive (false);
		AlliedShooter.gameObject.SetActive (true);
		AlliedShooter.transform.position = ShooterSpawnPos.position;
		Invoke ("ActvatedShooter", ActivateTime);
		GameObject.Find ("BlackScreen").GetComponent<ScreenFadeToBlack> ().ZeroSlow ();
		Invoke ("FUKINGASSHOLE", 0.1f);
		Enabled = true;
	}

	public void FUKINGASSHOLE()
	{
		AlliedShooter.GetComponent<Sentence> ().StartSentence ();
		AlliedShooter.GetComponent<Tasks> ().TriggerEvent ();
	}
	public void ActvatedShooter()
	{
		EnemyTriggers [0].enabled = true;
		EnemyTriggers [1].enabled = true;
		AlliedShooter.Activated = true;
		WeaponsFree (false);
	}

	public void WeaponsFree(bool On)
	{
		AlliedShooter.WeaponsFree = On;
	}
}
