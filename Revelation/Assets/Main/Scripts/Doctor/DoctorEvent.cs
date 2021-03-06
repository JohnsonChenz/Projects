using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorEvent : MonoBehaviour {

	public GameObject[] Blockers;
	public BoxCollider TriggerObj;
	public GameObject SentenceObj;

	public GameObject DoctorEventObj;
	public bool isTriggered;
	public GameObject DoctorObj;
	public GameObject Door_1;
	public GameObject Door_2;
	// Use this for initialization
	void Start () {
		
	}

	void OnDestroy()
	{
		TriggerObj.enabled = true;
		SentenceObj.GetComponent<Sentence> ().StartSentence ();
		SentenceObj.GetComponent<Tasks> ().TriggerEvent ();
	}

	public void doctorEvent()
	{
		for (int i = 0; i < Blockers.Length; i++) {
			Blockers[i].GetComponent<BoxCollider>().enabled = true;
		}
		if(GetComponent<Sentence>())
		{
			GetComponent<Sentence>().StartSentence();
		}
		if(GetComponent<Tasks>())
		{
			GetComponent<Tasks>().TriggerEvent();
		}
		DoctorObj.SetActive(true);
		Door_1.GetComponent<Rotate>().ITweenRotate();
		Door_2.GetComponent<Translate> ().ITweenTranslate ();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "MainCharater" && !isTriggered) {
			doctorEvent ();
			isTriggered = true;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
