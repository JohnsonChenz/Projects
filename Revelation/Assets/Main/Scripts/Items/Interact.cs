using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {


	public string interactButton;

	public float interactDistance = 3f;
	public float interactObjectsDistance = 3f;
	public LayerMask interactLayer;


	public bool isInteracting;
	public CharaterInventory charaterinventory;
	public CharaterIK characterIk;
	public ActorEvent actorevent;
	public MoveControl movecontrol;
	public Transform MainCharacter;

	public GameObject InteractIcon;
	GameObject EmptyGameObject;

	public float hitradius = 1f;

	public bool ishit;

	public Vector3 p2;

	//public Transform transform;

	public Vector3 Origin;
	public Vector3 Direction;

	// Use this for initialization
	void Start () {
		hitradius = 1f;
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		movecontrol = MainCharacter.GetComponent<MoveControl> ();
		actorevent = MainCharacter.GetComponent<ActorEvent> ();
		charaterinventory = MainCharacter.GetComponent<CharaterInventory> ();
		characterIk = MainCharacter.GetComponent<CharaterIK> ();
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray(transform.position, transform.forward);

		RaycastHit hit;

		if (movecontrol.charaterstatus.isDoAction || charaterinventory.CanvasInventory.GetComponent<Canvas> ().enabled)
			return;


		Origin = transform.position;
		Direction = transform.forward;

		p2 = Origin + Direction * interactDistance;

		if (Physics.SphereCast (Origin, hitradius, Direction, out hit, interactDistance, interactLayer, QueryTriggerInteraction.UseGlobal)) {
			ishit = true;
		}
		else
		{
			ishit = false;
		}

		if (Physics.SphereCast (Origin, hitradius, Direction, out hit, interactDistance, interactLayer, QueryTriggerInteraction.UseGlobal)) {


			if (hit.collider.GetComponent<Item> ()) {
				if (hit.collider.GetComponent<Item> ().CountAble) {
					InteractIcon.SetActive (true);
					InteractIcon.transform.GetChild (1).GetComponent<Text> ().text = hit.collider.GetComponent<Item> ().InventoryName + " (" + hit.collider.GetComponent<Item> ().count.ToString () + ")";
				} else if (!hit.collider.GetComponent<Item> ().CountAble) {
					InteractIcon.SetActive (true);
					InteractIcon.transform.GetChild (1).GetComponent<Text> ().text = hit.collider.GetComponent<Item> ().InventoryName;
				}
			}

			if (hit.collider.GetComponent<InteractObjects> ()) {
				if (Vector3.Distance (MainCharacter.position, hit.collider.GetComponent<InteractObjects> ().pivot.position) <= hit.collider.GetComponent<InteractObjects> ().InteractDistance) {
					InteractIcon.SetActive (true);
					InteractIcon.transform.GetChild (1).GetComponent<Text> ().text = hit.collider.GetComponent<InteractObjects> ().interactname;
				} else {
					InteractIcon.SetActive (false);
					InteractIcon.transform.GetChild (1).GetComponent<Text> ().text = null;
				}
			}

			// 門
			if (Input.GetButtonDown (interactButton)) {

				if (hit.collider.GetComponent<InteractObjects> () && hit.collider.GetComponent<InteractObjects> ().Enable && !movecontrol.charaterstatus.isAiming && !movecontrol.charaterstatus.isReloading && !movecontrol.charaterstatus.isSwitching) {
					if (Vector3.Distance (MainCharacter.position, hit.collider.GetComponent<InteractObjects> ().pivot.position) <= hit.collider.GetComponent<InteractObjects> ().InteractDistance) 
					{
						if (hit.collider.GetComponent<Pullrod> ())
						{
							movecontrol.ITweenPosMove (MainCharacter.gameObject, hit.collider.GetComponent<Pullrod> ().PosAdjust, 0.2f);
							movecontrol.ITweenRotateTo (MainCharacter.gameObject, hit.collider.GetComponent<Pullrod> ().RotAdjust, 0.2f);
							actorevent.PullRodTarget = hit.collider.transform;
							actorevent.PullRodStart();
						} 
						else 
						{
							actorevent.PullRodTarget = null;
						}

						if (hit.collider.GetComponent<KeyTask> ()) {
							hit.collider.GetComponent<KeyTask> ().charaterinventory = charaterinventory;
							hit.collider.GetComponent<KeyTask> ().CheckKey ();
						} 

						if (hit.collider.GetComponent<ColorTasks> () && hit.collider.GetComponent<ColorTasks> ().isMachine) {
							hit.collider.GetComponent<ColorTasks> ().TriggerMachine ();
						} 

						if (hit.collider.GetComponent<PassWord> ()) {
							hit.collider.GetComponent<Sentence> ().StartSentence ();
							hit.collider.GetComponent<PassWord> ().PassWordUi.SetActive (true);
						} 

						if (hit.collider.GetComponent<Chest> ()) {
							MainCharacter.GetComponent<Animator> ().SetTrigger ("OpenChest");
							movecontrol.mc = false;
							movecontrol.charaterstatus.isDoAction = true;
							movecontrol.CoverWeapon ();
							hit.collider.GetComponent<Chest> ().GiveAward ();
							hit.collider.GetComponent<Chest> ().Cover.GetComponent<Rotate> ().ITweenRotate ();
							movecontrol.ITweenPosMove (MainCharacter.gameObject, hit.collider.GetComponent<Chest> ().PosAdjust, 0.2f);
							movecontrol.ITweenRotateTo (MainCharacter.gameObject, hit.collider.GetComponent<Chest> ().RotAdjust, 0.2f);
							//hit.collider.GetComponent<PassWord> ().PassWordUi.SetActive (true);
						} 

						if (hit.collider.GetComponent<KickDoor> ()) {
							MainCharacter.GetComponent<Animator> ().SetTrigger ("Kick");
							movecontrol.mc = false;
							movecontrol.charaterstatus.isDoAction = true;
							movecontrol.CoverWeapon ();
							hit.collider.GetComponent<KickDoor> ().Kickdoor ();
							movecontrol.ITweenPosMove (MainCharacter.gameObject, hit.collider.GetComponent<KickDoor> ().PosAdjust, 0.2f);
							movecontrol.ITweenRotateTo (MainCharacter.gameObject, hit.collider.GetComponent<KickDoor> ().RotAdjust, 0.2f);
							//hit.collider.GetComponent<PassWord> ().PassWordUi.SetActive (true);
						} 

						if (hit.collider.GetComponent<Botton> ()) {
							hit.collider.GetComponent<Botton> ().Press ();
						} 

						if (hit.collider.GetComponent<SwitchCamera> () && hit.collider.GetComponent<InteractObjects> ().InteractDelay == 0) {
							hit.collider.GetComponent<SwitchCamera> ().SwitchToCamPost ();
							if (hit.collider.GetComponent<Sentence> () && hit.collider.name != "MachineMid") {
								hit.collider.GetComponent<Sentence> ().StartSentence ();
							}
						}

						if (hit.collider.GetComponent<GiveChips> ()) {
							//hit.collider.GetComponent<GiveChips> ().giveChips ();
						} 

						if (hit.collider.GetComponent<ChipsTask> ()) {
							if (!hit.collider.GetComponent<ChipsTask> ().CanUse) {
								hit.collider.GetComponent<ChipsTask> ().CheckChips ();
							} else {
								if (!hit.collider.GetComponent<ChipsTask> ().IsUsing) {
									hit.collider.GetComponent<ChipsTask> ().Use ();
								}
							}
						}
					} 
					else
					{
						actorevent.PullRodTarget = null;
					}
				}


				if(hit.collider.GetComponent<Item> ())
				{
					charaterinventory.AddItems (hit.collider.gameObject);
					charaterinventory.anim.SetTrigger("IsPickup"); 
					hit.collider.gameObject.SetActive (false);
					//Destroy (hit.collider.gameObject);
				}
				//else if (hit.collider.GetComponent<Item> ().typeItem == "Rocks") 
				//{
				//IfPickupRocks = true;
				//charaterinventory.item.Add (hit.collider.GetComponent<Item> ());
				//Destroy (hit.collider.gameObject);
				//}
				//charaterinventory.SortItem ();


			}

		} else {
			if (InteractIcon.activeInHierarchy) {
				//InteractIcon.transform.GetChild (0).GetComponent<Image> ().sprite = null;
				InteractIcon.transform.GetChild (1).GetComponent<Text> ().text = null;
				InteractIcon.SetActive (false);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Debug.DrawLine (Origin, p2);
		Gizmos.DrawWireSphere (p2, hitradius);
	}
}
