using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTask : MonoBehaviour {

	public CharaterInventory charaterinventory;

	public string Correct;
	public string Wrong;
	// Use this for initialization
	void Start () {
		
	}

	public void CheckKey()
	{
		if (charaterinventory.OtherCell.childCount > 0) {
			for (int i = 0; i < charaterinventory.OtherCell.childCount; i++) {
				Drag drag = charaterinventory.OtherCell.GetChild (i).GetComponent<Drag> ();
				if (drag.item.typeItem == "Key") {
					charaterinventory.NoSpawnRemove (drag);
					this.GetComponent<InteractObjects> ().NextObjects ();
					this.GetComponent<Sentence> ().sentence [0] = Correct;
					this.GetComponent<Sentence> ().Count = 0;
					this.GetComponent<Sentence> ().StartSentence ();
					if (this.GetComponent<Camerashake> ()) {
						this.GetComponent<Camerashake> ().Camtarget.GetComponent<CameraShake> ().shakeTime = 6f;
						this.GetComponent<Camerashake> ().Camtarget.GetComponent<CameraShake> ().shakeMagnitude = 0.05f;
						this.GetComponent<Camerashake> ().Camtarget.GetComponent<CameraShake> ().ShakeIt ();
					}
					return;
				} 
				if (drag.item.typeItem != "Key") {
						this.GetComponent<Sentence> ().sentence [0] = Wrong;
						this.GetComponent<Sentence> ().Count = 0;
						this.GetComponent<Sentence> ().StartSentence ();
					print (i);
						Debug.Log ("Wrong");
				}
			}
		} else {
			this.GetComponent<Sentence> ().sentence [0] = Wrong;
			this.GetComponent<Sentence> ().Count = 0;
			this.GetComponent<Sentence> ().StartSentence ();
			Debug.Log ("Wrong");
		}

	}
	// Update is called once per frame
	void Update () {
		
	}
}
