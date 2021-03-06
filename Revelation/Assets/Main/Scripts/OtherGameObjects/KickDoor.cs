using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickDoor : MonoBehaviour {


	public Transform MainCharacter;
	public CharaterInventory charaterinventory;
	public MoveControl movecontrol;

	public GameObject Door;
	public GameObject Cam;
	public Transform PosAdjust;
	public float RotAdjust;

	// Use this for initialization
	void Start () {
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		movecontrol = MainCharacter.GetComponent<MoveControl> ();
		charaterinventory = MainCharacter.GetComponent<CharaterInventory> ();
		Cam = GameObject.Find ("Camera").gameObject;
	}

	public void Kickdoor()
	{
		this.gameObject.layer = 0;
		Invoke ("Addforce", 1f);
		this.GetComponent<Sentence> ().StartSentence ();
	}
		

	public void Addforce()
	{
		Cam.GetComponent<CameraShake> ().shakeMagnitude = 0.03f;
		Cam.GetComponent<CameraShake> ().shakeTime = 0.2f;
		Cam.GetComponent<CameraShake> ().ShakeIt ();
		Door.AddComponent<Rigidbody> ();
		Door.GetComponent<Rigidbody> ().AddForce ((-transform.right + -transform.up) * 800f);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
