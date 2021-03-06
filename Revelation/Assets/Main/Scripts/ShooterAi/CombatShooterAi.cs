using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatShooterAi : MonoBehaviour {


	public Transform Target;
	public Transform HitExatlyPos;
	public CharaterStatus charaterstatus;

	public float AimSpeed;
	public float RotSpeed;
	public float RotAngle;
	public float RotDistance;
	public float WalkingSpeed;
	public float RunningSpeed;

	public float DetectedAngle;
	public float DetectedDistance;
	public float DetectedGunShotDistance;
	public float ShootCoolDown;
	public float RateOfFire;
	public GameObject Bullet;
	public Transform shotPoint;
	public Color BulletColor;
	public float gunspread;
	public int MaxAmmo;
	public int Ammo;
	public float damage;

	public LayerMask DetectObstacleMask;


	public bool AbleToShoot;
	public bool IsReloading;
	public bool IsAllied;
	public bool IsChaser;
	public bool IsCroucher;
	public float AimDelay;
	public float ShootDelay;
	public string State;
	public GameObject[] waypoints;
	public GameObject CoverPoint;
	public int currentWP = 0;
	public float StandTime;
	NavMeshAgent agent;

	public Animator anim;

	public Transform AimPivot;

	public Transform LookTarget; //角色面向之IK目標
	public Transform l_Hand; //左手要獲取之IK目標
	public Transform r_Hand; //右手要獲取之IK目標

	public float lk_Weight; //角色面向IK權重值
	public float lh_Weight; //左手IK權重值
	public float rh_Weight; //右手IK權重值

	public bool isAiming; //判斷玩家是否在瞄準
	public bool isAimingDebug; //瞄準除錯時使用

	public Transform r_Hand_Target;
	public GameObject MuzzleFlash;

	public EnemyStat enemystat;

	public GameObject Weapon;

	public GameObject AlliedShooter;
	public GameObject AlliedShooterTarget;
	public GameObject FakeTarget;
	public bool AlliedShooterEnemy;
	public TasksManager tasksmanager;
	public bool MeleeMode;
	public float MeleeDistance;
	public int ShootTime;
	public int ShootTimeCount;
	public int MeleeTime;
	public int MeleeTimeCount;
	public bool AbleToMelee;
	public float MeleeRotSpeed;
	public ShooterController shootcontroller;
	public AlarmSpawner alarmspawner;
	public bool ControlerAdded;
	// Use this for initialization
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

	void Start () {
		ShootDelay = 3f;
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		charaterstatus = GameObject.FindGameObjectWithTag ("MainCharater").transform.GetComponent<MoveControl>().charaterstatus;
		anim = this.GetComponent<Animator> ();
		agent = this.GetComponent<NavMeshAgent> ();
		Ammo = MaxAmmo;
		MeleeTimeCount = MeleeTime + 1;
		//ShootTimeCount = ShootTime;
		AbleToShoot = true;
		enemystat = this.GetComponent<EnemyStat> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Aim ();
		if (enemystat.health <= 0) {
			agent.enabled = false;
			return;
		}

		state ();

		if (this.GetComponent<Rigidbody> ()) {
			this.GetComponent<Rigidbody> ().velocity = new Vector3 (0, -1, 0);
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
	}
		
	void OnAnimatorIK()
	{
		
		if (enemystat.health > 0) 
		{
			AimPivot.LookAt (LookTarget);
		}
		anim.SetLookAtWeight (lk_Weight, 0.3f, 0.3f); //設定角色看向目標權重，值為lk_Weight
		anim.SetLookAtPosition (LookTarget.position); //設定角色看向之目標，即為LookTarget
		anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, lh_Weight); //設定角色左手IK位置權重，值為lh_Weigh
		anim.SetIKRotationWeight (AvatarIKGoal.LeftHand, lh_Weight); //設定角色左手IK旋轉權重，值為lh_Weigh
		anim.SetIKPosition (AvatarIKGoal.LeftHand, l_Hand.position); //設定角色左手抓取l_Hand之位置
		anim.SetIKRotation (AvatarIKGoal.LeftHand, l_Hand.rotation); //設定角色左手抓取l_Hand之旋轉軸
		anim.SetIKPositionWeight (AvatarIKGoal.RightHand, rh_Weight); //設定角色右手IK位置權重，值為rh_Weigh
		anim.SetIKRotationWeight (AvatarIKGoal.RightHand, rh_Weight); //設定角色左手IK旋轉權重，值為rh_Weigh
		anim.SetIKPosition (AvatarIKGoal.RightHand, r_Hand.position); //設定角色左手抓取r_Hand之位置
		anim.SetIKRotation (AvatarIKGoal.RightHand, r_Hand.rotation);
	}

	void state()
	{
		float accuracyWP = 2.0f;

		Vector3 direction = Target.position - anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position;
		float angle = Vector3.Angle (direction, this.transform.forward);
		direction.y = 0;

		//RightHandIK (true);

		if (State == "patrol" && waypoints.Length > 0) {
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
		} 
		else if(State == "patrol" && waypoints.Length == 0)
		{
			ShootDelay = 3f;
			StandIdle ();
		}

		if ((Vector3.Distance (Target.position, anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position) < DetectedDistance && !Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Target.position, DetectObstacleMask) && (angle < DetectedAngle || charaterstatus.isFireing)) || (State == "Detected")) {
			State = "Detected";

			if (ShootTimeCount >= ShootTime && MeleeTimeCount <= MeleeTime) {
				MeleeMode = true;
			}
			else if(ShootTimeCount <= ShootTime && MeleeTimeCount >= MeleeTime)
			{
				MeleeMode = false;
			}

			if (MeleeMode) {
				Melee (true);
				Aim (false);
			} else {
				if (Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Target.position, DetectObstacleMask)) {
					Run ();
					Aim (false);
					agent.SetDestination (Target.position);
					AimDelay = 0.5f;
				} else {
					if (AimDelay > 0) {
						AimDelay -= Time.deltaTime * 1f;
					}
					if (!IsCroucher) {
						StandIdle ();
					} else {
						CrouchIdle ();
					}
					
					if (AimDelay <= 0) {
						Aim (true);
					}
				}
			}
		}
	}
		

	void TakenDamageDetected()
	{

	}

	void Shoot()
	{
		if (AbleToShoot) {
			Ammo--;
			if(ShootTimeCount > ShootTime)
			{
				ShootTimeCount = ShootTime;
				MeleeTimeCount = 0;
			}
			ShootTimeCount++;
			anim.CrossFadeInFixedTime("Fire", 0.01f);
			l_Hand.gameObject.GetComponent<Animator>().CrossFadeInFixedTime("Shot", 0.01f);
			shotPoint.Rotate (shotPoint.rotation.x + Random.Range (-gunspread, gunspread), shotPoint.rotation.y + Random.Range (-gunspread, gunspread), shotPoint.rotation.z + Random.Range (-gunspread, gunspread));
			if (!IsAllied) {
				GameObject.Find ("BulletPool").GetComponent<ShooterBulletPool> ().SpawnBullet (shotPoint.position, shotPoint.rotation, damage, BulletColor, 30, 2f);
			} else {
				GameObject.Find ("BulletPool").GetComponent<AlliedShooterBulletPool> ().SpawnBullet (shotPoint.position, shotPoint.rotation, damage, BulletColor);
			}
			AbleToShoot = false;
			MuzzleFlash.SetActive (false);
			MuzzleFlash.SetActive (true);
			Invoke ("EnableShoot", RateOfFire);
			if (alarmspawner) {
				alarmspawner.SpawnShooter ();
			}
			for (int i = 0; i < shootcontroller.Shooter.Count; i++) {

				if (shootcontroller.Shooter [i] == null) {
					shootcontroller.Shooter.Remove (shootcontroller.Shooter [i]);
				}

				if (shootcontroller.Shooter [i].GetComponent<EnemyStat> ().health > 0) {
					if (shootcontroller.Shooter [i].GetComponent<ShooterAi> ()) {
						if (shootcontroller.Shooter [i].GetComponent<ShooterAi> ().State != "Detected") {
							if (Vector3.Distance (shootcontroller.Shooter [i].transform.position, anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position) < shootcontroller.Shooter [i].GetComponent<ShooterAi> ().DetectedGunShotDistance) {
								shootcontroller.Shooter [i].GetComponent<ShooterAi> ().State = "Detected";
							}
						}
					}

					if (shootcontroller.Shooter [i].GetComponent<CombatShooterAi> ()) {
						if (shootcontroller.Shooter [i].GetComponent<CombatShooterAi> ().State != "Detected") {
							if (Vector3.Distance (shootcontroller.Shooter [i].transform.position, anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position) < shootcontroller.Shooter [i].GetComponent<CombatShooterAi> ().DetectedGunShotDistance) {
								shootcontroller.Shooter [i].GetComponent<CombatShooterAi> ().State = "Detected";
							}
						}
					}
					if (shootcontroller.Shooter [i].GetComponent<CamShooter> ()) {
						if (shootcontroller.Shooter [i].GetComponent<CamShooter> ().State != "Detected") {
							if (Vector3.Distance (shootcontroller.Shooter [i].transform.position, anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position) < shootcontroller.Shooter [i].GetComponent<CamShooter> ().DetectedGunShotDistance) {
								shootcontroller.Shooter [i].GetComponent<CamShooter> ().State = "Detected";
							}
						}
					}
				} else {
					shootcontroller.Shooter.Remove (shootcontroller.Shooter [i]);
				}
			}
		}
	}

	void EnableShoot()
	{
		AbleToShoot = true;
	}

	void Melee(bool On)
	{
		if (enemystat.health <= 0) {
			return;
		}

		if (On) {
			rh_Weight += Time.deltaTime * 2;
		} else {
			rh_Weight -= Time.deltaTime * 10;
		}

		/*
		if (r_Hand_Target != null) //如果r_Hand_Target有被獲取為任意物件
		{
			r_Hand.position = r_Hand_Target.position; //把l_Hand的位置設定為r_Hand_Target之位置
			r_Hand.rotation = r_Hand_Target.rotation; //把l_Hand的旋轉值設定為r_Hand_Target之旋轉值
		}
		*/
		rh_Weight = Mathf.Clamp (rh_Weight, 0, 1);

		Vector3 direction = Target.position - anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position;
		float angle = Vector3.Angle (direction, this.transform.forward);
		direction.y = 0;

		if (angle > RotAngle) {
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), MeleeRotSpeed * Time.deltaTime);
		}

		if (rh_Weight > 0.6f && (Vector3.Distance (Target.position, anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position) < MeleeDistance) && !Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Target.position, DetectObstacleMask)) {
			StandIdle ();
			if (AbleToMelee && !tasksmanager.GamePaused) {
				if(MeleeTimeCount > MeleeTime)
				{
					MeleeTimeCount = MeleeTime;
					ShootTimeCount = 0;
				}
				MeleeTimeCount++;
				r_Hand.gameObject.GetComponent<Animator> ().CrossFadeInFixedTime ("Melee", 0.01f);
				AbleToMelee = false;
				Invoke ("EnableMelee", 1f);
			}
		} else if((Vector3.Distance (Target.position, anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position) > MeleeDistance) || Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Target.position, DetectObstacleMask)){
			Run ();
			agent.SetDestination (Target.position);
		}
		lk_Weight += Time.deltaTime * 10;
		lk_Weight = Mathf.Clamp (lk_Weight, 0, 10);
	}

	void EnableMelee()
	{
		AbleToMelee = true;
	}

	void Aim(bool On)
	{

		if (On || isAimingDebug) {
			anim.SetBool ("IsAiming", true);
			lh_Weight += Time.deltaTime * 2; //以Time.deltaTime(秒)為單位，增加rh_Weight(右手權重值)
			lk_Weight += Time.deltaTime * 10; //解釋同上，增加的是角色朝向IK值。
		} else {
			anim.SetBool ("IsAiming", false);
			lh_Weight -= Time.deltaTime * 2; //以Time.deltaTime(秒)為單位，減少rh_Weight(右手權重值)
			if (Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Target.position, DetectObstacleMask) && !MeleeMode) {
					lk_Weight -= Time.deltaTime * 10; //解釋同上，減少的是角色朝向IK值。
			}
		}



		lh_Weight = Mathf.Clamp (lh_Weight, 0, 1); //以此為範例，設定rh_Weight值，最小為0，最大為1
		lk_Weight = Mathf.Clamp (lk_Weight, 0, 10);


		if (enemystat.health <= 0) {
			return;
		}
        

		shotPoint.LookAt (Target);

		float step = AimSpeed * Time.deltaTime;
		LookTarget.transform.position = Vector3.MoveTowards(LookTarget.transform.position, Target.transform.position, step);

		if (!MeleeMode) {
			Vector3 direction = Target.position - anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position;
			Vector3 directionShotPoint = Target.position - AimPivot.transform.position;
			float angle = Vector3.Angle (direction, this.transform.forward);
			float angleShotPoint = Vector3.Angle (directionShotPoint, AimPivot.transform.forward);
			direction.y = 0;

			if (angle > RotAngle) {
				this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), RotSpeed * Time.deltaTime);
			}

			if (angleShotPoint < 20 && !Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Target.position, DetectObstacleMask) && lh_Weight >= 0.5f && lk_Weight >= 5 && !tasksmanager.GamePaused) {
			
				if (ShootDelay <= 0) {
					Shoot ();
				} else {
					ShootDelay -= Time.deltaTime;
				}
			}
		}
	}

	void Reload()
	{
		IsReloading = true;
		AbleToShoot = false;
		anim.SetBool ("IsReloading", true);
		if (anim.GetBool ("IsAiming")) {
			anim.SetBool ("IsAiming", false);
			Aim (false);
		}

	}

	void ReloadEnd()
	{
		Invoke ("EnableShoot", 1f);
		Ammo = MaxAmmo;
		IsReloading = false;
		anim.SetBool ("IsReloading", false);
	}

	void Walk()
	{
		anim.SetBool ("IsStandIdleing", false);
		anim.SetBool ("IsCrouchIdleing", false);
		anim.SetBool ("IsShooting", false);
		anim.SetBool ("IsReloading", false);
		anim.SetBool ("IsAiming", false);
		anim.SetBool ("IsRunning", false);
		anim.SetBool ("IsWalking", true);
		agent.speed = WalkingSpeed;
	}

	void Run()
	{
		anim.SetBool ("IsStandIdleing", false);
		anim.SetBool ("IsCrouchIdleing", false);
		anim.SetBool ("IsShooting", false);
		anim.SetBool ("IsReloading", false);
		anim.SetBool ("IsAiming", false);
		anim.SetBool ("IsRunning", true);
		anim.SetBool ("IsWalking", false);
		agent.speed = RunningSpeed;
	}

	void StandIdle()
	{
		anim.SetBool ("IsStandIdleing", true);
		anim.SetBool ("IsCrouchIdleing", false);
		anim.SetBool ("IsShooting", false);
		anim.SetBool ("IsReloading", false);
		anim.SetBool ("IsAiming", false);
		anim.SetBool ("IsRunning", false);
		anim.SetBool ("IsWalking", false);
		agent.SetDestination(transform.position);
		agent.speed = 0;
	}

	void CrouchIdle()
	{
		anim.SetBool ("IsStandIdleing", false);
		anim.SetBool ("IsCrouchIdleing", true);
		anim.SetBool ("IsShooting", false);
		anim.SetBool ("IsReloading", false);
		anim.SetBool ("IsAiming", false);
		anim.SetBool ("IsRunning", false);
		anim.SetBool ("IsWalking", false);
		agent.SetDestination(transform.position);
	}

}
