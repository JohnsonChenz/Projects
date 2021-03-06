using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tasks : MonoBehaviour {

	public bool isTriggered = false;
	public bool isTriggerByCollider = false;
	public bool TipsOn;
	public bool TitleOn;
	public bool TriggerObjOutlineOn;
	public int Level;
	public string Title;
	public string Info;
	public string Tips;

	public float TitleDelay;
	public float TipsDelay;
	public float TriggerObjectDelay;


	public GameObject[] TriggerObject;
	public TasksManager tasksmanager;
	// Use this for initialization
	void Start () {
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("MainCharater")) {
			if (!isTriggered && isTriggerByCollider) {
				TriggerEvent ();
				Invoke ("TriggerObjects", TriggerObjectDelay);
			}
		}
	}

	public void TriggerEvent()
	{
			isTriggered = true;
			if (TipsOn) {
				tasksmanager.UpdateTipsUI (Tips, TipsDelay);
			}

			if (TitleOn) {
				tasksmanager.UpdateTaskUI (Title, TitleDelay);
			}
	}

	void TriggerObjects()
	{
		for (int i = 0; i < TriggerObject.Length; i++) {

			if (TriggerObject [i].GetComponent<HighlighterController> () && TriggerObjOutlineOn) {
				if (!TriggerObject [i].GetComponent<HighlighterController> ().On) {
					TriggerObject [i].GetComponent<HighlighterController> ().On = true;
				}
			}

			if (TriggerObject [i].GetComponent<InteractObjects> ()) {
				if (!TriggerObject [i].GetComponent<InteractObjects> ().Enable) {
					TriggerObject [i].GetComponent<InteractObjects> ().Enable = true;
				} else {
					TriggerObject [i].GetComponent<InteractObjects> ().Enable = false;
				}
			}
		}
	}

}
