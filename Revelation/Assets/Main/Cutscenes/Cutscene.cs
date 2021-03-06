using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour {

	public Transform LookAt;

	public bool StartLooking;

	public GameObject nextCam;
	public GameObject ship;
	public bool PlayOnAwake;
	public float FocusTime;
	public bool StartFocus;
	public Vector3 OriginPos;

	public GameObject[] UIs;

	public GameObject MainCharater;
	public CharaterStatus charaterstatus;

	public Transform[] CharaterPos;

	public Collider[] colliders;

	public GameObject shipAxis;

	public Text Credits;

	// Use this for initialization
	void Start () {
		OriginPos = transform.localPosition;
		MainCharater = GameObject.FindGameObjectWithTag ("MainCharater").gameObject;
		if (UIs.Length > 0) {
			for (int i = 0; i < UIs.Length; i++) {
				UIs [i].SetActive (false);
			}
		}

		Invoke ("ClampCharaterMove", 0.1f);
	}


	public void ClampCharaterMove()
	{
		MainCharater.GetComponent<Animator> ().GetComponent<MoveControl> ().mc = false;

		if (charaterstatus) {
			charaterstatus.isDoAction = true;
		}
	}

	public void Disable()
	{
		this.gameObject.GetComponent<Camera> ().enabled = false;
		Destroy (this, 5f);
		nextCam.SetActive (true);
		if (Credits) {
			Destroy (Credits);
		}
	}

	public void Ship()
	{
		ship.SetActive (true);
		this.GetComponent<Animator> ().enabled = false;
		ITweenLookat (gameObject, LookAt, 0.5f, "startlooking");
		Invoke ("Focus", FocusTime);
	}

    public void Focus()
	{
		StartFocus = true;

		Invoke ("OffFocus", 4f);
	}

	public void OffFocus()
	{
		StartFocus = false;
		Invoke ("EndLooking", 2f);
		Invoke ("HitCamShake", 2.5f);
	}

	public void HitCamShake()
	{
		PlayerCamShake (3f, 0.5f);
		Invoke ("CharaterStandUp", 4f);
		Invoke ("Disable", 4.5f);
	}

	void OpenUi()
	{
		if (UIs.Length > 0) {
			for (int i = 0; i < UIs.Length; i++) {
				UIs [i].SetActive (true);
			}
		}
	}

	void Credit()
	{
		Credits.color = new Color (Credits.color.r, Credits.color.g, Credits.color.b, Credits.color.a + 0.01f);
		Invoke ("Credit", Time.deltaTime * 0.3f);
	}

	void OffCredit()
	{
		CancelInvoke ("Credit");
		Credits.color = new Color (Credits.color.r, Credits.color.g, Credits.color.b, Credits.color.a - 0.01f);
		Invoke ("OffCredit", Time.deltaTime * 0.3f);
	}
	// Update is called once per frame
	void Update () {

		if (StartLooking) {
			ITweenLookUpdate (gameObject, LookAt, 2f);
		}

		if (this.GetComponent<Camera> ()) {
			if (StartFocus) {
				if (this.GetComponent<Camera> ().fieldOfView > 4) {
					PlayerCamShake (1f, 4f);
					this.GetComponent<Camera> ().fieldOfView -= Time.deltaTime * 80;
				} else {
					this.GetComponent<Camera> ().fieldOfView = 4;
				}
			} else {
				if (this.GetComponent<Camera> ().fieldOfView < 60) {
					Originpos ();
					this.GetComponent<Camera> ().fieldOfView += Time.deltaTime * 150;
				} else {
					this.GetComponent<Camera> ().fieldOfView = 60;
					Originpos ();
				}
			}
		}
		/*
		if (Input.GetKeyDown (KeyCode.L)) {
			CharaterJumpDown();
		}

		if (Input.GetKeyDown (KeyCode.K)) {
			CharaterStandUp ();
		}
		*/

	}

	public void ShipOpen()
	{
		shipAxis.GetComponent<Rotate> ().ITweenRotate();
	}

	public void CharaterStandUp()
	{
		MainCharater.transform.position = CharaterPos [1].position;
		MainCharater.transform.rotation = CharaterPos [1].rotation;
		MainCharater.GetComponent<Animator> ().CrossFadeInFixedTime("StandUp", 0.0001f);
		//MainCharater.GetComponent<Animator> ().SetTrigger ("StandUp");

		charaterstatus.isDoAction = true;
		MainCharater.GetComponent<Animator> ().GetComponent<MoveControl> ().mc = false;
		MainCharater.GetComponent<Animator> ().applyRootMotion = true;
		colliders [1].enabled = true;
		colliders [0].enabled = false;
	}


	public void CharaterJumpDown()
	{
		MainCharater.transform.position = CharaterPos [0].position;
		MainCharater.transform.rotation = CharaterPos [0].rotation;
		MainCharater.GetComponent<Animator> ().CrossFadeInFixedTime("JumpDown", 0.6f);
		//MainCharater.GetComponent<Animator> ().SetTrigger ("JumpDown");
		charaterstatus.isDoAction = true;
		MainCharater.GetComponent<Animator> ().GetComponent<MoveControl> ().mc = false;
		MainCharater.GetComponent<Animator> ().applyRootMotion = true;
		colliders [0].enabled = true;
		colliders [1].enabled = false;
	}

	public void Originpos()
	{
		transform.position = new Vector3(OriginPos.x,OriginPos.y,OriginPos.z);
	}

	public void ITweenLookat(GameObject gameobject,Transform Target,float Duration, string TwiceMethod)
	{
		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("looktarget", Target);
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);

		moveSetting.Add ("oncomplete", TwiceMethod);
		moveSetting.Add ("oncompleteparams", "end");
		moveSetting.Add ("oncompletetarget", this.gameObject);

		iTween.LookTo(gameobject, moveSetting);
	}

	public void ITweenFrontMove(GameObject gameobject,float Amout,float Duration)
	{
		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("amount", new Vector3(0, 0, Amout));
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);
		iTween.MoveAdd (gameobject, moveSetting);
	}

	public void startlooking()
	{
		StartLooking = true;
	}

	public void EndLooking()
	{
		StartLooking = false;
	}


	public void ITweenLookUpdate(GameObject gameobject,Transform Target,float Duration)
	{
		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("looktarget", Target);
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);


		iTween.LookUpdate(gameobject, moveSetting);
	}

	public void Sentence()
	{
		this.GetComponent<Sentence> ().StartSentence ();
	}

	public void PlayerCamShake(float x,float y)
	{
		GetComponent<CameraShake> ().shakeMagnitude = x * 0.02f;
		GetComponent<CameraShake> ().shakeTime = y;
		GetComponent<CameraShake> ().ShakeIt ();
	}
}
