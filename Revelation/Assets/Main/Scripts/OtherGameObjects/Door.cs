using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {

	public bool open = false;

	public float doorOpenAngle = 90f;
	public float doorClosedAngle = 0f;

	public GameObject PassWordUi;

	public float smooth = 2f;

	public GameObject input1;
	public GameObject input2;
	public GameObject input3;
	public GameObject input4;

	public string pw1;
	public string pw2;
	public string pw3;
	public string pw4;

	public int TextCount;

	public void ChangeDoorState()
	{
		open = !open;
	}


	// Use this for initialization
	void Start () {
		input1.GetComponent<Text> ().text = null;
		input2.GetComponent<Text> ().text = null;
		input3.GetComponent<Text> ().text = null;
		input4.GetComponent<Text> ().text = null;
		TextCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (open) {

			Quaternion targetRotationOpen = Quaternion.Euler (0, doorOpenAngle, 0);
			transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRotationOpen, smooth * Time.deltaTime);
		} 
		else 
		{
			Quaternion targetRotationClosed = Quaternion.Euler (0, doorClosedAngle, 0);
			transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRotationClosed, smooth * Time.deltaTime);

		}
	}

	public void Check()
	{
		if (input1.GetComponent<Text> ().text == pw1 && input2.GetComponent<Text> ().text == pw2 && input3.GetComponent<Text> ().text == pw3 && input4.GetComponent<Text> ().text == pw4) {
			Debug.Log ("Correct");
			ChangeDoorState ();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			PassWordUi.SetActive (false);
		} else {
			Debug.Log ("Wrong");
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


	public void Input(int count)
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


}
