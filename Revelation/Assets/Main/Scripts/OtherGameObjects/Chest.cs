using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {


	public Transform MainCharacter;
	public CharaterInventory charaterinventory;
	public MoveControl movecontrol;

	public Transform Cover;
	public GameObject[] Awards;
	public Transform PosAdjust;
	public float RotAdjust;
	public float GiveDelay;

	// Use this for initialization
	void Start () {
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		movecontrol = MainCharacter.GetComponent<MoveControl> ();
		charaterinventory = MainCharacter.GetComponent<CharaterInventory> ();
	}

	public void GiveAward()
	{
		this.GetComponent<InteractObjects> ().Enable = false;
		if (GiveDelay > 0) {
			GiveDelay--;
			Invoke ("GiveAward", 1.0f);
		}
		else
		{
			for(int i = 0 ; i < Awards.Length ; i++)
			{
				GameObject obj = Instantiate (Awards [i]) as GameObject;
				obj.GetComponent<Item> ().WeaponOnHand = GameObject.Find (obj.GetComponent<Item> ().ModelName);
				charaterinventory.AddItems(obj);
				//Destroy (obj);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
