using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour {

	public Transform LKTG;

	void Start () {


	}



	void MoveByPath(){

		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("time", 5.0f);

		moveSetting.Add("easetype", iTween.EaseType.linear);

		moveSetting.Add("path", iTweenPath.GetPath("New Path 1"));



		iTween.MoveTo(this.gameObject , moveSetting);

	}
	// Update is called once per frame
	void Update () {
		//transform.LookAt (LKTG);

		if(Input.GetKeyDown(KeyCode.A))
			{
			GetComponent<Animator> ().SetTrigger ("OK");
			}
	}
}
