using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sentence : MonoBehaviour {

	public bool isTriggered = false;
	public bool isTriggerByCollider = false;
	public string[] sentence;
	public float[] delay;
	public int Count;
	public GameObject SentenceObj;
	// Use this for initialization
	void Start () {
		SentenceObj = GameObject.Find ("Sentence").transform.GetChild (0).gameObject;
		Count = 0;
	}
		
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("MainCharater")) {
			if (!isTriggered && isTriggerByCollider) {
				Count = 0;
				StartSentence ();
			}
		}
	}

	public void StartSentence()
	{
		    CancelInvoke ();
			isTriggered = true;
			if (Count < sentence.Length) {
				Invoke ("OnSentence", delay [Count]);
			}
	}

	public void OnSentence()
	{
		SentenceObj.GetComponent<Text> ().text = sentence [Count];
			Count++;
		if (Count < sentence.Length) {
			StartSentence ();
		} else {
			Invoke ("ClearSentence", delay [Count]);
		}
	}

	public void ClearSentence()
	{
		SentenceObj.GetComponent<Text> ().text = null;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
