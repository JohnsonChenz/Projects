using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossEvent : MonoBehaviour {


	public Transform Boss;
	public Transform Player;
	Animator anim;

	public Transform JumpDestinationPos;

	public bool isTriggered;

	public GameObject Cam;

	public Transform[] Rocks;
	// Use this for initialization
	void Start () {
		isTriggered = false;
		anim = Boss.GetComponent<Animator> ();
		Boss.GetComponent<AI2> ().enabled = false;
		Boss.GetComponent<NavMeshAgent> ().enabled = false;
		Boss.gameObject.SetActive (false);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("MainCharater") && !isTriggered) {
			isTriggered = true;
			Boss.gameObject.SetActive (true);
			Trigger ();
			GetComponent<SceneBlocker> ().BlockScene ();
		}
	}

	void Trigger()
	{
		Invoke ("ShakeCam", 2f);
	}

	void ShakeCam()
	{
		Cam.GetComponent<CameraShake> ().shakeMagnitude = 0.08f;
		Cam.GetComponent<CameraShake> ().shakeTime = 6f;
		Cam.GetComponent<CameraShake> ().ShakeIt ();
		Invoke ("RocksUp", 0);
	}

	void RocksUp()
	{
		for (int i = 0; i < Rocks.Length; i++) {
			Rocks [i].transform.GetComponent<Translate> ().ITweenTranslate ();
		}
		Invoke ("BossEventStart", 3f);
	}

	void BossEventStart()
	{
		anim.applyRootMotion = true;
		anim.SetTrigger("JumpDown");

		ITweenFrontMove (Boss.gameObject, 15, 1f, 0.5f);

		Invoke ("OpenAI", 9f);
	}

	void OpenAI()
	{
		Boss.GetComponent<AI2> ().enabled = true;
		Boss.GetComponent<NavMeshAgent> ().enabled = true;
		anim.SetBool ("isRunning", true);
		Boss.GetComponent<BossSpawn> ().Spawn ();
	}

	public void ITweenFrontMove(GameObject gameobject,float Amout,float Duration, float delay)
	{
		Hashtable moveSetting = new Hashtable();
		moveSetting.Add("delay", delay);
		moveSetting.Add("amount", new Vector3(0, 0, Amout));
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);
		iTween.MoveAdd (gameobject, moveSetting);
	}
		
	public void ITweenUpMove(GameObject gameobject,float Amout,float Duration, float delay)
	{
		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("amount", new Vector3(0, Amout, 0));
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);
		iTween.MoveAdd (gameObject, moveSetting);
	}

	// Update is called once per frame
	void Update () {
		if (Boss) {
			if (!Boss.GetComponent<AI2> ().enabled) {
				Boss.GetComponent<Rigidbody> ().velocity = new Vector3 (0, -20, 0);
			}
		}
	}
}
