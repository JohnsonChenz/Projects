using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBlocker : MonoBehaviour {

	public Transform[] Sceneblockers;
	// Use this for initialization
	void Start () {
		
	}

	public void BlockScene()
	{
		for (int i = 0; i < Sceneblockers.Length; i++) {
			Sceneblockers [i].transform.gameObject.SetActive (true);
		}
	}

	public void UnBlockScene()
	{
		for (int i = 0; i < Sceneblockers.Length; i++) {
			Sceneblockers [i].transform.gameObject.SetActive (false);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
