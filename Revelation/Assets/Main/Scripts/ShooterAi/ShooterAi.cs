using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShooterAi : MonoBehaviour {


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
	public bool RandomPotral;
	public bool Potral_Only_Once;
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

	public Transform l_Hand_Target;
	public GameObject MuzzleFlash;

	public EnemyStat enemystat;

	public GameObject Weapon;

	public List<GameObject> Enemy = new List<GameObject>();
	public GameObject AlliedShooter;
	public GameObject AlliedShooterTarget;
	public GameObject FakeTarget;
	public bool AlliedShooterEnemy;
	public TasksManager tasksmanager;
	public float AttackDistance;

	public bool Stuck;


	public bool WeaponsFree;
	public bool Activated;
	//public AlliedShooterEvent alliedshooterevent;
	public ShooterController shootcontroller;
	public AlarmSpawner alarmspawner;
	public string AlliedShooterOff;
	public Transform Cam;
	public bool ControlerAdded;
	// Use this for initialization
	void OnEnable()
	{
		if (!ControlerAdded) {
			if (!IsAllied) {
				shootcontroller = GameObject.Find ("ShooterController").GetComponent<ShooterController> ();
				shootcontroller.Shooter.Add (this.gameObject);
			}
			if (AlliedShooterEnemy) {
				AlliedShooter.GetComponent<ShooterAi> ().Enemy.Add (this.gameObject);
			}
			ControlerAdded = true;
		}
	}

	void Start () {
		ShootDelay = 3f;
		AbleToShoot = true;
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		charaterstatus = GameObject.FindGameObjectWithTag ("MainCharater").transform.GetComponent<MoveControl>().charaterstatus;
		anim = this.GetComponent<Animator> ();
		agent = this.GetComponent<NavMeshAgent> ();
		Ammo = MaxAmmo;
		enemystat = this.GetComponent<EnemyStat> ();
		Cam = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}

	public Transform Pos;
	// Update is called once per frame
	void Update () {
		//Aim ();

		if (enemystat.health <= 0) {

			if(Weapon.transform.parent != null)
			{
				Weapon.AddComponent<Rigidbody> ();
				//Weapon.GetComponent<BoxCollider> ().isTrigger = false;

				Weapon.transform.SetParent (null);
			}


			agent.enabled = false;
			return;
		}
		if (this.GetComponent<Rigidbody> ()) {
			this.GetComponent<Rigidbody> ().velocity = new Vector3 (0, -1, 0);
		}
		if (!IsAllied) {
			state ();
		} else {
			stateAllied ();
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
		/*
		if (AlliedShooterTarget) {
			Debug.DrawLine (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, AlliedShooterTarget.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Color.green);
		}
		*/

		if (Input.GetKeyDown (KeyCode.K)) {

		}
	}

	void ChooseEnemy()
	{
		
		if (Enemy.Count > 0) {
			for (int i = 0; i < Enemy.Count; i++) {

				if (Enemy [i] == null) {
					Target = FakeTarget.transform;
					AlliedShooterTarget = null;
					Enemy.Remove (Enemy [i]);
				}

				if (Enemy [i].GetComponent<EnemyStat> ().health > 0) {
					if (Enemy [i].activeInHierarchy) {
						if (Enemy [i].GetComponent<CamShooter> ()) {
							if ((Vector3.Distance (Enemy [i].transform.position, transform.position) < AttackDistance) && !Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Enemy [i].GetComponent<CamShooter> ().GunRoot.transform.position, DetectObstacleMask)) {
								AlliedShooterTarget = Enemy [i].gameObject;
								Target = Enemy [i].GetComponent<CamShooter> ().GunRoot.transform;
								break;
							}

							if ((Vector3.Distance (Enemy [i].transform.position, transform.position) > AttackDistance) || Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Enemy [i].transform.position, DetectObstacleMask)) {
								Target = FakeTarget.transform;
								AlliedShooterTarget = null;
							} 
						} else {
							if ((Vector3.Distance (Enemy [i].transform.position, transform.position) < AttackDistance) && !Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Enemy [i].GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, DetectObstacleMask)) {
								AlliedShooterTarget = Enemy [i].gameObject;
								if (AlliedShooterTarget.GetComponent<ShooterAi> ()) {
									Target = AlliedShooterTarget.GetComponent<ShooterAi> ().anim.GetBoneTransform (HumanBodyBones.Chest).transform;
								} 
								if (AlliedShooterTarget.GetComponent<CombatShooterAi> ()) {
									Target = AlliedShooterTarget.GetComponent<CombatShooterAi> ().anim.GetBoneTransform (HumanBodyBones.Chest).transform;
								}
								if (AlliedShooterTarget.GetComponent<AI2> ()) {
									Target = AlliedShooterTarget.GetComponent<AI2> ().anim.GetBoneTransform (HumanBodyBones.Chest).transform;
								}
								break;
							}

							if ((Vector3.Distance (Enemy [i].transform.position, transform.position) > AttackDistance) || Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Enemy [i].GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, DetectObstacleMask)) {
								Target = FakeTarget.transform;
								AlliedShooterTarget = null;
							} 
						}
					} else {
						Target = FakeTarget.transform;
						AlliedShooterTarget = null;
					}
				} 
				else 
				{
					Target = FakeTarget.transform;
					AlliedShooterTarget = null;
					Enemy.Remove (Enemy [i]);
				}
			}
		}
	}
	void OnAnimatorIK()
	{
		if (enemystat.health > 0) 
		{
			if (!IsAllied) {
				AimPivot.LookAt (LookTarget); //使AimPivot看向其目標LookTarget，也就是角色朝向的目標。
			} else {
				AimPivot.LookAt (LookTarget);
			}
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

		Vector3 direction = Target.position - this.transform.position;
		float angle = Vector3.Angle (direction, this.transform.forward);
		direction.y = 0;

		if (!IsReloading) {
			IK (true);
		} else {
			IK (false);
		}

		if (State == "patrol" && waypoints.Length > 0) {
			ShootDelay = 3f;
			if (Vector3.Distance (waypoints [currentWP].transform.position, transform.position) < accuracyWP) {
				if (RandomPotral) {
					currentWP = Random.Range (0, waypoints.Length);
				} else {
					if (currentWP != waypoints.Length - 1) {
						currentWP++;
					}
					else if (currentWP == waypoints.Length - 1) {
						if (!Potral_Only_Once) {
							currentWP = 0;
						} else {

						}
					}
				}
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

		if ((Vector3.Distance (Target.position, this.transform.position) < DetectedDistance && !Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Target.position, DetectObstacleMask) && (angle < DetectedAngle || charaterstatus.isFireing)) || (State == "Detected")) {
			State = "Detected";

			if (!IsChaser) {
				if (CoverPoint) {
					if (Vector3.Distance (CoverPoint.transform.position, this.transform.position) < 1) {
						if(AimDelay > 0)
						{
							AimDelay -= Time.deltaTime * 1f;
						}

						if (!IsCroucher) {
							StandIdle ();
						} else {
							CrouchIdle ();
						}

						if (!IsReloading) {
							if (AimDelay <= 0) {
								Aim (true);
							}
						} else {
							Aim (false);
						}

					} else {
						Run ();
						agent.SetDestination (CoverPoint.transform.position);
					}
				} else {
					if(AimDelay > 0)
					{
						AimDelay -= Time.deltaTime * 1f;
					}

					if (!IsCroucher) {
						StandIdle ();
					} else {
						CrouchIdle ();
					}

					if (!IsReloading) {
						if (AimDelay <= 0) {
							Aim (true);
						}
					} else {
						Aim (false);
					}
				}
			} else {
				if (Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Target.position, DetectObstacleMask)) {
					Run ();
					Aim (false);
					agent.SetDestination (Target.position);
					AimDelay = 0.5f;
				} else {
					if(AimDelay > 0)
					{
						AimDelay -= Time.deltaTime * 1f;
					}
					if (!IsCroucher) {
						StandIdle ();
					} else {
						CrouchIdle ();
					}

					if (!IsReloading) {
						if (AimDelay <= 0) {
							Aim (true);
						}
					} else {
						Aim (false);
					}
				}
			}
		}
	}

	void stateAllied()
	{


		float accuracyWP = 2.0f;

		if (Activated) {
			if (State == "patrol" && waypoints.Length > 0 && AlliedShooterTarget == null) {

				if (WeaponsFree) {
					ChooseEnemy ();
				}

				Aim (false);
				float step = 3f * Time.deltaTime;
				LookTarget.transform.position = Vector3.MoveTowards (LookTarget.transform.position, FakeTarget.transform.position, step);

				if (currentWP < waypoints.Length) {
					if (Vector3.Distance (waypoints [currentWP].transform.position, transform.position) < accuracyWP) {
						currentWP++;
					} else {
						Walk ();
						agent.speed = WalkingSpeed;
						agent.SetDestination (waypoints [currentWP].transform.position);
					} 
				} else {
					if (WeaponsFree) {
						GetComponent<Sentence> ().sentence [0] = AlliedShooterOff;
						GetComponent<Sentence> ().Count = 0;
						GetComponent<Sentence> ().isTriggered = false;
						GetComponent<Sentence> ().StartSentence ();
						WeaponsFree = false;
					}
					StandIdle ();
				}
			} else if (State == "patrol" && waypoints.Length == 0 && AlliedShooterTarget == null) {
				StandIdle ();
				if (WeaponsFree) {
					ChooseEnemy ();
				}
				Aim (false);
				float step = 3f * Time.deltaTime;
				LookTarget.transform.position = Vector3.MoveTowards (LookTarget.transform.position, FakeTarget.transform.position, step);
			}
		} else {
			State = "patrol";
			Target = FakeTarget.transform;
			AlliedShooterTarget = null;
			float step = 3f * Time.deltaTime;
			LookTarget.transform.position = Vector3.MoveTowards(LookTarget.transform.position, FakeTarget.transform.position, step);
			Aim (false);
			StandIdle ();
		}
		if ((State == "Detected" || AlliedShooterTarget != null) && WeaponsFree) {
			
			if (!IsCroucher) {
				StandIdle ();
			} else {
				CrouchIdle ();
			}
				
			if (Enemy.Count > 0) {
				if (!AlliedShooterTarget) {
					ChooseEnemy ();
					AimDelay = 1f;
				} else {
					if (AlliedShooterTarget.GetComponent<EnemyStat> ().health > 0) {
						if (AimDelay > 0) {
							AimDelay -= Time.deltaTime * 1f;
						} else {
							Aim (true);
						}
					} else {
						AlliedShooterTarget = null;
						Aim (false);
					}
				}
			} else {
				Target = FakeTarget.transform;
				float step = 3f * Time.deltaTime;
				LookTarget.transform.position = Vector3.MoveTowards(LookTarget.transform.position, FakeTarget.transform.position, step);
				AlliedShooterTarget = null;
				Aim (false);
			}
				
			/*
			if (AlliedShooterTarget) {
				if (AlliedShooterTarget.tag == "Enemy") {
					if (AlliedShooterTarget.GetComponent<EnemyStat> ().health > 0) {
						if (AimDelay > 0) {
							AimDelay -= Time.deltaTime * 1f;
						} else {
							Aim (true);
						}
					} else {
						AimDelay = 0.5f;
						ChooseEnemy ();
					}
				} else {
					ChooseEnemy ();
					Aim (false);
				}
			} else {
				Target = FakeTarget.transform;
				Aim (false);
			}
			*/
		} else {
			State = "patrol";
			Target = FakeTarget.transform;
			AlliedShooterTarget = null;
			float step = 3f * Time.deltaTime;
			LookTarget.transform.position = Vector3.MoveTowards(LookTarget.transform.position, FakeTarget.transform.position, step);
			Aim (false);
		}



		if (!IsReloading) {
			IK (true);
		}
				
		if (AlliedShooterTarget) {
			if (AlliedShooterTarget.GetComponent<CamShooter> ()) {
				if (Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, AlliedShooterTarget.GetComponent<CamShooter> ().GunRoot.transform.position, DetectObstacleMask)) {
					//State = "fuck";
					Target = FakeTarget.transform;
					AlliedShooterTarget = null;
				} 
			} else {
				if (Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, AlliedShooterTarget.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, DetectObstacleMask)) {
					//State = "fuck";
					Target = FakeTarget.transform;
					AlliedShooterTarget = null;
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
			anim.CrossFadeInFixedTime("Fire", 0.01f);
			r_Hand.gameObject.GetComponent<Animator>().CrossFadeInFixedTime("Shot", 0.01f);
			shotPoint.Rotate (shotPoint.rotation.x + Random.Range (-gunspread, gunspread), shotPoint.rotation.y + Random.Range (-gunspread, gunspread), shotPoint.rotation.z + Random.Range (-gunspread, gunspread));
			if (!IsAllied) {
				GameObject.Find ("BulletPool").GetComponent<ShooterBulletPool> ().SpawnBullet (shotPoint.position, shotPoint.rotation, damage, BulletColor, 50, 1f);
			} else {
				GameObject.Find ("BulletPool").GetComponent<AlliedShooterBulletPool> ().SpawnBullet (shotPoint.position, shotPoint.rotation, damage, BulletColor);
			}
			AbleToShoot = false;
			MuzzleFlash.SetActive (false);
			MuzzleFlash.SetActive (true);
			Invoke ("EnableShoot", RateOfFire);
			if (!IsAllied) {
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

	void IK(bool On)
	{
		if (On) {
			lh_Weight += Time.deltaTime * 2;
		} else {
			lh_Weight -= Time.deltaTime * 10;
		}

		if (l_Hand_Target != null) //如果l_Hand_Target有被獲取為任意物件
		{
			l_Hand.position = l_Hand_Target.position; //把l_Hand的位置設定為l_Hand_Target之位置
			l_Hand.rotation = l_Hand_Target.rotation; //把l_Hand的旋轉值設定為l_Hand_Target之旋轉值
		}
		lh_Weight = Mathf.Clamp (lh_Weight, 0, 1);
	}

	void Aim(bool On)
	{

		if (On || isAimingDebug) {
			anim.SetBool ("IsAiming", true);
			rh_Weight += Time.deltaTime * 2; //以Time.deltaTime(秒)為單位，增加rh_Weight(右手權重值)
			lk_Weight += Time.deltaTime * 10; //解釋同上，增加的是角色朝向IK值。
		} else {
			anim.SetBool ("IsAiming", false);
			rh_Weight -= Time.deltaTime * 2; //以Time.deltaTime(秒)為單位，減少rh_Weight(右手權重值)
			if (!IsAllied) {
				if (!IsReloading && Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Target.position, DetectObstacleMask)) {
					lk_Weight -= Time.deltaTime * 10; //解釋同上，減少的是角色朝向IK值。
				}
			} else {
				lk_Weight -= Time.deltaTime * 10;
			}
		}



		rh_Weight = Mathf.Clamp (rh_Weight, 0, 1); //以此為範例，設定rh_Weight值，最小為0，最大為1
		lk_Weight = Mathf.Clamp (lk_Weight, 0, 10);

		if (enemystat.health <= 0) {
			return;
		}

		shotPoint.LookAt (Target);

		float step = AimSpeed * Time.deltaTime;
		LookTarget.transform.position = Vector3.MoveTowards(LookTarget.transform.position, Target.transform.position, step);


		Vector3 direction = Target.position - this.transform.position;
		Vector3 directionShotPoint = Target.position - AimPivot.transform.position;
		float angle = Vector3.Angle (direction, this.transform.forward);
		float angleShotPoint = Vector3.Angle (directionShotPoint, AimPivot.transform.forward);
		direction.y = 0;

		if (angle > RotAngle) {
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), RotSpeed * Time.deltaTime);
		}

		if (IsAllied) {
			if (angleShotPoint < 20 && !Physics.Linecast (shotPoint.position, Target.position, DetectObstacleMask) && rh_Weight >= 0.5f && lk_Weight >= 5 && !tasksmanager.GamePaused) {

				if (ShootDelay <= 0) {
					Shoot ();
				} else {
					ShootDelay -= Time.deltaTime;
				}
			} else if (Ammo < 1) {
				Reload ();
			}
		}

		if (!IsAllied) {
			if (angleShotPoint < 20 && Ammo > 0 && !IsReloading && !Physics.Linecast (anim.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position, Target.position, DetectObstacleMask) && rh_Weight >= 0.5f && lk_Weight >= 5 && !tasksmanager.GamePaused) {
			
				if (ShootDelay <= 0) {
					Shoot ();
				} else {
					ShootDelay -= Time.deltaTime;
				}
			} else if (Ammo < 1) {
				Reload ();
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
