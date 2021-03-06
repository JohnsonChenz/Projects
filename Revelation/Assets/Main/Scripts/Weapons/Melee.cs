using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour {


	//人物動畫設定
	public GameObject otherObject;
	Animator anim;

	public CharaterStatus charaterstatus;

	public Transform EnemyTarget;

	public Transform InteractPoint;
	public float LockDistance = 10f;

	public Transform Pivot;

	public GameObject WeaponSelf;
	//public GameObject WeaponBodySelf;

	public bool isEquipped;
	public bool isUsing;

	public Vector3 PlayerPosition;
	public Vector3 EnemyPosition;


	public Transform Player;
	public CharaterIK charaterik;
	public CharaterInventory charaterinventory;
	public MoveControl movecontrol;
	public ActorEvent actorevent;
	public LayerMask interactLayer;

	public float degreesPerSecond;
	public float degreesPerSecond2;

	public WeaponProperties WeaponProperties;

	public Transform PutBackTrans;

	public PSMeshRendererUpdater ps;
	public bool IsActiveEffect;
	public ybotDamage yd;
	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		charaterik = Player.GetComponent<CharaterIK> ();
		movecontrol = Player.GetComponent<MoveControl> ();
		actorevent = Player.GetComponent<ActorEvent> ();
		anim = otherObject.GetComponent<Animator> ();
		yd = Player.GetComponent<ybotDamage> ();
		charaterinventory = Player.GetComponent<CharaterInventory> ();
		//isEquipped = false;
		isUsing = false;
	}


	
	// Update is called once per frame
	void Update () {

		if (isEquipped && (!isUsing || !WeaponSelf.activeInHierarchy) || charaterinventory.isInventoryActive) {
			CancelInvoke ();
			anim.SetBool ("isAttack00", false);
		}

		if (!WeaponSelf.activeInHierarchy || !isEquipped || !isUsing || charaterinventory.isInventoryActive) {
			if (IsActiveEffect) {
				if (!actorevent.ActtackChargeOn) {
					actorevent.ActtackChargeOn = false;
				}
				if (ps.IsActive) {
					ps.IsActive = false;
				}
			}
			return;
		}

		if (IsActiveEffect) {
			if (!ps.IsActive) {
				actorevent.ActtackChargeOn = true;
				ps.IsActive = true;
			}
		}

		Ray ray = new Ray(InteractPoint.position, InteractPoint.forward);

		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, LockDistance, interactLayer)) {

			if (!charaterstatus.isLocking) {
				EnemyTarget = hit.collider.GetComponent<Transform> ();
			}
		}


			
			
		if (EnemyTarget != null) 
		{
			
			PlayerPosition = Player.position;
			EnemyPosition = EnemyTarget.position;
			//print(Vector3.Distance(PlayerPosition,EnemyPosition));

			if (Input.GetMouseButtonDown (2) && Vector3.Distance (PlayerPosition, EnemyPosition) < LockDistance && !charaterstatus.isLocking && EnemyTarget.GetComponent<EnemyStat>().health > 0) {


				charaterstatus.isLocking = true;
				//anim.SetBool ("IsLocking", true);

				EnemyTarget.gameObject.GetComponent<HighlighterController> ().On = true;



			} else if (Input.GetMouseButtonDown (2) && charaterstatus.isLocking) {
				
				charaterstatus.isLocking = false;
				EnemyTarget.gameObject.GetComponent<HighlighterController> ().On = false;

			}
	
			if(Vector3.Distance (PlayerPosition, EnemyPosition) > LockDistance)
			{
				if(charaterstatus.isLocking)
				{
				charaterstatus.isLocking = false;
				EnemyTarget.gameObject.GetComponent<HighlighterController> ().On = false;

				}
			}

			if (charaterstatus.isLocking && !charaterinventory.CanvasInventory.GetComponent<Canvas> ().enabled && EnemyTarget.GetComponent<EnemyStat>().health > 0) {

				/*
				Vector3 dirFromMeToTarget = EnemyTarget.position - Player.position;
				dirFromMeToTarget.y = 0.0f;
				Quaternion lookRotation = Quaternion.LookRotation (dirFromMeToTarget);
				Player.rotation = Quaternion.Lerp (Player.rotation, lookRotation, Time.deltaTime * (degreesPerSecond / 360.0f));
                */
				charaterik.targetLook.position = EnemyTarget.position;


			} else {
				//anim.SetBool ("IsLocking", false);
				charaterstatus.isLocking = false;
				EnemyTarget.gameObject.GetComponent<HighlighterController> ().On = false;
				EnemyTarget = null;
				charaterik.targetLook.localPosition = new Vector3(0,0,1000);
			}

		}


		if (Input.GetMouseButtonDown (0) && !Player.GetComponent<MoveControl>().charaterstatus.isDoAction) {
			
			movecontrol.ITweenLookat (Player.gameObject, charaterik.targetLook, 0.1f);
			if (Vector3.Distance (PlayerPosition, EnemyPosition) < LockDistance && EnemyTarget != null) {

			}
			//anim.SetLookAtWeight (.3f, .3f, .3f);
			movecontrol.mc = false;
			anim.SetTrigger ("isMeleeAttack");
			Invoke ("canAtt", 0.4f);
		} else if (Input.GetMouseButtonUp (0)) {
			CancelInvoke ();
			anim.SetBool ("isAttack00", false);
		}

		if (Input.GetMouseButton (0)) {
			if (anim.GetBool ("isAttack00")) {
				movecontrol.ITweenLookat (Player.gameObject, charaterik.targetLook, 0.2f);
			}
		}
		if (Input.GetMouseButtonDown (1) && !Player.GetComponent<MoveControl>().charaterstatus.isDoAction) {
			//actorevent.OnAttackEnter();
			movecontrol.ITweenLookat (Player.gameObject, charaterik.targetLook, 0.1f);
			anim.SetTrigger ("isMeleeAttack2");
		}

		if (Input.GetKeyDown ("f") && yd.ap.fillAmount >= 0.5f && !charaterstatus.isSwitching) {
			anim.SetTrigger ("IsEMP");
			charaterstatus.isDoAction = true;
			yd.isLostAP ();
		}
	}

	public void canAtt(){
		anim.SetBool ("isAttack00", true);
	}

}




