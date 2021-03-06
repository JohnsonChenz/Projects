using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBoss : MonoBehaviour {

	public Animator anim;
	public string sector;
	public bool isAttacking;
	public bool isStunned;
	public int LBcooldown;
	public int LScooldown;
	public float Pipe_Left_HP;
	public float Pipe_Right_HP;
	public float hp;
	public float Max_hp;
	public bool isInvincible;

	public int Stage;

	public TasksManager tasksmanager;

	public GameObject Walker_1;
	public GameObject Walker_2;
	public GameObject Runner_1;
	public GameObject Runner_2;
	public int Mon_Count;
	public int Max_Mon_Count;
	public float Summon_Mon_Cooldown;
	public GameObject Runnder_1_Red;
	public GameObject Runnder_2_Red;
	public int Summon_Red_Mon_Time;
	public int Stage_1_KillGoal;
	public int Stage_2_KillGoal;
	public GameObject EliteMon;

	public Transform SpawnPosRight;
	public Transform SpawnPosLeft;

	public GameObject[] ShockWave;

	public Transform LaserObj;

	public Transform PlayerCam;

	public SFXControllerV3D LaserController;
	public GameObject LaserLine;
	public LightingBallAttack LtbAtk;

	public float LtbAtkCooldown;
	public float LSAtkCooldown;
	public float StunTime;

	public bool isDead;

	public GameObject CrystalHeart;
	public GameObject CrystalBloodExplosionObj;
	public Material[] CrystalMet;
	public Light CrystalLight;
	public int Crystalstage;
	public Transform Scene;

	// Use this for initialization
	void Start () {
		hp = Max_hp;
		PlayerCam = GameObject.Find ("Camera").transform;
		isInvincible = true;
		anim = GetComponent<Animator> ();
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		Scene = GameObject.FindGameObjectWithTag ("Scene").transform;
		Crystalstage = 1;
		CrystalStage (Crystalstage);
	}
	
	// Update is called once per frame
	void Update () {

		if (hp < 0 && !isDead) {
			anim.SetTrigger ("Dead");
			LaserObj.gameObject.SetActive (false);
			LtbAtk.gameObject.SetActive (false);
			GameObject[] obj = GameObject.FindGameObjectsWithTag("Monster");
			for (int i = 0; i < obj.Length; i++) {
				if (obj [i].GetComponent<EnemyStat> ()) {
					obj [i].GetComponent<EnemyStat> ().Dead ();
				}
			}
			isDead = true;
			GetComponent<ChangeLevel> ().Invoke ("ForceChangeScene", 3f);
			CrystalStage (3);
		}
			
		if (tasksmanager.GamePaused) {
			anim.speed = 0;
			//agent.speed = 0;
			//this.GetComponent<AudioSource> ().Pause ();
		} else {
			//this.GetComponent<AudioSource> ().UnPause ();
			if (anim.speed == 0) {
				if (Stage == 1 || Stage == 0) {
					anim.speed = 1;
				} else if (Stage == 2) {
					anim.speed = 1.1f;
				} else if (Stage == 3) {
					anim.speed = 1.2f;
				}
			}

			if (Mon_Count <= Max_Mon_Count && Stage > 0 && !isStunned) {
				if (Summon_Mon_Cooldown > 0) {
					Summon_Mon_Cooldown -= Time.deltaTime;
				} else {
					SummonNormanMons ();
					Summon_Mon_Cooldown = 20f;
				}
			}
		}

		if (Stage < 1 || isStunned) {
			return;
		}

		if (Stage >= 2) {
			if (LtbAtkCooldown > 0) {
				LtbAtkCooldown -= Time.deltaTime * 1;
			} else {
				LtbAtkCooldown = 0;
				anim.SetBool ("LBAttack", true);
			}
		}

		if (Stage >= 3) {
			if (LSAtkCooldown > 0) {
				LSAtkCooldown -= Time.deltaTime * 1;
			} else {
				LSAtkCooldown = 0;
				anim.SetBool ("LSAttack", true);
			}
		}
			


		if(!isAttacking)
		{
			if (sector == "Left") {
				anim.SetBool ("LeftAttack", true);
				isAttacking = true;
			}
			else if(sector == "Right")
			{
				anim.SetBool ("RightAttack", true);
				isAttacking = true;
			}
			else if(sector == "Mid")
			{
				anim.SetBool ("MidAttack", true);
				isAttacking = true;
			}
		}
	}

	void StartAttack()
	{
		isAttacking = true;
	}

	void EndAttack()
	{
		isAttacking = false;
		anim.SetBool ("LeftAttack", false);
		anim.SetBool ("RightAttack", false);
		anim.SetBool ("MidAttack", false);
	    anim.SetBool ("LSAttack", false);
		anim.SetBool ("LBAttack", false);
	}

	public void StartLaserAttack()
	{
		isAttacking = true;
		LSAtkCooldown = 30f;
	}

	public void StartLightBallAttack()
	{
		isAttacking = true;
		LtbAtkCooldown = 63f;
	}

	public void TakeAwayHealth(float HP)
	{
		
		if (!isInvincible || !tasksmanager.GamePaused) {
			hp -= HP;
		}

		if (hp < (Max_hp * 0.5f) && Crystalstage != 2) {
			Crystalstage = 2;
			CrystalStage (Crystalstage);
		}
	}

	public void AddStage()
	{
		Stage++;
	}

	public void Shockwave(int i)
	{
		ShockWave [i-1].SetActive (false);
		ShockWave [i-1].SetActive (true);
	}

	public void PlayerCamShake(AnimationEvent evt)
	{
		PlayerCam.GetComponent<CameraShake> ().shakeMagnitude = evt.intParameter * 0.02f;
		PlayerCam.GetComponent<CameraShake> ().shakeTime = evt.floatParameter;
		PlayerCam.GetComponent<CameraShake> ().ShakeIt ();
	}

	public void LaserBall()
	{
		LaserObj.gameObject.SetActive (true);
		LaserObj.GetComponent<LaserBall> ().ScaleOn = true;
	}

	public void FireLaser(){

		if (!LaserController.onFire) {
		    LaserLine.gameObject.SetActive (true);
			LaserController.onFire = true;
		} else {
			LaserController.onFire = false;
			LaserObj.GetComponent<LaserBall> ().ScaleOn = false;
			Invoke ("LaserOff", 2f);
		}
	}

	public void LaserOff()
	{
		LaserLine.gameObject.SetActive (false);
	}

	public void LightBallAttackOn()
	{
		LtbAtk.On ();
	}

	public void Stunned()
	{
		isInvincible = true;
		EndAttack ();
		anim.SetBool ("isStunned", true);
		isStunned = true;
		if (Stage == 1) {
			Invoke ("SummonRedMons", 10f);
		}
		if (Stage == 2) {
			Invoke ("SummonEliteMons", 10f);
		}
		//Invoke ("EndStun", StunTime);
	}

	public void EndStun()
	{
		Stage++;
		Invoke ("GodModOff", 8f);
		EndAttack ();
		anim.SetBool ("isStunned", false);
		isStunned = false;
		if (Stage == 2) {
			LtbAtkCooldown = 0;
			anim.speed = 1.1f;
		}
		if (Stage == 3) {
			LtbAtk.AttackFreq = 0.4f;
			anim.speed = 1.4f;
			LScooldown = 0;
		}

	}

	public void GodModOff()
	{
		isInvincible = false;
	}

	public void SummonRedMons()
	{
		GameObject M1 = Instantiate (Runnder_1_Red, SpawnPosLeft.position, SpawnPosLeft.rotation,Scene);
		M1.SetActive (true);

		GameObject M2 = Instantiate(Runnder_2_Red,SpawnPosRight.position,SpawnPosRight.rotation,Scene);
		M2.SetActive (true);

		if(Summon_Red_Mon_Time > 0)
		{
		Summon_Red_Mon_Time--;
		Invoke("SummonRedMons", 15f);
		}
	}

	public void SummonEliteMons()
	{
		GameObject M1 = Instantiate (EliteMon, SpawnPosLeft.position, SpawnPosLeft.rotation,Scene);
		M1.SetActive (true);

		GameObject M2 = Instantiate(EliteMon,SpawnPosRight.position,SpawnPosRight.rotation,Scene);
		M2.SetActive (true);
	}

	public void SummonNormanMons()
	{
		if (Stage == 1) {
			if (Mon_Count < Max_Mon_Count) {
				GameObject M1 = Instantiate (Walker_1, SpawnPosLeft.position, SpawnPosLeft.rotation,Scene);
				M1.SetActive (true);
				Mon_Count++;
				if (Mon_Count < Max_Mon_Count) {
					GameObject M2 = Instantiate(Walker_1,SpawnPosRight.position,SpawnPosRight.rotation,Scene);
					M2.SetActive (true);
					Mon_Count++;
				}
			}
		}
		if (Stage == 2) {
			if (Mon_Count < Max_Mon_Count) {
				GameObject M1 = Instantiate (Walker_1, SpawnPosLeft.position, SpawnPosLeft.rotation,Scene);
				M1.SetActive (true);
				Mon_Count++;
				if (Mon_Count < Max_Mon_Count) {
					GameObject M2 = Instantiate(Walker_2,SpawnPosRight.position,SpawnPosRight.rotation,Scene);
					M2.SetActive (true);
					Mon_Count++;
				}
			}
		}
		if (Stage == 3) {
			if (Mon_Count < Max_Mon_Count) {
				GameObject M1 = Instantiate (Runner_1, SpawnPosLeft.position, SpawnPosLeft.rotation,Scene);
				M1.SetActive (true);
				Mon_Count++;
				if (Mon_Count < Max_Mon_Count) {
					GameObject M2 = Instantiate(Runner_2,SpawnPosRight.position,SpawnPosRight.rotation,Scene);
					M2.SetActive (true);
					Mon_Count++;
				}
			}
		}
	}

	public void Boss_Stage_Kill_Goal(int stage)
	{
		if (stage == 1) {
			Stage_1_KillGoal++;
			if (Stage_1_KillGoal >= 10) {
				EndStun ();
			}
		}
		if (stage == 2) {
			Stage_2_KillGoal++;
			if (Stage_2_KillGoal >= 2) {
				EndStun();
			}
		}
	}


	public void CrystalStage(int stage)
	{
		if (stage == 1) {
			CrystalHeart.GetComponent<Renderer> ().material = CrystalMet [0];
		}
		if (stage == 2) {
			CrystalHeart.GetComponent<Renderer> ().material = CrystalMet [1];
			CrystalBloodExplosionObj.SetActive (true);
			CrystalLight.intensity *= 0.5f;
		}
		if (stage == 3) {
			CrystalHeart.GetComponent<Renderer> ().material = CrystalMet [2];
			CrystalBloodExplosionObj.SetActive (true);
			CrystalLight.intensity *= 0;
		}
	}
}
