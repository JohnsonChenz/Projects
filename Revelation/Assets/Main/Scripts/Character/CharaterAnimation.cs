using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterAnimation : MonoBehaviour {


	public Animator anim;
	public CharaterMoveCamController charaterMovement;
	public CharaterStatus charaterStatus;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void AnimationUpdate () {
		anim.SetBool ("sprint", charaterStatus.isSprint);
		anim.SetBool ("aiming", charaterStatus.isAiming);

		if (!charaterStatus.isAiming)
			AnimationNormal ();
		else
			AnimationAiming ();
	}

	void AnimationNormal()
	{
		//anim.SetFloat ("vertical",charaterMovement.moveAmount, 0.15f, Time.deltaTime);
	}


	void AnimationAiming()
	{
		float v = charaterMovement.vertical;
		float h = charaterMovement.horizontal;

		anim.SetFloat ("vertical", v, 0.15f, Time.deltaTime);
		anim.SetFloat ("horizontal", h , 0.15f, Time.deltaTime);
	}
}
