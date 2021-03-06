using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStat : MonoBehaviour {
	
	public Rigidbody[] rigid;
	public Collider[] collider;
	public Collider ColliderBodySelf;
	public Rigidbody RigidbodyBodySelf;
	public float health;
	public Animator anim;

	public Material de;
	public Renderer re;
	public float dissolveOverTime=-1;
	public bool canDissolve = false;

	public float takeaway;
	public float burntime;
	public ParticleSystem FireEffect;

	public bool isDead;
	public bool isStunned;
	public bool CantDmg;

	AudioSource AS;
	public float Mass = 0.1f;
	public TasksManager tasksmanager;
	public Transform Cam;

	void Start()
	{
		isDead = false;
		AS = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		RigidbodyBodySelf = GetComponent<Rigidbody> ();
		ColliderBodySelf = GetComponent<CapsuleCollider> ();
		ColliderBodySelf.isTrigger = false;
		if (rigid.Length > 0) {
			for (int i = 0; i < rigid.Length; i++) {
				rigid [i].isKinematic = true;
				rigid [i].mass = Mass;
				collider [i] = rigid [i].GetComponent<Collider> ();
			}
		}

		if (collider.Length > 0) {
			for (int i = 0; i < collider.Length; i++) {
				collider [i].isTrigger = true;
			}
		}

		if (GetComponent<ShooterAi> ()) {
			if(!GetComponent<ShooterAi> ().IsAllied)
			{
				
				collider [1].GetComponent<CapsuleCollider> ().radius = 0.09f;
				collider [2].GetComponent<CapsuleCollider> ().radius = 0.09f;
				collider [3].GetComponent<CapsuleCollider> ().radius = 0.09f;
				collider [4].GetComponent<CapsuleCollider> ().radius = 0.09f;
				collider [6].GetComponent<CapsuleCollider> ().radius = 0.06f;
				collider [7].GetComponent<CapsuleCollider> ().radius = 0.06f;
				collider [9].GetComponent<CapsuleCollider> ().radius = 0.06f;
				collider [10].GetComponent<CapsuleCollider> ().radius = 0.06f;

			}
		}
		/*
		foreach (Rigidbody rb in rigid) {
			rb.isKinematic = true;
			rb.mass = 0.1f;

		}
		foreach (Collider rb in collider) {
			rb.isTrigger = true;
		}
		*/
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		Cam = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}

	void Update () {

		if (canDissolve) {
			dissolveOverTime += Time.deltaTime * 0.75f;
			de.SetFloat ("_DissolveAmount", dissolveOverTime);

		}

		if (burntime > 0) {
			burntime -= Time.deltaTime;
			health -= takeaway * Time.deltaTime * 5;

			if(health <= 0)
				Dead();
			//burntime = 0;
		}
		else if (burntime < 0)
		{
			FireEffect.Stop ();
			burntime = 0;
		}
	}

	public void TakeAwayHealth(float TakeAway)
	{
		if (CantDmg || tasksmanager.GamePaused) {
			return;
		}
		health -= TakeAway;
		if (this.GetComponent<AI2> ()) {
			this.GetComponent<AI2> ().isDamaged = true;
		}
			if(health <= 0)
				Dead();
	}

	public void TakeAwayHealthBurn(float TakeAway, float BurnTime)
	{
		if (CantDmg || tasksmanager.GamePaused) {
			return;
		}
		takeaway = TakeAway;
		health -= takeaway;

		burntime = BurnTime;

		FireEffect.Play ();
		if (this.GetComponent<AI2> ()) {
			this.GetComponent<AI2> ().isDamaged = true;
		}
		if(health <= 0)
			Dead();
	}

	public void Dead()
	{


		if (isDead) {
			return;
		}
		/*
		if (this.GetComponent<AI2> ().IsBoss) {
			anim.SetTrigger ("IsDeath");
		}
		*/

		if (GetComponent<CamShooter> ()) {
			GetComponent<CamShooter> ().Explode ();
				Cam.GetComponent<CameraShake> ().shakeMagnitude = 0.03f;
				Cam.GetComponent<CameraShake> ().shakeTime = 0.2f;
				Cam.GetComponent<CameraShake> ().ShakeIt ();
		}



		AS.Stop ();
		Destroy (RigidbodyBodySelf);
		Destroy (ColliderBodySelf);
        this.gameObject.GetComponent<NavMeshAgent>().speed = 0;
		this.gameObject.GetComponent<NavMeshAgent> ().enabled = false;

		if(this.GetComponent<SpawnItemWhenDead>())
		{
			this.GetComponent<SpawnItemWhenDead> ().SpawnItem ();
		}
		if (rigid.Length > 0) {
			for (int i = 0; i < rigid.Length; i++) {
				rigid [i].isKinematic = false;
			}
		}

		if (collider.Length > 0) {
			for (int i = 0; i < collider.Length; i++) {
				collider [i].isTrigger = false;
			}
		}
		/*
		foreach (Rigidbody rb in rigid) {
			rb.isKinematic = false;
		}
		foreach (Collider rb in collider) {
			rb.isTrigger = false;
		}
		*/

		anim.enabled = false;


		if (this.GetComponent<Sentence> ()) {
			this.GetComponent<Sentence> ().StartSentence();
		}

		if (this.GetComponent<SceneBlocker> ()) {
			this.GetComponent<SceneBlocker> ().UnBlockScene ();
		}
		if (this.GetComponent<Tasks> ()) {
			this.GetComponent<Tasks> ().TriggerEvent ();
		}

		//Invoke("Dissolve", 3f);
		isDead = true;
		Destroy (this.gameObject, 15);
	}

	public void Dissolve(){
		re.GetComponent<Renderer> ().material = de;
		FireEffect.Stop ();
		canDissolve = true;

	}
		
	public void ExitStunned()
	{
		isStunned = false;
	}

	public void ITweenVector3Move(GameObject gameobject,float AmoutX,float AmoutY,float AmoutZ,float Duration)
	{
		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("amount", new Vector3(AmoutX, AmoutY, AmoutZ));
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);
		iTween.MoveAdd (gameObject, moveSetting);
	}
}
