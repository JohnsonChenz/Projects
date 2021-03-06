using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyInfo : MonoBehaviour {

	public GameObject InfoObj;
	// Use this for initialization
	void Start () {
		
	}

	void OnDestroy()
	{
		if (InfoObj.GetComponent<Sentence> ()) {
			InfoObj.GetComponent<Sentence> ().StartSentence ();
		}
		if (InfoObj.GetComponent<Tasks> ()) {
			InfoObj.GetComponent<Tasks>().TriggerEvent();
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
