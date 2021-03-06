using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {

	public CharaterInventory charaterinventory;
	public CharaterIK characterIk;
	public ActorEvent actorevent;
	public MoveControl movecontrol;
	public Transform MainCharacter;

	public GameObject[] StartItems;
	public bool Gave;
	// Use this for initialization
	void Start () {
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		movecontrol = MainCharacter.GetComponent<MoveControl> ();
		actorevent = MainCharacter.GetComponent<ActorEvent> ();
		charaterinventory = MainCharacter.GetComponent<CharaterInventory> ();
		characterIk = MainCharacter.GetComponent<CharaterIK> ();
		if (!Gave) {
			Invoke ("GiveItem", 0.1f);
		}
	}

	void GiveItem()
	{
		Gave = true;
		for (int i = 0; i < StartItems.Length; i++) {
			if (StartItems [i]) {
				charaterinventory.AddItems (StartItems [i]);
				//Destroy (StartItems [i]);
				charaterinventory.Weaponcount = 0;
				charaterinventory.anim.SetInteger ("Switch", 0);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
