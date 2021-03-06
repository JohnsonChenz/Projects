using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSaver : MonoBehaviour {

	public GameObject Scene;
	public GameObject Scene_saved;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F2)) {
			if (Scene_saved) {
				Destroy (Scene_saved);
			}
			GameObject obj = Instantiate (Scene,null);
			obj.SetActive (false);
			Scene_saved = obj;
		}
		if (Input.GetKeyDown (KeyCode.F6)) {
			Scene.SetActive (false);
			Destroy (Scene);
			GameObject obj = Instantiate (Scene_saved,null);
			obj.SetActive (false);
			Scene = obj;
			obj.SetActive (true);
		}
	}
}
