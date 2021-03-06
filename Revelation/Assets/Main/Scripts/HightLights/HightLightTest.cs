using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HightLightTest : MonoBehaviour {

	public Collider collider2;
	public CharaterInventory charaterinventory;
	public Transform MainCharacter;
	// Update is called once per frame
	void Start()
	{
		collider2 = GetComponent<Collider> ();
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		charaterinventory = MainCharacter.GetComponent<CharaterInventory> ();
	}


	void OnTriggerEnter(Collider other)
	{
		if (!collider2 || other.gameObject.GetComponent<Outline>())
			return;



		if (other.gameObject.layer == LayerMask.NameToLayer("Interactable")) {
			if (other.gameObject.tag == "Door") {
				other.gameObject.GetComponent<HighlighterController> ().On = true;
			}
			else
			{
				if (other.gameObject.GetComponent<Item> ()) {
					if (other.gameObject.GetComponent<Item> ().NonePick) {
						charaterinventory.AddItems (other.gameObject);
						charaterinventory.anim.SetTrigger ("IsPickup"); 
						Destroy (other.gameObject);
					}
				}
				other.gameObject.GetComponent<HighlighterController> ().On = true;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (!collider2 || !other.gameObject.GetComponent<HighlighterController> ())
			return;
		
		if (other.gameObject.GetComponent<HighlighterController> ().On) {
			other.gameObject.GetComponent<HighlighterController> ().On = false;
		}
	}
		
}
