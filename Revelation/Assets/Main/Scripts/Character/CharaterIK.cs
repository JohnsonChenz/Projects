using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterIK : MonoBehaviour {

	public Animator anim;


	public CharaterInventory charaterInventory;
	public CharaterStatus charaterStatus;

	public bool interactTarget;

	public Transform targetLook;

	public Transform l_Hand;
	public Transform l_Hand_Target;
	public Transform r_Hand;
	public Transform r_Hand_Target;

	public Quaternion lh_rot;


	public float rh_Weight;
	public float lh_Weight;
	public float lk_Weight;

	public float bd_Weight;
	public float hd_Weight;

	public float rh_WeightAddMtp;
	public float lh_WeightAddMtp;

	public float rh_WeightMinusMtp;
	public float lh_WeightMinusMtp;

	public Transform shoulder;
	public Transform aimPivot;

	public Transform LookAtTarget;

    public RuntimeAnimatorController RightHandAnim;

	public Transform Camera;
	public float Angle;
	public float angle;
	public float lkk_Weight;
	// Use this for initialization
	void Start () {

		Camera = GameObject.Find ("Camera").transform;
		charaterInventory = GetComponent<CharaterInventory> ();
		anim = GetComponent<Animator> ();
		shoulder = anim.GetBoneTransform (HumanBodyBones.RightShoulder).transform;


		aimPivot = new GameObject ().transform;
		aimPivot.name = "aim pivot";
		aimPivot.transform.parent = transform;

		r_Hand = new GameObject ().transform;
		r_Hand.name = "right hand";
		r_Hand.transform.parent = aimPivot;
        r_Hand.gameObject.AddComponent<Animator>();
        r_Hand.gameObject.GetComponent<Animator>().runtimeAnimatorController = RightHandAnim;



        l_Hand = new GameObject ().transform;
		l_Hand.name = "left hand";
		l_Hand.transform.parent = aimPivot;

	}
	
	// Update is called once per frame
	void Update () {


		//iTween.MoveUpdate (l_Hand.gameObject, l_Hand_Target.position, 0.3f);
		if (charaterInventory.WeaponAvailable && lh_Weight > 0) {
			lh_rot = l_Hand_Target.rotation;
			l_Hand.position = l_Hand_Target.position;
		}



		if (charaterStatus.isAiming) {
			rh_WeightAddMtp = 2f;
			rh_WeightMinusMtp = 10f;
			rh_Weight += Time.deltaTime * rh_WeightAddMtp;
		} 
		else if (interactTarget)
		{
			rh_WeightAddMtp = 0.5f;
			rh_WeightMinusMtp = 0.5f;
			r_Hand.rotation = r_Hand_Target.rotation;
			r_Hand.position = r_Hand_Target.position;
			rh_Weight += Time.deltaTime * rh_WeightAddMtp;


			//lh_Weight -= Time.deltaTime * 30;
		}
		else
		{
			//r_Hand_Target = null;
			rh_Weight -= Time.deltaTime * rh_WeightMinusMtp;
		}

		rh_Weight = Mathf.Clamp (rh_Weight, 0, 1);

	}
	void OnAnimatorIK()
	{
		Vector3 direction = Camera.position - this.transform.position;
		direction.y = 0;
		angle = Vector3.Angle (direction, this.transform.forward);

		aimPivot.position = shoulder.position;

		if (charaterStatus.isAiming) {
			aimPivot.LookAt (targetLook);

			//lk_Weight = .3f;
			anim.SetLookAtWeight (lk_Weight, .3f, .3f);

			anim.SetLookAtPosition (targetLook.position);

			anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, lh_Weight);
			anim.SetIKRotationWeight (AvatarIKGoal.LeftHand, lh_Weight);
			anim.SetIKPosition (AvatarIKGoal.LeftHand, l_Hand.position);
			anim.SetIKRotation (AvatarIKGoal.LeftHand, lh_rot);

			anim.SetIKPositionWeight (AvatarIKGoal.RightHand, rh_Weight);
			anim.SetIKRotationWeight (AvatarIKGoal.RightHand, rh_Weight);
			anim.SetIKPosition (AvatarIKGoal.RightHand, r_Hand.position);
			anim.SetIKRotation (AvatarIKGoal.RightHand, r_Hand.rotation);
		} 
		else 
		{

			//aimPivot.LookAt (targetLook);
			if (angle > Angle) {
				lkk_Weight += Time.deltaTime * 0.3f;
				anim.SetLookAtWeight (lkk_Weight, 0.3f, 1.1f, 0);
			}
			else if (angle <= Angle) {
				lkk_Weight -= Time.deltaTime * 0.3f;
				anim.SetLookAtWeight (lkk_Weight, 0.3f, 1.1f, 0);
			}
			anim.SetLookAtPosition (targetLook.position);

			anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, lh_Weight);
			anim.SetIKRotationWeight (AvatarIKGoal.LeftHand, lh_Weight);
			anim.SetIKPosition (AvatarIKGoal.LeftHand, l_Hand.position);
			anim.SetIKRotation (AvatarIKGoal.LeftHand, lh_rot);

			anim.SetIKPositionWeight (AvatarIKGoal.RightHand, rh_Weight);
			anim.SetIKRotationWeight (AvatarIKGoal.RightHand, rh_Weight);
			anim.SetIKPosition (AvatarIKGoal.RightHand, r_Hand.position);
			anim.SetIKRotation (AvatarIKGoal.RightHand, r_Hand.rotation);
		}

		lkk_Weight = Mathf.Clamp (lkk_Weight, 0, 0.3f);


	}
}
