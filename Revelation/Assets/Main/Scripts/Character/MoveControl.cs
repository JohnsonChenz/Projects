using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveControl : MonoBehaviour {

	public Animator anim;

	public Slider healthbar;


    public float AimMoveX,AimMoveY;
	public float NormalMoveMount;

	public CharaterStatus charaterstatus;
	public CharaterInventory charaterinventory;
	public CharaterIK charaterik;
	public Transform MainCameraTransform;

	public bool mc = true;

	public float vertical;
	public float horizontal;
	public float rotationSpeed;

	public Vector3 rotationDirection;
	public Vector3 moveDirection;

	public float BaseSpeed = 0.03f;
	public float RunSpeedMuiltiple;
	public float AimSpeedMuiltiple;

    public float speed;

	public bool FPSMode = false;
	public bool IgnoreMc = false;

	public Transform ClimbTarget;

	public bool IsWalking;
	public bool IsRunning;

	public GameObject CrosshairManager;
	public CircleProgress Circleprogress;
    // Use this for initialization
	public TasksManager tasksmanager;

	void Awake()
	{
		anim = this.GetComponent<Animator> ();
	}

    void Start () {
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		Circleprogress = GameObject.Find ("CircleProgress").gameObject.GetComponent<CircleProgress>();
		CrosshairManager = GameObject.Find ("CrosshairManager").gameObject;
		charaterstatus.isDoAction = false;
		charaterinventory = GetComponent<CharaterInventory> ();
		charaterik = GetComponent<CharaterIK> ();
		BaseSpeed = 0.03f;
		RunSpeedMuiltiple = 3.5f;
		anim.SetFloat ("AimMoveX", 0);
		anim.SetFloat ("AimMoveY", 0);
		anim.SetFloat ("NormalMoveAmount", 0);
		mc = true;         
		IsWalking = false;
		IsRunning = false;
		//CrosshairOn ();
    }
		
	void OnEnable()
	{
		CrosshairOn ();
	}
	// Update is called once per frame
	void FixedUpdate () {
		
		if (charaterstatus.isGround) {
			anim.SetBool ("IsGrounded", true);
		} 
		else if(!charaterstatus.isGround && !anim.applyRootMotion && !charaterstatus.isDoAction)
		{
			anim.SetBool ("IsGrounded", false);
		}


		if (charaterstatus.isDeath == true) {
			return;
		}



		anim.SetFloat ("AimMoveX", AimMoveX);
		anim.SetFloat ("AimMoveY", AimMoveY);
		anim.SetFloat ("NormalMoveAmount", NormalMoveMount);






		if (mc == false || tasksmanager.GamePaused) {
			if (NormalMoveMount > 0) {
				NormalMoveMount -= Time.deltaTime * 2;
			} else if(NormalMoveMount < 0){
				NormalMoveMount = 0;
			}

			if (IgnoreMc) {
			    MoveUpdate ();
			} 
			speed = 0;
			AimMoveX = 0;
			AimMoveY = 0;
			return;
		}

		if (charaterstatus.isAiming) {
			

			NormalMoveMount = 0.5f;
			speed = Mathf.Clamp(speed, 0, BaseSpeed*AimSpeedMuiltiple);
            if (Input.GetKey ("a")) 
			{
				/*if (Input.GetKey (KeyCode.LeftShift)) 
				{
					
					if (!Input.GetKey ("w") && !Input.GetKey ("s")) {
						speed *= 2.5f;
					}

					if (AimMoveX > -0.97f) {
						AimMoveX -= Time.deltaTime * 3;
					}
				} 
				else 
				{*/
					if (AimMoveX < -0.47f) {
						AimMoveX += Time.deltaTime * 2;
					} 
					else if (AimMoveX > -0.45f) 
					{
						AimMoveX -= Time.deltaTime * 1;
					}
                //}
                transform.Translate (-speed, 0, 0);
			} 
			else if (!Input.GetKey ("a") && AimMoveX < 0) 
			{
				AimMoveX += Time.deltaTime * 3;
			}




			if (Input.GetKey ("d")) 
			{
			
				/*if (Input.GetKey (KeyCode.LeftShift)) 
				{
					if (!Input.GetKey ("w") && !Input.GetKey ("s")) 
					{
						speed *= 2.5f;
					}

					if (AimMoveX < 0.97f) {
						AimMoveX += Time.deltaTime * 3;
					}
				} 
				else 
				{*/
					if (AimMoveX > 0.47f) 
					{
						AimMoveX -= Time.deltaTime * 2;
					} 
					else if (AimMoveX < 0.45f) 
					{
						AimMoveX += Time.deltaTime * 1;
					}
                //}
                transform.Translate (speed, 0, 0);
			} 
			else if (!Input.GetKey ("d") && AimMoveX > 0) 
			{
				AimMoveX -= Time.deltaTime * 3;
			}




			if (Input.GetKey ("w")) 
			{
				/*if (Input.GetKey (KeyCode.LeftShift)) 
				{
					speed *= 2.5f;
					if (AimMoveY < 0.97f) {
						AimMoveY += Time.deltaTime * 3;
					}
				} 
				else 
				{*/
					if (AimMoveY > 0.47f) 
					{
						AimMoveY -= Time.deltaTime * 2;
					} 
					else if (AimMoveY < 0.45f) 
					{
						AimMoveY += Time.deltaTime * 1;
					}

                //}
                transform.Translate (0, 0, speed);
			} 
			else if (!Input.GetKey ("w") && AimMoveY > 0) 
			{
				AimMoveY -= Time.deltaTime * 3;
			}





			if (Input.GetKey ("s")) 
			{
				/*if (Input.GetKey (KeyCode.LeftShift)) 
				{
					speed *= 2;
					if (AimMoveY > -0.97f) {
						AimMoveY -= Time.deltaTime * 3;
					}
				} 
				else 
				{*/
					if (AimMoveY < -0.47f) {
						AimMoveY += Time.deltaTime * 2;
					} 
					else if (AimMoveY > -0.45f) 
					{
						AimMoveY -= Time.deltaTime * 1;
					}
                //}
                transform.Translate (0, 0, -speed);
			} 
			else if (!Input.GetKey ("s") && AimMoveY < 0) 
			{
				AimMoveY += Time.deltaTime * 3;
			}



			if (!Input.GetKey ("w") && !Input.GetKey ("s") && !Input.GetKey ("a") && !Input.GetKey ("d")) {

                speed -= Time.deltaTime * 0.1f;

                if (AimMoveY > 0 && AimMoveY < 0.07f) {
					AimMoveY = 0;
				}

				if (AimMoveY > 0.065f && AimMoveY < 0) {
					AimMoveY = 0;
				}

				if (AimMoveX < 0 && AimMoveX > -0.067f) {
					AimMoveX = 0;
				}

				if (AimMoveX > -0.034f && AimMoveX < 0) {
					AimMoveX = 0;
				}
			}
            else
            {
                speed += Time.deltaTime * 0.5f;
            }
		} 
		else 
		{
			if (!FPSMode) {

				if (Input.GetKey ("w") || Input.GetKey ("s") || Input.GetKey ("a") || Input.GetKey ("d")) {
					if (!Input.GetKey (KeyCode.LeftShift)) {
						if (speed > BaseSpeed) {
							speed -= Time.deltaTime * 0.1f;
						} else if (speed < BaseSpeed) {
							speed += Time.deltaTime * 0.1f;
						}

						//speed = Mathf.Clamp(speed, 0, BaseSpeed);
						if (NormalMoveMount < 0.52f) {
							NormalMoveMount += Time.deltaTime * 1.5f;
						}
						if (NormalMoveMount > 0.54f) {
							NormalMoveMount -= Time.deltaTime * 1;
						}
						IsWalking = true;
						IsRunning = false;
					} else {
						if (speed < BaseSpeed * RunSpeedMuiltiple) {
							speed += Time.deltaTime * 0.1f;
						} else if (speed > BaseSpeed * RunSpeedMuiltiple) {
							speed -= Time.deltaTime * 0.1f;
						}
						speed = Mathf.Clamp (speed, 0, BaseSpeed * RunSpeedMuiltiple);
						if (NormalMoveMount < 0.97f) {
							NormalMoveMount += Time.deltaTime * 1.5f;
						}
						IsWalking = false;
						IsRunning = true;
					}
                
					transform.Translate (0, 0, speed);
				} else if (!Input.GetKey ("w") && !Input.GetKey ("s") && !Input.GetKey ("a") && !Input.GetKey ("d")) {
					//speed = Mathf.Clamp(speed, 0, BaseSpeed);

					transform.Translate (0, 0, speed);

					if (speed > 0) {
						speed -= Time.deltaTime * 0.15f;
					} else if(speed < 0){
						speed = 0;
					}

					if (NormalMoveMount > 0) {
						NormalMoveMount -= Time.deltaTime * 2;
					} else if(NormalMoveMount < 0){
						NormalMoveMount = 0;
					}
			
				}
			} else {

				if(Input.GetKey(KeyCode.LeftShift))
					speed = 0.07f;
				if (Input.GetKey ("w"))
					transform.Translate(0, 0, speed);
				if (Input.GetKey ("s"))
					transform.Translate(0, 0, -speed);
				if (Input.GetKey ("a"))
					transform.Translate(-speed, 0, 0);
				if (Input.GetKey ("d"))
					transform.Translate(speed, 0, 0);
			}

        }

		MoveUpdate ();

	}

	void Update()
	{
		if (charaterstatus.isDoAction || charaterstatus.isDeath || tasksmanager.GamePaused)
			return;

		if (Input.GetKeyDown (KeyCode.Space))
		{
			charaterstatus.isDoAction = true;
			anim.SetTrigger("isJumping");
			IgnoreMc = true;
			OpenRootMotion();
			anim.applyRootMotion = false;
			//GetComponent<Rigidbody>().useGravity = false;
		}

		if (mc != false) {
			if (Input.GetKeyDown (KeyCode.C)) {
				anim.SetTrigger ("Cross");
				OpenRootMotion ();
				ITweenFrontMove (gameObject, 2.5f, 1);
				this.GetComponent<Rigidbody> ().isKinematic = true;
			}
        

			if (Input.GetKeyDown (KeyCode.E)) {
				if (ClimbTarget != null) {
					this.GetComponent<Rigidbody> ().isKinematic = true;
					mc = false;
					OpenRootMotion ();
					Climb (0);
					charaterstatus.isDoAction = true;
				}
			}
		}
		/*
		if (Input.GetKeyDown (KeyCode.X))
		{
			anim.SetTrigger("Step Up");
			OpenRootMotion ();
			ITweenVector3Move (gameObject, 0, 0.1f, 0, 0.2f);
			this.GetComponent<Rigidbody> ().isKinematic = true;
		}
		*/
	}
	 




	public void OnAttackEnter()
	{
		mc = false;
	}

	public void OnAttackExit()
	{
		mc = true;
	} 

	public void ResetTrigger(string triggerName){
		anim.ResetTrigger (triggerName);
	}


	public void OffRootMotion()
	{
		
		if (!GetComponent<CapsuleCollider> ().enabled) {
			GetComponent<CapsuleCollider> ().enabled = true;
		}
		if (GetComponent<CharaterInventory> ().Weaponcount != 0 && GetComponent<CharaterInventory> ().Weaponcount != 3) {
			GetComponent<CharaterInventory> ().WeaponAvailable = true;
		}
		if (anim.applyRootMotion) {
			anim.applyRootMotion = false;
		}
		charaterstatus.isDoAction = false;
		IgnoreMc = false;
		mc = true;
		this.GetComponent<Rigidbody> ().isKinematic = false;
		RecoverWeapon ();
	}

	public void OpenRootMotion()
	{
		if (GetComponent<CharaterInventory> ().Weaponcount != 0) {
			GetComponent<CharaterInventory> ().WeaponAvailable = false;
		}
		//anim.applyRootMotion = true;
		mc = false;
	}

	public void StepUp()
	{
		iTween.Stop();
		ITweenVector3Move (gameObject, 0, 0.8f, 1, 0.4f);

		//ITweenUpMove(gameObject,0.8f,0.2f);
		//ITweenFrontMove (gameObject, 0.5f, 0.3f);
	}
	public void StepUp2()
	{
		//iTween.Stop();
		//ITweenVector3Move (gameObject, 0, 0, 0.5f, 0.2f);
		//ITweenUpMove(gameObject,1f,0.3f);
		//ITweenFrontMove (gameObject, 0.6f, 0.2f);
	}

	public void Climb(int Stage)
	{
		if (Stage == 0) {
			
			for (int i = 0; i < ClimbTarget.childCount; i++) {
					Vector3 OriginPos = ClimbTarget.GetChild (i).localPosition;
					ClimbTarget.GetChild (i).position = new Vector3 (transform.position.x, ClimbTarget.GetChild (i).position.y, transform.position.z);
					ClimbTarget.GetChild (i).localPosition = new Vector3 (ClimbTarget.GetChild (i).localPosition.x, ClimbTarget.GetChild (i).localPosition.y, OriginPos.z);
		
			}
			ITweenRotateTo (gameObject, ClimbTarget.GetComponent<Climb> ().RotAdjust, 0.3f);
			anim.SetTrigger("Climb");
			ITweenPosMove (gameObject, ClimbTarget.GetChild(0), 0.3f);
		}
		if (Stage == 1) {
			ITweenPosMove (gameObject, ClimbTarget.GetChild(1), 0.3f);
		}
		if (Stage == 2) {
			ITweenPosMove (gameObject, ClimbTarget.GetChild(2), 0.3f);
		}
		if (Stage == 3) {
			ITweenPosMove (gameObject, ClimbTarget.GetChild(3), 0.3f);
			charaterstatus.isDoAction = false;
			ClimbTarget = null;
		}
	}



	public bool Ground()
	{
		Vector3 origin = transform.position;
		origin.y += 0.6f;
		Vector3 dir = -Vector3.up;
		float dis = 1f;
		RaycastHit hit;
		if(Physics.Raycast(origin, dir, out hit, dis))
		{
			//Vector3 tp = hit.point;
			//transform.position = tp;
			return true;
		}
		return false;
	}

	public void ITweenFrontMove(GameObject gameobject,float Amout,float Duration)
	{
		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("amount", new Vector3(0, 0, Amout));
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);
		iTween.MoveAdd (gameobject, moveSetting);
	}

	public void ITweenFrontMoveAttack(AnimationEvent evt)
	{
		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("amount", new Vector3(0, 0, evt.intParameter));
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", evt.floatParameter);
		iTween.MoveAdd (this.gameObject, moveSetting);
	}

	public void ITweenUpMove(GameObject gameobject,float Amout,float Duration)
	{
		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("amount", new Vector3(0, Amout, 0));
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);
		iTween.MoveAdd (gameObject, moveSetting);
	}

	public void ITweenVector3Move(GameObject gameobject,float AmoutX,float AmoutY,float AmoutZ,float Duration)
	{
		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("amount", new Vector3(AmoutX, AmoutY, AmoutZ));
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);
		iTween.MoveAdd (gameObject, moveSetting);
	}

	public void ITweenPosMove(GameObject gameobject,Transform Pos,float Duration)
	{
		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("position", Pos);
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);
		iTween.MoveTo (gameObject, moveSetting);
	}

	public void ITweenLookat(GameObject gameobject,Transform Target,float Duration)
	{
		Hashtable moveSetting = new Hashtable();

		moveSetting.Add("looktarget", Target);
		moveSetting.Add("axis", "y");
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);
		iTween.LookTo(gameobject, moveSetting);
	}

	public void ITweenRotateTo(GameObject gameobject,float yAxis,float Duration)
	{
		Hashtable moveSetting = new Hashtable();
		moveSetting.Add("rotation", new Vector3(0, yAxis, 0));
		moveSetting.Add("easetype", iTween.EaseType.linear);
		moveSetting.Add("time", Duration);
		iTween.RotateTo(gameobject, moveSetting);
	}

	public void MoveUpdate()
	{
		vertical = Input.GetAxis ("Vertical");
		horizontal = Input.GetAxis ("Horizontal");


		Vector3 moveDir = MainCameraTransform.forward * vertical;
		moveDir += MainCameraTransform.right * horizontal;
		moveDir.Normalize ();
		moveDirection = moveDir;
		rotationDirection = MainCameraTransform.forward;

		RotationNormal ();
		//charaterstatus.isGround = Ground ();

	}

	public void RotationNormal()
	{
		if (!charaterstatus.isAiming && !FPSMode) {
			rotationDirection = moveDirection;
			rotationSpeed = 0.15f;

		}
		else if (charaterstatus.isAiming || FPSMode)
		{
			//rotationDirection = moveDirection;
			rotationSpeed = 0.5f;
		}

		Vector3 targetDir = rotationDirection;
		targetDir.y = 0;

		if (targetDir == Vector3.zero)
			targetDir = transform.forward;

		Quaternion lookDir = Quaternion.LookRotation (targetDir);
		Quaternion targetRot = Quaternion.Slerp (transform.rotation, lookDir, rotationSpeed);
		transform.rotation = targetRot;
	}

	public void CoverWeapon()
	{		
		if (charaterinventory.Weaponcount != 0) {
			if (charaterinventory.activeMeleeWeapon) {
				charaterinventory.activeMeleeWeapon.WeaponSelf.SetActive (false);
			}

			if (charaterinventory.activeWeapon) {
				charaterinventory.activeWeapon.WeaponSelf.SetActive (false);
			}
			charaterik.r_Hand.gameObject.GetComponent<Animator> ().runtimeAnimatorController = null;
			charaterinventory.WeaponAvailable = false;
		}
	}

	public void RecoverWeapon()
	{

		if (charaterinventory.Weaponcount != 0) 
		{

			if (charaterinventory.activeMeleeWeapon) {
				charaterinventory.activeMeleeWeapon.WeaponSelf.SetActive (true);
			}

			if (charaterinventory.activeWeapon) {
				charaterik.r_Hand.gameObject.GetComponent<Animator> ().runtimeAnimatorController = charaterinventory.activeWeapon.WeaponProperties.RightHandAnim;
				charaterik.r_Hand.localPosition = charaterinventory.activeWeapon.WeaponProperties.rHandPos;
				Quaternion rotRight = Quaternion.Euler (charaterinventory.activeWeapon.WeaponProperties.rHandRot.x, charaterinventory.activeWeapon.WeaponProperties.rHandRot.y, charaterinventory.activeWeapon.WeaponProperties.rHandRot.z);
				charaterik.r_Hand.localRotation = rotRight;
				charaterinventory.activeWeapon.WeaponSelf.SetActive (true);
			}

			if (charaterinventory.Weaponcount != 3 && charaterinventory.Weaponcount != 0) {
				charaterinventory.WeaponAvailable = true;
			}
		} 
	}
	public void StartCircleProgress(string i)
	{
		Circleprogress.StartCircleProgress(i);
	}

	public void CrosshairOn()
	{
		CrosshairManager.gameObject.SetActive(true);
	}

	public void CrosshairOff()
	{
		CrosshairManager.gameObject.SetActive(false);
	}

	/*
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Debug.DrawLine (transform.position, transform.position + transform.forward * currenthitdistance);
		Gizmos.DrawWireSphere (p1, hitradius);
		Gizmos.DrawWireSphere (p2, hitradius);
	}
    */
}

