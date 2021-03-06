using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;


public class Detectedhit : MonoBehaviour {

	public Slider healthbar;

	public ActorEvent actorevent;

	Animator anim;

	public bool hitback=false;
	public bool canhitback=false;

	public string opponent;

	public EnemyStat enemystat;

	public PlayableDirector pd;



	void OnTriggerEnter (Collider other) {


		if (healthbar.value <= 0)
			return;

		
		if (healthbar.value > 0 && other.gameObject.tag == opponent && actorevent.WeaponControll == true) {
			//enemystat.TakeAwayHealth (30);
			actorevent.Standstill ();

		}


		if (other.gameObject.tag == opponent && hitback == true && actorevent.WeaponControll) {
			anim.SetTrigger ("doHit");
			}

		if (other.gameObject.tag == opponent && canhitback == true && actorevent.WeaponControll) {
			pd.Play ();
		}


	}
	public void hit()
	{
		healthbar.value -= 3;

		if (healthbar.value <= 0) {
			anim.SetBool ("IsDeath", true);
		}

	}
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		healthbar.maxValue = enemystat.health;
	}

	void Update()
	{
		healthbar.value = enemystat.health;
	}

	public void HitBackEnable(){
		
		hitback = true;
	}

	public void HitBackDisable(){
		hitback = false;
	}

	public void canHitBackAttack(){
		actorevent.WeaponControll = true;
		canhitback = true;
	}

	public void noHitBackAttack(){
		actorevent.WeaponControll = false;
		canhitback = false;
	}


}
