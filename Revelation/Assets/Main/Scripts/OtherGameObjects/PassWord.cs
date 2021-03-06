using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassWord : MonoBehaviour {

	public GameObject PassWordUi;
	public GameObject input1;
	public GameObject input2;
	public GameObject input3;
	public GameObject input4;

	public MoveControl movecontrol;
	public Transform MainCharacter;

	public string pw1;
	public string pw2;
	public string pw3;
	public string pw4;

	public int TextCount;

	public string CorrectWord;
	// Use this for initialization
	void Start () {
		input1.GetComponent<Text> ().text = null;
		input2.GetComponent<Text> ().text = null;
		input3.GetComponent<Text> ().text = null;
		input4.GetComponent<Text> ().text = null;
		TextCount = 0;
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		movecontrol = MainCharacter.GetComponent<MoveControl> ();
	}
	
	// Update is called once per frame

	public void Check()
	{
		if (input1.GetComponent<Text> ().text == pw1 && input2.GetComponent<Text> ().text == pw2 && input3.GetComponent<Text> ().text == pw3 && input4.GetComponent<Text> ().text == pw4) {
			Debug.Log ("Correct");
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			movecontrol.charaterstatus.isDoAction = false;
			movecontrol.mc = true;
			GameObject.Find ("MainCamera").GetComponent<CameraHander> ().CanRotate = true;
			PassWordUi.SetActive (false);
			this.GetComponent<Sentence> ().sentence [0] = CorrectWord;
			this.GetComponent<Sentence> ().Count = 0;
			this.GetComponent<Sentence> ().StartSentence ();
			this.GetComponent<InteractObjects> ().NextObjects ();
		} else {
			Debug.Log ("Wrong");
		}
	}

	public void Inputs(int count)
	{
		TextCount++;
		if (TextCount == 1) {
			input1.GetComponent<Text> ().text = count.ToString();
		}
		if (TextCount == 2) {
			input2.GetComponent<Text> ().text = count.ToString();
		}
		if (TextCount == 3) {
			input3.GetComponent<Text> ().text = count.ToString();
		}
		if (TextCount == 4) {
			input4.GetComponent<Text> ().text = count.ToString();
		}
		if (TextCount == 5) {
			input1.GetComponent<Text> ().text = count.ToString();
			TextCount = 1;
		}
	}

	public void Clear()
	{
		input1.GetComponent<Text> ().text = null;
		input2.GetComponent<Text> ().text = null;
		input3.GetComponent<Text> ().text = null;
		input4.GetComponent<Text> ().text = null;
		TextCount = 0;
	}

	void Update()
	{
		if (PassWordUi.activeInHierarchy) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			movecontrol.charaterstatus.isDoAction = true;
			movecontrol.mc = false;
			GameObject.Find ("MainCamera").GetComponent<CameraHander> ().CanRotate = false;
		} else {
			//Close ();
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Close ();
		}
	}

	public void Close()
	{
		PassWordUi.SetActive (false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		movecontrol.charaterstatus.isDoAction = false;
		movecontrol.mc = true;
		GameObject.Find ("MainCamera").GetComponent<CameraHander> ().CanRotate = true;
	}
}
