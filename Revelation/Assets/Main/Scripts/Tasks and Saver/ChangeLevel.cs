using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour {


	public TasksManager taskmanager;
	public string NextScene;
	public bool IsLoaded;
	public bool NonTrigger;
	// Use this for initialization
	void Start () {
		taskmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("MainCharater") && !IsLoaded && !NonTrigger)
		{
		  IsLoaded = true;
		  taskmanager.SaveItem ();
		  Invoke ("ChangeScene", 6f);
		  GameObject.Find ("BlackScreen").GetComponent<ScreenFadeToBlack> ().Black ();
		}
	}

	public void ForceChangeScene()
	{
		if (!IsLoaded) {
			IsLoaded = true;
			taskmanager.SaveItem ();
			Invoke ("ChangeScene", 6f);
			GameObject.Find ("BlackScreen").GetComponent<ScreenFadeToBlack> ().Black ();
		}

	}
	void ChangeScene()
	{
		SceneManager.LoadScene (NextScene);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.F12)) {
			SceneManager.LoadScene (0);
		}
		if (Input.GetKey (KeyCode.F9)) {
			SceneManager.LoadScene (4);
		}
	}
}
