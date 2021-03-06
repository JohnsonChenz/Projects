using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour {

	public GameObject CamPre;
	public GameObject CamPost;
	public bool HasSwitched;
	public float Time;
	public bool CanDisableByPress;
	public MoveControl movecontrol;
	public Transform MainCharacter;
	public TasksManager tasksmanager;
	// Use this for initialization
	void Awake () {
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		movecontrol = MainCharacter.GetComponent<MoveControl> ();
		CamPre = GameObject.Find ("Camera").gameObject;
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
	}

	void OnEnable()
	{
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		movecontrol = MainCharacter.GetComponent<MoveControl> ();
	}
	void Start () {
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		movecontrol = MainCharacter.GetComponent<MoveControl> ();
	}

	// Update is called once per frame
	void Update () {
		
		if (CanDisableByPress) {
			if (CamPost.activeInHierarchy) {
				if (Input.GetKeyDown (KeyCode.E)) {
					SwitchToCamPre ();
				}
			}
		}

	}

	public void SwitchToCamPost()
	{
		if (!HasSwitched) {
			CamPre.SetActive (false);
			CamPost.SetActive (true);
			movecontrol.mc = false;
			movecontrol.charaterstatus.isDoAction = true;
			HasSwitched = true;

			if (Time > 0) {
				Invoke ("SwitchToCamPre", Time);
			}

			if (this.GetComponent<GiveChips> ()) {
				this.GetComponent<GiveChips> ().Pipes.SetActive (true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				tasksmanager.GamePaused = true;
				tasksmanager.DenyGamePaused = true;
			}
		}
	}

	public void SwitchToCamPre()
	{
		if (HasSwitched) {
			CamPre.SetActive (true);
			CamPost.SetActive (false);
			movecontrol.mc = true;
			movecontrol.charaterstatus.isDoAction = false;
			HasSwitched = false;


			if (this.GetComponent<GiveChips> ()) {
				this.GetComponent<GiveChips> ().Pipes.SetActive (false);
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				tasksmanager.GamePaused = false;
				tasksmanager.DenyGamePaused = false;
			}
		}
	}

}
