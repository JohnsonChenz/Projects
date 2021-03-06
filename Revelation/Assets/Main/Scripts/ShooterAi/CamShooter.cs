using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CamShooter : MonoBehaviour {

	public bool isFlyer;
	public Transform Target;
	public Transform shotPoint;
	public Transform GunRoot;
	public Transform GunRotate;
	public float GunRotateSpeed;
	public Transform FakeTarget;
	public CharaterStatus charaterstatus;
	public float AimSpeed;
	public float WalkingSpeed;
	public float RunningSpeed;
	public float RotSpeed;
	public float RotAngle;
	public Animator anim;
	public NavMeshAgent agent;
	public EnemyStat enemystat;
	public float ShootDelay;
	public float DetectedAngle;
	public float DetectedDistance;
	public float DetectedGunShotDistance;
	public GameObject[] waypoints;
	public int currentWP = 0;
	public Transform LookTarget; //角色面向之IK目標
	public string State;
	public float StandTime;
	public LayerMask DetectObstacleMask;
	public float CoolDownTime;
	public float cooldowntime;
	public float ShootTime;
	public float shoottime;

	public GameObject MuzzleFlash;
	public bool AbleToShoot;
	public float gunspread;
	public float RateOfFire;
	public float damage;
	public Color BulletColor;
	public GameObject[] explodeObjs;
	public GameObject ExplodeEffect;
	public float Exploderadius;
	public float Explodeforce;

	public TasksManager tasksmanager;

	public bool AlliedShooterEnemy;
	public GameObject AlliedShooter;
	public bool IsChaser;
	public GameObject Laser;

	public ShooterController shootcontroller;
	public AlarmSpawner alarmspawner;
	public bool ControlerAdded;
	void OnEnable()
	{
		if (!ControlerAdded) {
			shootcontroller = GameObject.Find ("ShooterController").GetComponent<ShooterController> ();
			shootcontroller.Shooter.Add (this.gameObject);
			if (AlliedShooterEnemy) {
				AlliedShooter.GetComponent<ShooterAi> ().Enemy.Add (this.gameObject);
			}
			ControlerAdded = true;
		}
	}
	// Use this for initialization
	void Start () {

		ShootDelay = 3f;
		charaterstatus = GameObject.FindGameObjectWithTag ("MainCharater").transform.GetComponent<MoveControl>().charaterstatus;
		anim = this.GetComponent<Animator> ();
		agent = this.GetComponent<NavMeshAgent> ();
		enemystat = this.GetComponent<EnemyStat> ();
		//cooldowntime = CoolDownTime;
		shoottime = ShootTime;
		AbleToShoot = true;
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
	}

	void Onenable()
	{
		//AbleToShoot = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (enemystat.health > 0) {
			state ();
		} else {
			if (Laser) {
				Laser.SetActive (false);
			}
			return;
		}

		if (tasksmanager.GamePaused) {
			anim.speed = 0;
			agent.speed = 0;
			this.GetComponent<AudioSource> ().Pause ();
		} else {
			this.GetComponent<AudioSource> ().UnPause ();
			if (anim.speed == 0) {
				anim.speed = 1;
			}
		}
			
		if (this.GetComponent<Rigidbody> ()) {
			this.GetComponent<Rigidbody> ().velocity = new Vector3 (0, -1, 0);
		}
	}

	void state()
	{
		float accuracyWP = 5.0f;

		Vector3 direction = Target.position - this.transform.position;
		float angle = Vector3.Angle (direction, this.transform.forward);
		direction.y = 0;


		if (State == "patrol" && waypoints.Length > 0) {
			ResetLookTar ();
			ShootDelay = 3f;
			if (Vector3.Distance (waypoints [currentWP].transform.position, transform.position) < accuracyWP) {
				currentWP = Random.Range (0, waypoints.Length);
				StandTime = 3;
				StandIdle ();
			} else {
				if (StandTime > 0) {
					StandTime -= Time.deltaTime;
					StandIdle ();
				} else {
					Walk ();
					agent.speed = WalkingSpeed;
					agent.SetDestination (waypoints [currentWP].transform.position);
				}
			}
			if (isFlyer) {
				Laser.SetActive (false);
			} else {
				Laser.SetActive (true);
			}
		} 
		else if(State == "patrol" && waypoints.Length == 0)
		{
			ResetLookTar ();
			ShootDelay = 3f;
			StandIdle();
			if (isFlyer) {
				Laser.SetActive (false);
			} else {
				Laser.SetActive (true);
			}
		}

		if ((Vector3.Distance (Target.position, this.transform.position) < DetectedDistance && !Physics.Linecast (transform.position, Target.position, DetectObstacleMask) && (angle < DetectedAngle || charaterstatus.isFireing)) || (State == "Detected")) {
			State = "Detected";
			if (IsChaser) {
				if (Physics.Linecast (transform.position, Target.position, DetectObstacleMask)) {
					Run ();
					ShootDelay = 3f;
					agent.SetDestination (Target.position);
					ResetLookTar ();
				} else {
					StandIdle ();
					if (!tasksmanager.GamePaused) {
						Attack ();
					}
				}
			} else {
				if (Physics.Linecast (transform.position, Target.position, DetectObstacleMask)) {
					ShootDelay = 3f;
					ResetLookTar ();
					StandIdle ();
				} else {
					StandIdle ();
					if (!tasksmanager.GamePaused) {
						Attack ();
					}
				}
			}
			Laser.SetActive (true);
		}
	}

	void ResetLookTar()
	{
		GunRoot.LookAt (LookTarget);
		float step = AimSpeed * Time.deltaTime;
		LookTarget.transform.position = Vector3.MoveTowards(LookTarget.transform.position, FakeTarget.transform.position, step);
	}

	void LookTar()
	{
		GunRoot.LookAt (LookTarget);
		float step = AimSpeed * Time.deltaTime;
		LookTarget.transform.position = Vector3.MoveTowards(LookTarget.transform.position, Target.transform.position, step);
	}

	void Attack()
	{
		LookTar ();
		shotPoint.LookAt (Target);
		Vector3 direction = Target.position - this.transform.position;
		Vector3 directionShotPoint = Target.position - shotPoint.transform.position;
		float angle = Vector3.Angle (direction, this.transform.forward);
		float angleShotPoint = Vector3.Angle (directionShotPoint, shotPoint.transform.forward);
		direction.y = 0;

		if (angle > RotAngle && isFlyer) {
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), RotSpeed * Time.deltaTime);
		}

		if (ShootDelay > 0) {
			ShootDelay -= Time.deltaTime;
			GunRotate.Rotate (0, 0, GunRotateSpeed * Time.deltaTime);
		} else if(angleShotPoint < 20){
			
			if (cooldowntime > 0) {
				cooldowntime -= Time.deltaTime;
			} else if(cooldowntime < 0){
				cooldowntime = 0;
				shoottime = ShootTime;
			}


			if (shoottime > 0) {
				shoottime -= Time.deltaTime;
				Shot ();
				GunRotate.Rotate (0, 0, GunRotateSpeed * Time.deltaTime * 2);
			} else if(shoottime < 0){
				shoottime = 0;
				cooldowntime = CoolDownTime;
				GunRotate.Rotate (0, 0, GunRotateSpeed * Time.deltaTime * 0.5f);
			}
		}
	}

	void Shot()
	{
		if (AbleToShoot) {
			anim.CrossFadeInFixedTime("Fire", 0.01f);
			shotPoint.Rotate (shotPoint.rotation.x + Random.Range (-gunspread, gunspread), shotPoint.rotation.y + Random.Range (-gunspread, gunspread), shotPoint.rotation.z + Random.Range (-gunspread, gunspread));
			GameObject.Find ("BulletPool").GetComponent<ShooterBulletPool> ().SpawnBullet (shotPoint.position, shotPoint.rotation, damage, BulletColor, 30, 1f);
			AbleToShoot = false;
			MuzzleFlash.SetActive (false);
			MuzzleFlash.SetActive (true);
			Invoke ("EnableShoot", RateOfFire);
			for (int i = 0; i < shootcontroller.Shooter.Count; i++) {

				if (shootcontroller.Shooter [i] == null) {
					shootcontroller.Shooter.Remove (shootcontroller.Shooter [i]);
				}

				if (shootcontroller.Shooter [i].GetComponent<EnemyStat> ().health > 0) {
					if (shootcontroller.Shooter [i].GetComponent<ShooterAi> ()) {
						if (shootcontroller.Shooter [i].GetComponent<ShooterAi> ().State != "Detected") {
							if (Vector3.Distance (shootcontroller.Shooter [i].transform.position, this.transform.position) < shootcontroller.Shooter [i].GetComponent<ShooterAi> ().DetectedGunShotDistance) {
								shootcontroller.Shooter [i].GetComponent<ShooterAi> ().State = "Detected";
							}
						}
					}

					if (shootcontroller.Shooter [i].GetComponent<CombatShooterAi> ()) {
						if (shootcontroller.Shooter [i].GetComponent<CombatShooterAi> ().State != "Detected") {
							if (Vector3.Distance (shootcontroller.Shooter [i].transform.position, this.transform.position) < shootcontroller.Shooter [i].GetComponent<CombatShooterAi> ().DetectedGunShotDistance) {
								shootcontroller.Shooter [i].GetComponent<CombatShooterAi> ().State = "Detected";
							}
						}
					}
					if (shootcontroller.Shooter [i].GetComponent<CamShooter> ()) {
						if (shootcontroller.Shooter [i].GetComponent<CamShooter> ().State != "Detected") {
							if (Vector3.Distance (shootcontroller.Shooter [i].transform.position, this.transform.position) < shootcontroller.Shooter [i].GetComponent<CamShooter> ().DetectedGunShotDistance) {
								shootcontroller.Shooter [i].GetComponent<CamShooter> ().State = "Detected";
							}
						}
					}
				} else {
					shootcontroller.Shooter.Remove (shootcontroller.Shooter [i]);
				}
			}
			if (alarmspawner) {
				alarmspawner.SpawnShooter ();
			}
		}
	}

	void EnableShoot()
	{
		AbleToShoot = true;
	}

	void Walk()
	{
		agent.speed = WalkingSpeed;
	}

	void Run()
	{
		agent.speed = RunningSpeed;
	}

	void StandIdle()
	{
		if (isFlyer) {
			agent.SetDestination (transform.position);
		}
	}

	public void Explode()
	{
		Instantiate (ExplodeEffect, transform.position, transform.rotation);

		for (int i = 0; i < explodeObjs.Length; i++) {
			if (!explodeObjs [i].GetComponent<Rigidbody> ()) {
				explodeObjs [i].AddComponent<Rigidbody> ();
			}
			if (!explodeObjs [i].GetComponent<BoxCollider> ()) {
				explodeObjs [i].AddComponent<BoxCollider> ();
			} else {
				explodeObjs [i].GetComponent<BoxCollider> ().enabled = true;
			}
			explodeObjs [i].transform.SetParent (null);
			Rigidbody rb = explodeObjs [i].GetComponent<Rigidbody> ();

			rb.AddExplosionForce (Explodeforce, transform.position, Exploderadius);
			rb.AddForce (transform.up * Explodeforce);
			rb.isKinematic = false;
			Destroy (explodeObjs [i], 15f);
		}
		/*
		foreach(Collider nearbyObject in colliders)
		{
			
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody> ();

			if (rb != null) {
				//rb.AddForce(forces, ForceMode.Impulse);
				rb.AddExplosionForce (Explodeforce * 5f, transform.position, Exploderadius);
				rb.AddForce (transform.up * Explodeforce);
			}
		}
		*/
	}
}
