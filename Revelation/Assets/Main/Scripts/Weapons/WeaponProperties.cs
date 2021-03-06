using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Weapon/properties")]
public class WeaponProperties : ScriptableObject {

	public Vector3 rHandPos;
	public Vector3 rHandRot;

	public Vector3 BackPos;
	public Vector3 BackRot;

    public RuntimeAnimatorController RightHandAnim;
	//public GameObject weaponPrefab;
	//public GameObject itemPrefab;

	//public Vector3 WeaponPos;
	//public Vector3 WeaponRot;
}
