using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBallAttack : MonoBehaviour {

	public Transform Target;
	public GameObject AtkObj;
	public GameObject LtVfx;
	public float AttackFreq;
	public bool CanAtk;
	public TasksManager tasksmanager;
	public float Time;

	// Use this for initialization
	void Start () {
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		Target = GameObject.FindGameObjectWithTag ("MainCharater").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (tasksmanager.GamePaused)
			return;
		
		if (Input.GetKeyDown (KeyCode.H)) {
			On ();
		}


		if (gameObject.activeInHierarchy) {
			gameObject.transform.LookAt (Target);
			if (CanAtk) {
				CanAtk = false;
				GameObject obj = Instantiate (AtkObj, transform.position, transform.rotation);
				CancelInvoke ("CanAtkAgain");
				Invoke ("CanAtkAgain", AttackFreq);
			}
		}
	}

	public void CanAtkAgain()
	{
		CanAtk = true;
	}

	public void On()
	{
		Invoke ("CanAtkAgain", 5f);
		LtVfx.SetActive (true);
		LtVfx.GetComponent<LaserBall> ().ScaleOn = true;
		CancelInvoke ("Off");
		Invoke ("Off", Time);
	}

	public void Off()
	{
		CancelInvoke ("CanAtkAgain");
		CanAtk = false;
		LtVfx.GetComponent<LaserBall> ().ScaleOn = false;
	}
}
