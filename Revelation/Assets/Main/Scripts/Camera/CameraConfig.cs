using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Camera/Config")]
public class CameraConfig : ScriptableObject {

	public float turnSmooth;
	public float pivotSpeed;
	public float Y_rot_speed;
	public float X_rot_speed;
	public float minAngle;
	public float maxAngle;
	public float normalZ;
	public float normalX;
	public float aimZ;
	public float aimSnipeZ;
	public float aimX;
	public float aimSnipeX;
	public float aimY;
	public float aimSnipeY;
	public float normalY;
	public float MeleeX;
	public float MeleeY;
	public float MeleeZ;
	public float InventoryX;
	public float InventoryY;
	public float InventoryZ;
}
