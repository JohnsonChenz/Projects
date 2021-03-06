using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjects : MonoBehaviour {


	public string interactname;
	public bool Enable;
	public GameObject InteractIcon;
	public GameObject[] TriggerNextTaskObject;
	public GameObject[] TriggerNextTaskObjectAnim;
	public float InteractDelay;
	public float InteractDistance;

	public SwitchCamera switchcamera;
	public bool CanSwitchCamera;

	public Transform pivot;

	// Use this for initialization
	void Start () {
		InteractIcon = GameObject.Find ("Interaction");
	}

	public void NextObjects()
	{
		Enable = false;
		if (TriggerNextTaskObject.Length > 0) {
			for (int i = 0; i < TriggerNextTaskObject.Length; i++) {
				if (TriggerNextTaskObject [i].GetComponent<InteractObjects> ().Enable) {
					TriggerNextTaskObject [i].GetComponent<InteractObjects> ().Enable = false;
				} else {
					TriggerNextTaskObject[i].GetComponent<InteractObjects> ().Enable = true;
				}
				if (TriggerNextTaskObject [i].GetComponent<HighlighterController> ()) {
					if (TriggerNextTaskObject [i].GetComponent<HighlighterController> ().On) {
						TriggerNextTaskObject [i].GetComponent<HighlighterController> ().On = false;
					} else {
						TriggerNextTaskObject [i].GetComponent<HighlighterController> ().On = true;
					}
				}
			}
		}
		if (TriggerNextTaskObjectAnim.Length > 0) {
			for (int i = 0; i < TriggerNextTaskObjectAnim.Length; i++) {
				if (TriggerNextTaskObjectAnim [i].GetComponent<Rotate> ()) {
					TriggerNextTaskObjectAnim [i].GetComponent<Rotate> ().ITweenRotate ();
				}
				if (TriggerNextTaskObjectAnim [i].GetComponent<Translate> ()) {
					TriggerNextTaskObjectAnim [i].GetComponent<Translate> ().ITweenTranslate ();
				}

				if (TriggerNextTaskObjectAnim [i].GetComponent<HighlighterController> ()) {
					if (TriggerNextTaskObjectAnim [i].GetComponent<HighlighterController> ().On) {
						TriggerNextTaskObjectAnim [i].GetComponent<HighlighterController> ().On = false;
					} else {
						TriggerNextTaskObjectAnim [i].GetComponent<HighlighterController> ().On = true;
					}
				}
			}
		}

		if(GetComponent<Sentence>())
		{
			GetComponent<Sentence> ().StartSentence ();
		}
			
		if(GetComponent<Tasks>())
		{
			GetComponent<Tasks> ().TriggerEvent ();
		}

		if (switchcamera && CanSwitchCamera) {
			switchcamera.gameObject.SetActive (true);
			switchcamera.SwitchToCamPost ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (!Enable) {
			if (this.GetComponent<HighlighterController> ()) {
				this.GetComponent<HighlighterController> ().On = false;
			}
			if (this.GetComponent<Collider> ().enabled) {
				this.GetComponent<Collider> ().enabled = false;
			}
		} else {	
			if (!this.GetComponent<Collider> ().enabled) {
				this.GetComponent<Collider> ().enabled = true;
			}	
		}
	}
}
