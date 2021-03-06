using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ybotDamage : MonoBehaviour {

	public Animator anim;
	public string opponent;
	public bool ybotcanHit = true;
	public Image hp;
	public Image ap;
	//public float fillaa;
	public bool HitBack = false;
	//public GameObject hitbackFX;
	public ActorEvent ac;
	public MoveControl movecontrol;
	public bool lostHP = false;

	bool addAP=false;
	public bool lostAP = false;
	public CharaterInventory charaterinventory;
	public CharaterStatus charaterstatus;
	public float addAmount;
	public float HPTemp;
	public BloodScreen bloodscreen;
	public float Multiple = 1;

	public Transform SpawnPoint;


	public bool OnMete = false;
	public GameObject Ro;
	public Animator Roo;
	public string opponent2;

	public TasksManager tasksmanager;



	// Use this for initialization
	void Start () {
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		anim = GetComponent<Animator> ();
		ac=GetComponent<ActorEvent> ();
		movecontrol = GetComponent<MoveControl> ();
		charaterinventory = GetComponent<CharaterInventory> ();
		charaterstatus = charaterinventory.charaterstatus;
		bloodscreen = GameObject.Find ("BloodScreen").gameObject.GetComponent<BloodScreen>();
		charaterstatus.isHealing = false;
	}

	public void BulletHit(float Damage)
	{
		if (ybotcanHit && !ac.charaterStatus.isDoAction && this.gameObject.tag == "MainCharater" && !ac.charaterStatus.isDeath && !tasksmanager.GamePaused) {

			hp.fillAmount -= Damage;

			if (hp.fillAmount > 0.8f) {
				bloodscreen.GiveBloodScreen (0.3f);
				bloodscreen.FadeSpeed = 0.3f;
			} else if (hp.fillAmount > 0.5f && hp.fillAmount <= 0.8f) {
				bloodscreen.GiveBloodScreen (0.6f);
				bloodscreen.FadeSpeed = 0.2f;
			} else if (hp.fillAmount > 0.2f && hp.fillAmount <= 0.5f) {
				bloodscreen.GiveBloodScreen (0.8f);
				bloodscreen.FadeSpeed = 0.1f;
			} else if (hp.fillAmount > 0.01f && hp.fillAmount <= 0.2f) {
				bloodscreen.GiveBloodScreen (1f);
				bloodscreen.FadeSpeed = 0;
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		
		if (other.gameObject.tag == opponent && ybotcanHit && !ac.charaterStatus.isDoAction && this.gameObject.tag == "MainCharater" && !ac.charaterStatus.isDeath && !tasksmanager.GamePaused) {
			if (!movecontrol.charaterstatus.isAiming && ac.charaterinventory.Weaponcount != 3) {
				movecontrol.CoverWeapon ();
			}

			if (ac.charaterinventory.Weaponcount != 3) {
				anim.SetTrigger ("Hit");
			}
			ybotcanHit = false;
			lostHP = true;
			Invoke ("stopLostHP", 1f);
			Invoke ("canHit", 3f);

			if (hp.fillAmount > 0.8f) {
				bloodscreen.GiveBloodScreen (0.3f);
				bloodscreen.FadeSpeed = 0.3f;
			}
			else if (hp.fillAmount > 0.5f && hp.fillAmount <= 0.8f) {
				bloodscreen.GiveBloodScreen (0.6f);
				bloodscreen.FadeSpeed = 0.2f;
			}
			else if (hp.fillAmount > 0.2f && hp.fillAmount <= 0.5f) {
				bloodscreen.GiveBloodScreen (0.8f);
				bloodscreen.FadeSpeed = 0.1f;
			}
			else if (hp.fillAmount > 0.01f && hp.fillAmount <= 0.2f) {
				bloodscreen.GiveBloodScreen (1f);
				bloodscreen.FadeSpeed = 0;
			}
			//hp.fillAmount -= 0.1f;

			//hp.fillAmount -= Mathf.Lerp(hp.fillAmount,0f,0.1f*Time.deltaTime);


		}
		/*
		if (other.gameObject.tag == opponent && HitBack) {
			ac.Standstill ();
			Instantiate (hitbackFX, transform.position, transform.rotation);
			addAP = true;
			Invoke ("stopadd", 1f);
		}
*/
		if (other.gameObject.tag == opponent2 && ybotcanHit) {
			anim.speed = 0f;
			OnMete = true;
			Ro.SetActive (true);
		}





	}

	void canHit(){
		ybotcanHit = true;
	}


	void stopLostHP(){
		lostHP = false;

	}

	void stopadd(){
		addAP = false;
	}

	public void isLostAP(){
		lostAP = true;
		Invoke ("stopLostAP", 1f);
	}

	void stopLostAP(){
		lostAP = false;

	}

	// Update is called once per frame
	void Update () {
		if (lostHP == true) {
			//fillaa = hp.fillAmount - 0.1f;
			//hp.fillAmount -= Mathf.Lerp(hp.fillAmount,fillaa,0.1f*Time.deltaTime);
			hp.fillAmount -= 1.0f/10.0f*Time.deltaTime * Multiple;

			ap.fillAmount += 2.0f/10.0f*Time.deltaTime;

		}

		if (hp.fillAmount == 0 && charaterstatus.isDeath == false && !charaterstatus.isDoAction) {
			Dead ();
		}

		if (lostAP == true) {
			ap.fillAmount -= 5.0f/10.0f*Time.deltaTime;
		}

		if (hp.fillAmount < (HPTemp + addAmount) && hp.fillAmount < 1) {
			bloodscreen.FadeSpeed = 0.3f;
			hp.fillAmount += Time.deltaTime / 10f;
		} else {
			addAmount = 0;
		}


		if (Input.GetKeyDown (KeyCode.F11)) {
			Respawn ();
		}
		/*
		if (Input.GetKeyDown (KeyCode.Alpha8)) {
			Dead();
		}
        */

		if (OnMete) {
			if (Input.GetKeyDown ("f")) {
				ac.AddEMP ();
				anim.speed = 1f;
				OnMete = false;
				Roo.SetTrigger ("go");
				Invoke ("OffRock", 1.5f);
			}
		}
	}

	public void Dead()
	{
		bloodscreen.GiveBloodScreen (1);
		bloodscreen.FadeSpeed = 0;
		charaterstatus.isDeath = true;
		charaterstatus.isAiming = false;
		charaterstatus.isDoAction = false;
		lostHP = false;
		anim.SetBool ("IsDeath", true);
		movecontrol.CoverWeapon ();
		Multiple = 1;
		GameObject.Find ("BlackScreen").GetComponent<ScreenFadeToBlack> ().Invoke ("Black", 4f);
		Invoke ("Respawn", 8f);
	}

	public void Respawn()
	{
		hp.fillAmount = 1;
		charaterstatus.isDeath = false;
		anim.SetBool ("IsDeath", false);
		movecontrol.RecoverWeapon ();
		charaterinventory.CameraDepth.Find("UICamera").gameObject.SetActive(true);
		bloodscreen.GiveBloodScreen (0.01f);
		bloodscreen.FadeSpeed = 1f;
		GameObject.Find ("BlackScreen").GetComponent<ScreenFadeToBlack> ().ZeroSlow ();
		if (SpawnPoint != null) {
			this.transform.position = SpawnPoint.position;
		}
	}
	public void Starthealing(string i)
	{
		movecontrol.CoverWeapon ();
		movecontrol.charaterstatus.isDoAction = true;
		anim.SetTrigger("Heal");
	    HPTemp = hp.fillAmount;
		addAmount = float.Parse(i);
		charaterstatus.isHealing = true;
	}

	public void Stophealing()
	{
		HPTemp = 0;
		movecontrol.charaterstatus.isDoAction = false;
		charaterstatus.isHealing = false;
		movecontrol.RecoverWeapon ();
	}

	void OffRock(){
		Ro.SetActive (false);
	}

	

}
