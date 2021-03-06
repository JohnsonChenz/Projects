using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterInput : MonoBehaviour {

	public CharaterStatus charaterStatus;

	public bool debugAiming;
	public bool isAiming;

	public Weapon weapon;
	public Transform targetLook;
	public bool opportunityToAim;
	public float distance;

	void Start()
	{
		targetLook = weapon.targetLook;
	}


	public void InputUpdate()
	{
		RayCastAiming ();

		if (Input.GetMouseButton (1) && opportunityToAim) {
			charaterStatus.isAiming = true;
		}

		if (Input.GetMouseButton (1) && !opportunityToAim) {
			charaterStatus.isAiming = false;
		}

		if (!Input.GetMouseButton (1)) {
			charaterStatus.isAiming = false;
		}

		//if (!debugAiming)
			//charaterStatus.isAiming = Input.GetMouseButton (1);
		//else
			//charaterStatus.isAiming = isAiming;


		if (Input.GetMouseButtonDown (0) && opportunityToAim) {
			weapon.Shoot ();
		}
	}

	public void RayCastAiming()
	{
		Debug.DrawLine (transform.position + transform.up * 1.4f, targetLook.position, Color.green);

		distance = Vector3.Distance (transform.position + transform.up * 1.4f, targetLook.position);

		if (distance > 1.5f) {
			opportunityToAim = true;

		} 
		else 
		{
			opportunityToAim = false;
		}
	}
}
