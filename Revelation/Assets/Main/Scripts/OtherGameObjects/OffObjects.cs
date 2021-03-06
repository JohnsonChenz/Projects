using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffObjects : MonoBehaviour {

	public float sec;

	public GameObject[] OpenObj;
	// Use this for initialization
	void Start () {
		
	}


	void OnEnable()
	{
		CancelInvoke ();
		Invoke ("Off", sec);

		if (OpenObj != null) {
			for (int i = 0; i < OpenObj.Length; i++) {
				if (!OpenObj [i].activeInHierarchy) {
					OpenObj [i].SetActive (true);
				}
			}
		}
	}

	void Off()
	{
		if (gameObject.activeInHierarchy) {
			gameObject.SetActive (false);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
