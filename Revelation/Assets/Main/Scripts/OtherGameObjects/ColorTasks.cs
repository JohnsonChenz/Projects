using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTasks : MonoBehaviour {

	public string CurrentColor;

	public Color Correct;
	public GameObject MachineMid;
	public GameObject MachineRight;
	public GameObject MachineLeft;
	public GameObject Tier1Left;
	public GameObject Tier1Right;
	public GameObject Tier2Left;
	public GameObject Tier2Right;
	public GameObject Tier3;
	public float intensify;

	Material mymat;

	public ParticleSystem[] Toxics;
	public GameObject ToxicsDMG;
	public bool isMachine;

	public enum Machine
	{
		Right,Left
	}

	public Machine machine;

	// Use this for initialization
	void Start () {
		mymat = GetComponent<Renderer>().material;
		CurrentColor = "None";
		mymat.SetColor("_EmissionColor", Color.clear);
	}

	public void TriggerMachine()
	{
		if (machine == Machine.Left) {
			if (CurrentColor == "None") {
				Red ();
				Tier1Left.GetComponent<ColorTasks> ().Red ();
				Tier2Left.GetComponent<ColorTasks> ().Blue ();
			}
			else if (CurrentColor == "Red") {
				Blue ();
				Tier1Left.GetComponent<ColorTasks> ().Blue ();
				Tier2Left.GetComponent<ColorTasks> ().Red ();
			}
			else if (CurrentColor == "Blue") {
				Red();
				Tier1Left.GetComponent<ColorTasks> ().Red ();
				Tier2Left.GetComponent<ColorTasks> ().Blue ();
			}
			tier3 ();
		}		
		if (machine == Machine.Right) {
			if (CurrentColor == "None") {
				Cyan ();
				Tier1Right.GetComponent<ColorTasks> ().Cyan ();
				Tier2Right.GetComponent<ColorTasks> ().Blue ();
			}
			else if (CurrentColor == "Cyan") {
				Red ();
				Tier1Right.GetComponent<ColorTasks> ().Red ();
				Tier2Right.GetComponent<ColorTasks> ().Yellow ();
			}
			else if (CurrentColor == "Red") {
				Cyan();
				Tier1Right.GetComponent<ColorTasks> ().Cyan ();
				Tier2Right.GetComponent<ColorTasks> ().Blue ();
			}
			tier3 ();
		}
	}

	public void tier3()
	{
		if(Tier2Left.GetComponent<ColorTasks>().CurrentColor == "Blue" && Tier2Right.GetComponent<ColorTasks> ().CurrentColor == "Blue")
		{
		    Tier3.GetComponent<ColorTasks> ().Blue ();
		}
		else if (Tier2Left.GetComponent<ColorTasks>().CurrentColor == "Blue" && Tier2Right.GetComponent<ColorTasks> ().CurrentColor == "Yellow") {
			Tier3.GetComponent<ColorTasks>().White();
		}
		else if(Tier2Left.GetComponent<ColorTasks>().CurrentColor == "Red" && Tier2Right.GetComponent<ColorTasks> ().CurrentColor == "Blue")
		{
				Tier3.GetComponent<ColorTasks> ().Purple ();

		}
		else if (Tier2Left.GetComponent<ColorTasks>().CurrentColor == "Red" && Tier2Right.GetComponent<ColorTasks> ().CurrentColor == "Yellow") {
			Tier3.GetComponent<ColorTasks>().Green();
		}
		Tier4 ();
	}

	public void Tier4()
	{
		if (Tier3.GetComponent<ColorTasks> ().CurrentColor == "Green") {
			MachineLeft.GetComponent<InteractObjects> ().Enable = false;
			MachineRight.GetComponent<InteractObjects> ().Enable = false;
			MachineMid.layer = 0;
			Invoke ("Pass", 3f);
		}
	}
	public void Pass()
	{
		
		for (int i = 0; i < Toxics.Length; i++) {
			var main = Toxics [i].main;
			if (main.loop == true) {
				main.loop = false;
			}
		}
		if (ToxicsDMG.activeInHierarchy) {
			ToxicsDMG.SetActive (false);
		}
		if (MachineMid.GetComponent<InteractObjects> ().Enable) {
			MachineMid.GetComponent<InteractObjects> ().NextObjects ();
		}
	}


	public void Red()
	{
		CurrentColor = "Red";
		mymat.SetColor("_EmissionColor", Color.clear);
		mymat.SetColor("_EmissionColor", new Color (255, 0, 4) * intensify);
	}

	public void Blue()
	{
		CurrentColor = "Blue";
		mymat.SetColor("_EmissionColor", Color.clear);
		mymat.SetColor("_EmissionColor", new Color (0, 77, 191) * intensify);
	}

	public void Purple()
	{
		CurrentColor = "Purple";
		mymat.SetColor("_EmissionColor", Color.clear);
		mymat.SetColor("_EmissionColor", new Color (191, 27, 170) * intensify);
	}

	public void Cyan()
	{
		CurrentColor = "Cyan";
		mymat.SetColor("_EmissionColor", Color.clear);
		mymat.SetColor("_EmissionColor", new Color (27, 191, 175) * intensify);
	}

	public void Yellow()
	{
		CurrentColor = "Yellow";
		mymat.SetColor("_EmissionColor", Color.clear);
		mymat.SetColor("_EmissionColor", new Color (191, 176, 45) * intensify);
	}

	public void White()
	{
		CurrentColor = "White";
		mymat.SetColor("_EmissionColor", Color.clear);
		mymat.SetColor("_EmissionColor", new Color (255, 255, 255) * intensify);
	}

	public void Green()
	{
		CurrentColor = "Green";
		mymat.SetColor("_EmissionColor", Color.clear);
		mymat.SetColor("_EmissionColor", new Color (27, 191, 33) * intensify);
	}

	void Update () {
		/*
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			Cyan ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			Red ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			Blue ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			Purple ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha8)) {
			White ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			Yellow ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			Green ();
		}
		*/
	}
}
