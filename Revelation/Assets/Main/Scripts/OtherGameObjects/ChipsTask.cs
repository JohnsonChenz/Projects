using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipsTask : MonoBehaviour {

	public GameObject A;
	public GameObject B;
	public GameObject C;
	public GameObject D;

	public GameObject Chip_A;
	public GameObject Chip_B;
	public GameObject Chip_C;
	public GameObject Chip_D;

	public int COUNT_A = 1;
	public int COUNT_B = 1;
	public int COUNT_C = 1;
	public int COUNT_D = 1;

	public int Seletion = 1;
	public bool cooldown;
	public bool Pass = false;
	public GameObject Camera;
	public GameObject Camera_Chips;
	public GameObject Door;
	public GameObject MainCharater;
	public CharaterInventory charaterinventory;
	public TasksManager tasksmanager;

	public bool IsUsing;
	public bool CanUse = false;
	public int ChipCount;

	public string Correct;
	public string Wrong;
	public string Complete;
	// Use this for initialization
	void Start () {
		if (Camera.activeInHierarchy) {
			Camera.SetActive (false);
		}
		if (Camera_Chips.activeInHierarchy) {
			Camera_Chips.SetActive (false);
		}
		MainCharater = GameObject.Find ("ybot").gameObject;
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		charaterinventory = MainCharater.GetComponent<CharaterInventory> ();
	}


	// Update is called once per frame
	void Update () {

		if (IsUsing) {
			this.GetComponent<Collider> ().enabled = false;
			this.GetComponent<HighlighterController> ().On = false; 
			tasksmanager.GamePaused = true;
			tasksmanager.DenyGamePaused = true;
			if (Input.GetKeyDown (KeyCode.F)) {
				UnUse ();
			}
		}
			
		if (cooldown == true || Pass == true || !IsUsing) {
			A.GetComponent<HighlighterController> ().On = false;
			B.GetComponent<HighlighterController> ().On = false;
			C.GetComponent<HighlighterController> ().On = false;
			D.GetComponent<HighlighterController> ().On = false;
			return;
		}



		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Seletion--;
			if (Seletion < 1) {
				Seletion = 4;
			}
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			Seletion++;
			if (Seletion > 4) {
				Seletion = 1;
			}
		}

		if (Seletion == 1) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				COUNT_A++;
				if (COUNT_A > 4) {
					COUNT_A = 1;
				}
				A.GetComponent<Rotate> ().ITweenRotate();
				cooldown = true;
				Invoke ("CoolDown", 1.2f);
			}
			A.GetComponent<HighlighterController> ().On = true;
		} else {
			A.GetComponent<HighlighterController> ().On = false;
		}
		if (Seletion == 2) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				COUNT_B++;
				if (COUNT_B > 4) {
					COUNT_B = 1;
				}
				B.GetComponent<Rotate> ().ITweenRotate();
				cooldown = true;
				Invoke ("CoolDown", 1.2f);
			}
			B.GetComponent<HighlighterController> ().On = true;
		} else {
			B.GetComponent<HighlighterController> ().On = false;
		}
		if (Seletion == 3) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				COUNT_C++;
				if (COUNT_C > 4) {
					COUNT_C = 1;
				}
				C.GetComponent<Rotate> ().ITweenRotate();
				cooldown = true;
				Invoke ("CoolDown", 1.2f);
			}
			C.GetComponent<HighlighterController> ().On = true;
		} else {
			C.GetComponent<HighlighterController> ().On = false;
		}
		if (Seletion == 4) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				COUNT_D++;
				if (COUNT_D > 4) {
					COUNT_D = 1;
				}
				D.GetComponent<Rotate> ().ITweenRotate();
				cooldown = true;
				Invoke ("CoolDown", 1.2f);
			}
			D.GetComponent<HighlighterController> ().On = true;
		} else {
			D.GetComponent<HighlighterController> ().On = false;
		}
	}

	void CoolDown()
	{
		cooldown = false;
		Check ();
	}

	void Check()
	{
		if (COUNT_A == 2 && COUNT_B == 4 && COUNT_C == 2 && COUNT_D == 4) {
			Pass = true;
			Invoke ("OpenDoor", 2);
		}
	}

	void OpenDoor()
	{
		Camera.SetActive (true);
		Door.GetComponent<Rotate> ().ITweenRotate ();
		Invoke ("Off", 7);
	}

	void Off()
	{
		UnUse ();
		Camera.SetActive (false);
		this.GetComponent<Sentence> ().sentence [0] = Complete;
		this.GetComponent<Sentence> ().Count = 0;
		this.GetComponent<Sentence> ().StartSentence ();	
		this.GetComponent<Tasks> ().TriggerEvent ();
	}

	public void CheckChips()
	{
		if (charaterinventory.OtherCell.childCount > 0) {
			for (int i = 0; i < charaterinventory.OtherCell.childCount; i++) {
				Drag drag = charaterinventory.OtherCell.GetChild (i).GetComponent<Drag> ();
				if (drag.item.nameItem == "Chip_1") {
					charaterinventory.NoSpawnRemove (drag);
					Chip_A.SetActive (true);
					A.GetComponent<Renderer>().material.SetColor("_EmissionColor", A.GetComponent<Renderer>().material.color * 2f);
					ChipCount++;
				}
				if (drag.item.nameItem == "Chip_2") {
					charaterinventory.NoSpawnRemove (drag);
					Chip_B.SetActive (true);
					B.GetComponent<Renderer>().material.SetColor("_EmissionColor", B.GetComponent<Renderer>().material.color * 2f);
					ChipCount++;
				}
				if (drag.item.nameItem == "Chip_3") {
					charaterinventory.NoSpawnRemove (drag);
					Chip_C.SetActive (true);
					C.GetComponent<Renderer>().material.SetColor("_EmissionColor", C.GetComponent<Renderer>().material.color * 2f);
					ChipCount++;
				}
				if (drag.item.nameItem == "Chip_4") {
					charaterinventory.NoSpawnRemove (drag);
					Chip_D.SetActive (true);
					D.GetComponent<Renderer>().material.SetColor("_EmissionColor", D.GetComponent<Renderer>().material.color * 2f);
					ChipCount++;
				}

				if (ChipCount == 4) {
					this.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_EmissionColor", this.transform.GetChild(1).GetComponent<Renderer>().material.color * 2f);
					CanUse = true;	
					this.GetComponent<Sentence> ().sentence [0] = Correct;
					this.GetComponent<Sentence> ().Count = 0;
					this.GetComponent<Sentence> ().StartSentence ();	
				} else {
					this.GetComponent<Sentence> ().sentence [0] = Wrong;
					this.GetComponent<Sentence> ().Count = 0;
					this.GetComponent<Sentence> ().StartSentence ();	
				}
			}
		} else {
			this.GetComponent<Sentence> ().sentence [0] = Wrong;
			this.GetComponent<Sentence> ().Count = 0;
			this.GetComponent<Sentence> ().StartSentence ();	
		}
	}

	public void Use()
	{
		charaterinventory.charaterstatus.isDoAction = true;
		IsUsing = true;
		tasksmanager.GamePaused = true;
		tasksmanager.DenyGamePaused = true;
		Camera_Chips.SetActive (true);
	}

	public void UnUse()
	{
		charaterinventory.charaterstatus.isDoAction = false;
		IsUsing = false;
		tasksmanager.GamePaused = false;
		tasksmanager.DenyGamePaused = false;
		Camera_Chips.SetActive (false);
		if (!Pass) {
			this.GetComponent<InteractObjects> ().Enable = true;
		} else {
			this.GetComponent<InteractObjects> ().Enable = false;
		}
	}
}
