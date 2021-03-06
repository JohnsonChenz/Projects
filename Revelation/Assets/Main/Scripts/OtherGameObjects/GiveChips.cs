using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveChips : MonoBehaviour {

	public GameObject Chips_Move;
	public GameObject Chips_Given;
	public GameObject Camera;
	public GameObject Camera_player;
	public GameObject MainCharater;
	public TasksManager tasksmanager;
	public GameObject Pipes;

	// Use this for initialization
	void Start () {
		if (Camera.activeInHierarchy) {
			Camera.SetActive (false);
		}
		Camera_player = GameObject.Find ("Camera").gameObject;
		MainCharater = GameObject.Find ("ybot").gameObject;
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
	}

	public void giveChips()
	{
		if (GetComponent<InteractObjects> ()) {
			this.GetComponent<InteractObjects> ().Enable = false;
		}
		tasksmanager.GamePaused = true;
		tasksmanager.DenyGamePaused = true;
		Camera.SetActive (true);
		Chips_Move.GetComponent<Translate> ().ITweenTranslate ();
		Invoke ("OffCam", 5f);
	}

	public void OffCam()
	{
		tasksmanager.GamePaused = false;
		tasksmanager.DenyGamePaused = false;
		Camera.SetActive (false);
		MainCharater.GetComponent<CharaterInventory> ().AddItems (Chips_Given);
		MainCharater.GetComponent<CharaterInventory> ().charaterstatus.isDoAction = false;
		MainCharater.GetComponent<MoveControl> ().mc = true;
		Chips_Given.SetActive (false);
		this.GetComponent<GiveChips> ().Pipes.SetActive (false);
		Cursor.lockState = CursorLockMode.Locked;
		Camera_player.SetActive (true);
		if (GetComponent<AlliedShooterEvent> ()) {
			GameObject.Find ("BlackScreen").GetComponent<ScreenFadeToBlack> ().Invoke ("Black", 0);
			Invoke ("ActivateShooter", 3f);
		}
	}


	void ActivateShooter()
	{
		if (GetComponent<AlliedShooterEvent> ()) {
			GetComponent<AlliedShooterEvent> ().StartShooterEvent ();
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
