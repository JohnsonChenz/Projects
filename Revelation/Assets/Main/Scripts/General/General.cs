using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour {

	public Transform Target;
	public GameObject Leg;
	public Animator LegAnim;
	public Animator BodyAnim;
	public float Leg_Speed_Walk;
	public float Leg_Speed_Run;
	public float Leg_Speed_Rotation;
	public float Speed_KnifeCharge;
	public bool CanMove;
	public bool CanRotate;
	public bool UsingAbility;
	public bool KnifeCharge;

	public bool MeleeMode;
	public bool GunMode;
	public bool CanSwitchMode;
	public float ChooseCoolDown;
	// Use this for initialization
	void Start () {
		BodyAnim = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Target) {

			if (CanSwitchMode) {
				CanSwitchMode = false;
				Invoke ("ChooseCooldown", ChooseCoolDown);
				ChooseMode ();
			}

			if (CanRotate) {
				Vector3 direction = Target.position - Leg.transform.position;
				direction.y = 0;
				float angle = Vector3.Angle (direction, Leg.transform.forward);
				Leg.transform.rotation = Quaternion.Slerp (Leg.transform.rotation, Quaternion.LookRotation (direction), Leg_Speed_Rotation);
			}

			if (CanMove) {
				Move (Leg_Speed_Walk);
			} else if(!UsingAbility) {
				Idle ();
			}

			if (KnifeCharge) {
				Move (Speed_KnifeCharge);
			}

			if (MeleeMode) {
	
			}

		} else {
			Idle ();
		}
	}

	public void ChooseMode()
	{
		int i = Random.Range(0, 2);
		switch (i) {
		case 0:
			Melee ();
			break;
		case 1:
			Gun();
			break;
		}
	}

	public void Melee()
	{
		MeleeMode = true;
		GunMode = false;
		BodyAnim.SetBool ("KnifeCharge", true);
	}

	public void Gun()
	{
		MeleeMode = false;
		GunMode = true;
		BodyAnim.SetBool ("KnifeCharge", false);
	}

	public void ChooseCooldown()
	{
		CanSwitchMode = true;
	}

	public void Move(float Speed)
	{
		LegAnim.SetBool ("Move", true);
		LegAnim.SetBool ("Idle", false);
		if (Vector3.Distance (Target.position, Leg.transform.position) > 1.5f) {
			Leg.transform.Translate (0, 0, Speed * Time.deltaTime);
		} else {
			Idle ();
		}
	}

	public void Idle()
	{
		LegAnim.SetBool ("Move", false);
		LegAnim.SetBool ("Idle", true);
	}

	public void knifeChargeStart()
	{
		CanMove = false;
	}

	public void KnifeChargeing()
	{
		UsingAbility = true;
		KnifeCharge = true;
	}

	public void KnifeChargeEnd()
	{
		UsingAbility = false;
		KnifeCharge = false;
	}
}
