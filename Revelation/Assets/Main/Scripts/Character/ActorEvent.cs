using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorEvent : MonoBehaviour {

	public MoveControl movecontrol;

	public Animator anim;

	public bool WeaponControll;

	public CharaterStatus charaterStatus;
	public CharaterIK charaterik;
	public CharaterInventory charaterinventory;
	public ParticleSystem slash1A;
	public ParticleSystem slash1B;
	public ParticleSystem slash1C;

	public ParticleSystem sp;

	public Transform PullRodTarget;
	//public Rigidbody rigid;
	//public Vector3 rollvec;

	public GameObject SwordEffect;

	public bool ActtackChargeOn;
	public GameObject AttackchargeEffect;
	public GameObject AttackchargeEffect2;
	public GameObject EMP_FX;

	// Use this for initialization

	void Start () {
		WeaponControll = false;
		charaterStatus.isAiming = false;
		charaterStatus.isDeath = false;
		charaterStatus.isLocking = false;
		charaterStatus.isPickuping = false;
		charaterStatus.isReloading = false;
		charaterStatus.isSwitching = false;
		anim = GetComponent<Animator> ();
		charaterik = GetComponent<CharaterIK> ();
		movecontrol = GetComponent<MoveControl> ();
		charaterinventory = GetComponent<CharaterInventory> ();
		//rigid = GetComponent<Rigidbody> ();

	}

	public void OnAttackEnter()
	{
		//movecontrol.mc = false;
		SwordEffect.SetActive (true);
	}

	public void OnAttackExit()
	{
		movecontrol.mc = true;
		SwordEffect.SetActive (false);
	}

	public void WeaponEnable()
	{
		WeaponControll = true;
	}

	public void WeaponDisable()
	{
		WeaponControll = false;
	}

	public void Hit()
	{
		WeaponControll = false;
		movecontrol.mc = false;
		anim.applyRootMotion = true;

	}

	public void HitExit()
	{
		movecontrol.RecoverWeapon ();
		movecontrol.mc = true;
		anim.applyRootMotion = false;
	}

	public void Death()
	{
		charaterStatus.isDeath = true;
		print ("Death");
	}


	public void Slash1A(){
		slash1A.Play ();
	}

	public void Slash1B(){
		slash1B.Play ();
	}

	public void Slash1C(){
		slash1C.Play ();
	}

	public void Standstill(){
		anim.speed = 0;
		Invoke ("MeleeHit", 0.08f);
		//sp.Play ();
	}

	public void MeleeHit(){
		anim.speed = 1;

	}

	public void ResetTrigger(string triggerName){
		anim.ResetTrigger (triggerName);
	}

	public void PullRodStart()
	{
		if (PullRodTarget != null && !PullRodTarget.GetComponent<Pullrod>().isTriggered) {
			
			charaterinventory.charaterstatus.isDoAction = true;
			movecontrol.mc = false;
			PullRodTarget.GetComponent<Pullrod> ().isTriggered = true;
			movecontrol.CoverWeapon ();
			PullRod ();
		}
	}


	void PullRod()
	{
		PullRodTarget.GetComponent<Pullrod> ().mainObject.GetComponent<Animator> ().SetTrigger ("PullRod");
		charaterik.r_Hand_Target = PullRodTarget.GetComponent<Pullrod> ().target;
		charaterik.r_Hand_Target.position = PullRodTarget.GetComponent<Pullrod> ().target.position;
		charaterik.r_Hand_Target.rotation = PullRodTarget.GetComponent<Pullrod> ().target.rotation;
		GetComponent<Animator> ().SetTrigger ("PullLever");
		//ITweenPosMove (gameObject, PullRodTarget.GetComponent<Pullrod> ().standPos, 0.3f);
		//ITweenRotateTo (gameObject, PullRodTarget.GetComponent<Pullrod> ().mainObject, 0.3f);
	}

	public void PullLevelStart()
	{
		charaterik.interactTarget = true;

	}

	public void PullLevelLooseHand()
	{
		charaterik.interactTarget = false;
		charaterik.r_Hand_Target = null;
		charaterStatus.isDoAction = false;
		Invoke ("CanMoving", 1f);
		if (PullRodTarget.GetComponent<InteractObjects> ()) {
			float delay = PullRodTarget.GetComponent<InteractObjects> ().InteractDelay;
			if (delay > 0) {
				PullRodTarget.GetComponent<InteractObjects> ().Invoke ("NextObjects", delay);
			} else {
				PullRodTarget.GetComponent<InteractObjects> ().NextObjects ();
			}
		}
		if (PullRodTarget.GetComponent<Tasks> ()) {
			PullRodTarget.GetComponent<Tasks> ().TriggerEvent ();
		}
		if (PullRodTarget.GetComponent<Sentence> ()) {
			PullRodTarget.GetComponent<Sentence> ().StartSentence ();
		}

		if (PullRodTarget.GetComponent<AlertEvents> ()) 
		{
			PullRodTarget.GetComponent<AlertEvents> ().AlertStart ();
		}
		movecontrol.Invoke ("RecoverWeapon", 1f);
	}

	public void CanMoving()
	{
		movecontrol.mc = true;
	}


	public void PickUpStart()
	{
		charaterStatus.isPickuping = true;
	}

	public void PickUpEnd()
	{
		charaterStatus.isPickuping = false;
	}
	/*
	public void OnAnimatorMove(){


		rollvec += anim.deltaPosition;


	}
    */


	public void OnAttackcharge(){
		if (ActtackChargeOn) {
			Instantiate (AttackchargeEffect, transform.position, Quaternion.identity);
		}
	}

	public void OnAttackcharge2(){
		if (ActtackChargeOn) {
			Instantiate (AttackchargeEffect2, transform.position, transform.rotation);
		}
	}

	public void OnEMPEnter(){
		movecontrol.mc = false;
		anim.applyRootMotion = true;
	}

	public void OnEMPExit(){
		movecontrol.mc = true;
		charaterStatus.isDoAction = false;
		anim.applyRootMotion = false;
	}

	public void AddEMP(){
		Instantiate (EMP_FX, transform.position, transform.rotation);
	}


}
