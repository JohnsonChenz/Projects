using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class AI2 : MonoBehaviour {


	public Transform player;
	public Transform PlayerCam;
	public Animator anim;

	public Slider healthbar;
	public Slider healthbarplayer;

	//string state = "pursuing";
	public string state = "patrol";
	public string statePortal = "walking";
	public string stateDetected = "";
	public GameObject[] waypoints;
	public float DetectedDistance;
	public float DetectedAngle;
	public float AttackDistance = 2;
	public float Runningspeed;
	public float Walkingspeed;
	int currentWP = 0;
	int SwitchCount = 0;
	public int SwitchAttackCount = 0;

	public EnemyStat enemystat;

	float cooldown = 5.0f;

    NavMeshAgent agent;

	public CharaterStatus charaterstatus;

	public MonsterController monstercontroller;

	public bool DetectedbyYell = false;

	public bool isDamaged = false;

    bool Once = false;




	public bool isgo = false;

	public int watch ;
	public int attacktimes;


	public bool at = true;
	public bool canmove = false;
	public bool IsSpawningMonster = false;

	public LayerMask DetectObstacleMask;

	public bool inEMP = false;

	public string opponent;

	public GameObject DamageObj;
	public GameObject DamageObj_2;
	public float AttackRange;

	public GameObject Yell_FX;
	public GameObject ShockWave_FX;

	public bool IsBoss;
	public bool IsThrower;
	public GameObject Knife;
	public GameObject Knife_Throw;
	public Transform ThrowPos;
	public bool HandCover;

	public TasksManager tasksmanager;

	public bool AlliedShooterEnemy;
	public GameObject AlliedShooter;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		PlayerCam = GameObject.Find ("Camera").transform;
		currentWP = Random.Range(0, waypoints.Length);
		//monstercontroller.Monster.Add (this.gameObject);
		anim = GetComponent<Animator> ();
        agent = GetComponent<NavMeshAgent>();
        Once = false;
		HandCover = false;
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		DamageObj.transform.localScale = new Vector3 (AttackRange, AttackRange, AttackRange);
		if (DamageObj_2 != null) {
			DamageObj_2.transform.localScale = new Vector3 (AttackRange, AttackRange, AttackRange);
		}
    }
		
	void OnEnable()
	{
		player = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		PlayerCam = GameObject.Find ("Camera").transform;
		currentWP = Random.Range(0, waypoints.Length);
		//monstercontroller.Monster.Add (this.gameObject);
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent>();
		Once = false;
		HandCover = false;
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		DamageObj.transform.localScale = new Vector3 (AttackRange, AttackRange, AttackRange);
		if (DamageObj_2 != null) {
			DamageObj_2.transform.localScale = new Vector3 (AttackRange, AttackRange, AttackRange);
		}
		if (AlliedShooterEnemy) {
			AlliedShooter.GetComponent<ShooterAi> ().Enemy.Add (this.gameObject);
		}
	}
	// Update is called once per frame
	void Update () {


		if (enemystat.health <= 0) {
			DamageObj.SetActive (false);
			if (DamageObj_2 != null) {
				DamageObj_2.SetActive (false);
			}
			return;
		}

		if (ThrowPos) {
			ThrowPos.LookAt (player.GetComponent<Animator> ().GetBoneTransform (HumanBodyBones.Chest).position);
		}

        float rotSpeed = 0.02f;
        float speed = 0.005f;
        float accuracyWP = 2.0f;

        Vector3 direction = player.position - this.transform.position;
		float angle = Vector3.Angle (direction, this.transform.forward);
		direction.y = 0;
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, -1, 0);


        if (state == "patrol" && waypoints.Length > 0) {
			anim.ResetTrigger ("Attack");
			anim.SetBool ("isRunning", false);
			if (Vector3.Distance (waypoints [currentWP].transform.position, transform.position) < accuracyWP) {
				SwitchPortalSituation ();

				currentWP = Random.Range (0, waypoints.Length);
			}

			if (statePortal == "walking") {
				
				anim.SetBool ("isIdle", false);
				anim.SetBool ("isWalking", true);
				direction = waypoints [currentWP].transform.position - transform.position;
                agent.SetDestination(waypoints[currentWP].transform.position);
				agent.speed = Walkingspeed;
            } else if (statePortal == "idle") {
				anim.SetBool ("isWalking", false);
				anim.SetBool ("isIdle", true);
                agent.SetDestination(transform.position);
                agent.speed = 0;
                if (cooldown > 0) {
					cooldown -= Time.deltaTime;
				} else {
					SwitchPortalSituation ();
				}
			}

		} 
		else if (state == "patrol" && waypoints.Length == 0)
		{
			agent.speed = 0;
			agent.SetDestination(transform.position);
			anim.ResetTrigger ("Attack");
				anim.applyRootMotion = true;
			anim.SetBool ("isIdle", true);
		}


		if (isDamaged && state != "Detected") {
			SwitchCount = 0;
			isDamaged = false;
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
			state = "Detected";
		}
			
		//Debug.DrawLine (this.transform.position, player.position, Color.red);

		if ((Vector3.Distance (player.position, this.transform.position) < DetectedDistance && !Physics.Linecast (transform.position, player.position, DetectObstacleMask) && (angle < DetectedAngle || charaterstatus.isFireing)) || (state == "Detected")) {


			state = "Detected";
			statePortal = "";


			if (SwitchCount < 1 && !DetectedbyYell) {

				SwitchCount++;
				SwitchDetectedSituation ();
			}


			if (DetectedbyYell) {

                rotSpeed *= 7;
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                agent.SetDestination(transform.position);
                if (angle < 10) {
					//stateDetected = "Chase";
					//DetectedbyYell = false;
				}
			}

			if (stateDetected == "Chase") {
				
				anim.SetBool ("isIdle", false);

				if (this.GetComponent<BossSpawn> ()) {
					if (this.GetComponent<BossSpawn> ().CanSpawn) {
						if (SwitchAttackCount < 1) {
							SwitchAttackCount++;
							anim.SetBool ("attack1", false);
							anim.SetBool ("attack2", false);
							anim.SetBool ("attack3", false);
							SwitchAttackSituation ();
							agent.SetDestination (transform.position);
							//this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 1);
							canmove = false;
						}
						return;
					}
				}


				if (Vector3.Distance (player.position, this.transform.position) <= AttackDistance && !IsSpawningMonster) {

					this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.5f);
					canmove = false;
					agent.SetDestination (transform.position);

					anim.SetBool ("isRunning", false);

					if (SwitchAttackCount < 1) {
							SwitchAttackCount++;
							anim.SetBool ("attack1", false);
							anim.SetBool ("attack2", false);
							anim.SetBool ("attack3", false);
							SwitchAttackSituation ();

					}
					anim.applyRootMotion = true;
					if (IsBoss) {
						anim.SetBool ("InCover", false);
					}
					anim.SetBool ("isIdle", false);
					anim.SetBool ("isWalking", false);

				} 
				else 
				{
					SwitchAttackCount = 0;


					if (!anim.GetBool ("attack1") && !anim.GetBool ("attack2") && !anim.GetBool ("attack3") && canmove && !IsSpawningMonster && !inEMP) {
						agent.speed = Runningspeed;
						anim.SetBool ("isRunning", true);
						anim.applyRootMotion = false;
						agent.SetDestination (player.position);
						anim.SetBool ("isIdle", false);
						anim.SetBool ("isWalking", false);
					} else {
						if (canmove && !IsSpawningMonster) {
							this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.1f);
						}
						agent.SetDestination (transform.position);
					}
				}

                anim.SetBool ("isYelling", false);



				if (inEMP == true) {
					anim.speed = 0.3f;
					agent.speed = 0.1f;
				}

			}
				
			/*
			if (stateDetected == "Yell") {
				anim.SetBool("isYelling", true);

                rotSpeed *= 7;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed);
                agent.SetDestination(transform.position);


                if (!Once)
                {
					for (int i = 0; i < monstercontroller.Monster.Count; i++) {
						//Debug.Log (monstercontroller.Monster [i].name);
						if (Vector3.Distance(monstercontroller.Monster [i].transform.position, this.transform.position) < 7)
						{
							if (monstercontroller.Monster [i].GetComponent<AI2>().state != "Detected")
							{
								monstercontroller.Monster [i].GetComponent<AI2>().state = "Detected";aw
								monstercontroller.Monster [i].GetComponent<AI2>().DetectedbyYell = true;		
							}
						}
					}

                    foreach (GameObject child in monstercontroller.Monster)
                    {
                        Debug.Log(child.name);

                        if (Vector3.Distance(child.transform.position, this.transform.position) < 7)
                        {
                            if (child.GetComponent<AI2>().state != "Detected")
                            {
                                child.GetComponent<AI2>().state = "Detected";
                                child.GetComponent<AI2>().DetectedbyYell = true;
                            }
                        }
                    }

                    Once = true;
                                        
                }

			}
			  */
		} 

		if (Vector3.Distance (player.position, this.transform.position) > DetectedDistance || Physics.Linecast (transform.position, player.position, DetectObstacleMask)) 
		{
			SwitchAttackCount = 0;
			anim.SetBool ("attack1", false);
			anim.SetBool ("attack2", false);
			anim.SetBool ("attack3", false);

			SwitchCount = 0;
			DetectedbyYell = false;
			anim.SetBool ("isAttacking", false);
			anim.SetBool ("isRunning", false);
			anim.SetBool ("isYelling", false);
			state = "patrol";
			stateDetected = "";
			statePortal = "walking";
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

	public void YOUCANNOTFUKINGMOVE()
	{
		canmove = false;
	}
    public void StartChase()
    {
        anim.SetBool("isYelling", false);
        stateDetected = "Chase";
    }

	void SwitchPortalSituation()
	{
		int situaltion = Random.Range(0, 2);
		switch (situaltion) {
		case 0:
			statePortal = "walking";
			break;
		case 1:
			statePortal = "idle";
			cooldown = 5.0f;
			break;
		}
	}

	void SwitchDetectedSituation()
	{
		int situaltion = Random.Range(0, 1);
		switch (situaltion) {
		case 0:
			stateDetected = "Chase";
			Once = false;
            //cooldown = 5.0f;
			break;
		}
	}


	void SwitchAttackSituation()
	{
		canmove = false;
		agent.SetDestination (transform.position);
		if (this.GetComponent<BossSpawn> ()) {
			if (!this.GetComponent<BossSpawn> ().CanSpawn) {
				int situaltion = Random.Range (0, 3);
				switch (situaltion) {
				case 0:
					anim.SetBool ("attack1", true);
					break;
				case 1:
					anim.SetBool ("attack2", true);
					break;
				case 2:
					anim.SetBool ("attack3", true);
					break;
				}
			} else {
				anim.SetTrigger ("Yell");
				this.GetComponent<BossSpawn> ().Spawn ();
			}
		}
		else
		{
			int situaltion = Random.Range (0, 3);
			switch (situaltion) {
			case 0:
				anim.SetBool ("attack1", true);
				break;
			case 1:
				anim.SetBool ("attack2", true);
				break;
			case 2:
				anim.SetBool ("attack3", true);
				break;
			}
		}
	}

	public void ExitAttack()
	{
		SwitchAttackCount = 0;
		anim.SetBool ("attack1", false);
		anim.SetBool ("attack2", false);
		anim.SetBool ("attack3", false);
	}

	public void StartMoving()
	{
		canmove = true;
		//enemystat.CantDmg = false;
	}

	void FirstAttack(){
		anim.SetTrigger ("attack1");
		//randomMove ();
	}

	public void PlayerCamShake(AnimationEvent evt)
	{
		PlayerCam.GetComponent<CameraShake> ().shakeMagnitude = evt.intParameter * 0.02f;
		PlayerCam.GetComponent<CameraShake> ().shakeTime = evt.floatParameter;
		PlayerCam.GetComponent<CameraShake> ().ShakeIt ();
	}


	/*
	void randomMove(){
		int randommove = Random.Range (0, 5);

		if(randommove==0||randommove==1){

			goright();
		}

		if(randommove==2||randommove==3){
			goleft();
		}

		if(randommove==4){
			move();
		}

	}

	void goleft(){
		watch = Random.Range (4, 8);


		InvokeRepeating ("timer", 0, 1);
		anim.SetBool ("left", true);
	}




	void goright(){

		//	if (isgo=true) {
		//	go ();
		//}

		watch = Random.Range (4, 8);


		InvokeRepeating ("timer", 0, 1);
		anim.SetBool ("right", true);
	}




	void timer(){
		watch -= 1;

		Debug.Log ("watch");
		if ( watch <= 0) {

			CancelInvoke ("timer");
			anim.SetBool ("right", false);
			anim.SetBool ("left", false);

			//attacktimes = Random.Range (0, 11);
			move ();
		}
	}




	void move(){

		attacktimes = Random.Range (0, 11);

		if (canmove == true) {


			if (attacktimes <= 1) {
				move ();
			} else if (attacktimes >= 2 && attacktimes <= 4) {
				anim.SetTrigger ("attack1");
			} else if (attacktimes >= 5 && attacktimes <= 7) {
				anim.SetTrigger ("attack2");
			} else if (attacktimes >= 8 && attacktimes <= 10) {
				anim.SetTrigger ("attack3");
			}

		}


	}


	void exitattack(){

		randomMove ();
		//ani.SetBool ("right", true);
	}
	*/

	public void OnDamageEnter(){
		DamageObj.SetActive(true);
		if (DamageObj_2 != null) {
			DamageObj_2.SetActive (true);
		}
	}

	public void OnDamageExit(){
		DamageObj.SetActive (false);
		if (DamageObj_2 != null) {
			DamageObj_2.SetActive (false);
		}
	}


	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == opponent) {
			inEMP = true;
			Invoke ("stopEMP", 6f);
		}
	}

	void stopEMP(){
		inEMP = false;
		anim.speed = 1f;
	}

	public void YellFXOn()
	{
		Yell_FX.SetActive (true);
	}

	public void YellFXOff()
	{
		Yell_FX.SetActive (false);
	}

	public void CoverOn()
	{
		if (Vector3.Distance (player.position, this.transform.position) >= 2) {
			anim.SetBool ("InCover", true);
			HandCover = true;
		}
	}

	public void CoverOff()
	{
		HandCover = false;
	}
	public void SpawnShockWaveFX()
	{
		Instantiate (ShockWave_FX, transform.position, transform.rotation);
	}

	public void OpenKnifeModel()
	{
		Knife.SetActive (true);
	}

	public void OffKnifeModel()
	{
		Knife.SetActive (false);
	}

	public void ThrowKnife()
	{
		GameObject obj = Instantiate (Knife_Throw, ThrowPos.position, ThrowPos.rotation);
	}
}
