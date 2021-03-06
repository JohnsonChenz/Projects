using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjs : MonoBehaviour {

	public GameObject[] objMove;
	public GameObject[] objRot;
	public bool isTriggered;
	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (isTriggered) {
			return;
		}
		if (other.gameObject.tag == "MainCharater") {
			isTriggered = true;

			if (objMove.Length > 0) {
				for (int i = 0; i < objMove.Length; i++) {
					if (objMove [i].GetComponent<Translate> ()) {
						objMove [i].GetComponent<Translate> ().ITweenTranslate ();
					}
				}
			}

			if (objRot.Length > 0) {
				for (int i = 0; i < objRot.Length; i++) {
					if (objRot [i].GetComponent<Rotate> ()) {
						objRot [i].GetComponent<Rotate> ().ITweenRotate ();
					}
				}
			}
		}

	}
	// Update is called once per frame
	void Update () {
		
	}
}
