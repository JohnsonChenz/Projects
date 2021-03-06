using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHander : MonoBehaviour {

	public Transform camTrans;
	public Transform pivot;
	public Transform MainCharacter;
	public Transform mTransform;


	public CharaterStatus charaterStatus;
	public CharaterInventory charaterinventory;
	public CameraConfig cameraConfig;
	public bool leftPivot;
	public float delta;

	public Transform targetLook;

	public float mouseX;
	public float mouseY;
	public float smoothX;
	public float smoothY;
	public float smoothXVlocity;
	public float smoothYVlocity;
	public float lookAngle;
	public float titlAngle;

    public float recoil;

	public LayerMask CameraCollisionMask;
	public LayerMask HitAbleLayer;

	public bool CanRotate = true;

	Vector3 OriginPos;
	public float FollowSpeed = 1;

	public float targetX;
	public float targetY;
	public float targetZ;

    public void Recoil(float amount)
    {
        recoil += amount;
    }

	void Start()
	{
		lookAngle = 174f;
		OriginPos = new Vector3(camTrans.localPosition.x,camTrans.localPosition.y,camTrans.localPosition.z);
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		charaterinventory = MainCharacter.GetComponent<CharaterInventory> ();

	}
	void Update()
	{
		Tick ();


        if(recoil > 0f)
        {
            recoil -= Time.deltaTime * 10;
        }
		else if(recoil < 0f)
        {
            recoil = 0;
        }

		//RaycastHit hit;
		//Debug.DrawLine (mTransform.position, Charater.position, Color.green);

	}


	void Tick()
	{

		//float targetY = cameraConfig.normalY;

		delta = Time.deltaTime;

		HP ();
		if (!charaterinventory.CanvasInventory.GetComponent<Canvas>().enabled && CanRotate) {
			HR ();
		}
		Vector3 targetPosition = Vector3.Lerp (mTransform.position , MainCharacter.position, FollowSpeed);
		mTransform.position = targetPosition;

		TargetLook ();

		RaycastHit hit;
		if (Physics.Linecast (pivot.position, camTrans.position, out hit, CameraCollisionMask)) {
			//Vector3 hitPoint = new Vector3 (hit.point.x + hit.normal.x * .1f, hit.point.y + hit.normal.y * .1f, hit.point.z + hit.normal.z * .1f);
			camTrans.position = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
		} 


		//Debug.DrawLine (camTrans.position, pivot.position, Color.red);

	}

	void TargetLook()
	{
		//Ray ray = new Ray (camTrans.position, camTrans.forward * 500);
		RaycastHit hit;


		if(Physics.Raycast(camTrans.position,camTrans.forward, out hit, 500f , HitAbleLayer))
		{
			Debug.DrawLine (camTrans.position, hit.point, Color.green);
			targetLook.position = hit.point;
		}
		else
		{
			targetLook.position = Vector3.Lerp (targetLook.position, targetLook.transform.forward * 200, Time.deltaTime * 50);
		}
	}

	void HP()
	{
		targetX = cameraConfig.normalX;
		targetY = cameraConfig.normalY;
		targetZ = cameraConfig.normalZ;

		if (charaterinventory.Weaponcount == 3) {
			targetZ = cameraConfig.MeleeZ;
			targetX = cameraConfig.MeleeX;
			targetY = cameraConfig.MeleeY;
		}

		if (charaterStatus.isAiming) 
		{
			if (charaterinventory.activeWeapon.isSnipeWeapon) {
				targetZ = cameraConfig.aimSnipeZ;
				targetX = cameraConfig.aimSnipeX;
				targetY = cameraConfig.aimSnipeY;
			} else {
				targetX = cameraConfig.aimX;
				targetY = cameraConfig.aimY;
				targetZ = cameraConfig.aimZ;
			}
		}

		if (charaterinventory.CanvasInventory.GetComponent<Canvas> ().enabled) {
			targetZ = cameraConfig.InventoryZ;
			targetX = cameraConfig.InventoryX;
			targetY = cameraConfig.InventoryY;
		}

		if(leftPivot)
		{

			targetX = -targetX;
		}

		Vector3 newPivotPosition = pivot.localPosition;

		newPivotPosition.x = targetX;
		newPivotPosition.y = targetY;

		Vector3 newCameraPosition = camTrans.localPosition;

		newCameraPosition.x = OriginPos.x;
		newCameraPosition.y = OriginPos.y;
		newCameraPosition.z = targetZ;

		float t = delta * cameraConfig.pivotSpeed;
		pivot.localPosition = Vector3.Lerp (pivot.localPosition, newPivotPosition, t);
		camTrans.localPosition = Vector3.Lerp (camTrans.localPosition, newCameraPosition, t);

	}

	void HR()
	{
		mouseX = Input.GetAxis ("Mouse X");
		mouseY = Input.GetAxis ("Mouse Y");

		if (cameraConfig.turnSmooth > 0) {
			smoothX = Mathf.SmoothDamp (smoothX, mouseX, ref smoothXVlocity, cameraConfig.turnSmooth);
			smoothY = Mathf.SmoothDamp (smoothY, mouseY, ref smoothXVlocity, cameraConfig.turnSmooth);
		} 
		else 
		{
			smoothX = mouseX;
			smoothY = mouseY;
		}
			

		lookAngle += smoothX * cameraConfig.Y_rot_speed;
		Quaternion targetRot = Quaternion.Euler (0, lookAngle, 0);
		mTransform.rotation = targetRot;

		titlAngle -= smoothY * cameraConfig.Y_rot_speed;
		titlAngle = Mathf.Clamp (titlAngle, cameraConfig.minAngle, cameraConfig.maxAngle);
		pivot.localRotation = Quaternion.Euler (titlAngle + -recoil, 0, 0);
	}

}
