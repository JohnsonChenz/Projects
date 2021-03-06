using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlliedShooterBullet : MonoBehaviour {


	public int Speed;
	Vector3 lastpos;
	//public GameObject decal;

	public LayerMask enemyBodyPartLayer;
	public LayerMask MapLayer;
	public LayerMask ObjLayer;

	public GameObject metalHitEffect;
	public GameObject sandHitEffect;
	public GameObject stoneHitEffect;
	public GameObject[] meatHitEffect;
	public GameObject woodHitEffect;

	public float damage;

	// Use this for initialization
	void Start () {
		lastpos = transform.position;
		//Destroy (gameObject, 10);
	}

	void OnEnable()
	{
		lastpos = transform.position;
		Invoke ("Recovery", 2.5f);
	}
	// Update is called once per frame
	void Update () {
		if (!gameObject.activeInHierarchy)
			return;
		transform.Translate (Vector3.forward * Speed * Time.deltaTime);

		RaycastHit hit;

		//Debug.DrawLine (lastpos, transform.position);

		if (Physics.Linecast (lastpos, transform.position, out hit, enemyBodyPartLayer)) {


			string meterialName = hit.collider.sharedMaterial.name;

			switch (meterialName) 
			{
			case "Meat":
				SpawnDecal (hit, meatHitEffect [Random.Range (0, meatHitEffect.Length)]);
				Meat (hit);
				break;
			case "Metal":
				SpawnDecal (hit, metalHitEffect);
				Metal (hit);
				break;
			}

			GameObject.Find ("BulletPool").GetComponent<AlliedShooterBulletPool> ().RecoveryBullet (gameObject);
			//Destroy (gameObject);
		}
			
		if (Physics.Linecast (lastpos, transform.position, out hit, ObjLayer)) {


			if (hit.collider.sharedMaterial == null) {
				SpawnDecal (hit, metalHitEffect);
				GameObject.Find ("BulletPool").GetComponent<AlliedShooterBulletPool> ().RecoveryBullet (gameObject);
				hit.transform.GetComponent<ObjHit> ().Damage (damage);
				return;
			}

			/*
			string meterialName = hit.collider.sharedMaterial.name;

			switch (meterialName) 
			{
			case "Metal":
				SpawnDecal (hit, metalHitEffect);
				break;
			case "Sand":
				SpawnDecal (hit, sandHitEffect);
				break;
			case "Stone":
				SpawnDecal (hit, stoneHitEffect);
				break;
			case "Wood":
				SpawnDecal (hit, woodHitEffect);
				break;
			}
			*/
			SpawnDecal (hit, metalHitEffect);
			GameObject.Find ("BulletPool").GetComponent<AlliedShooterBulletPool> ().RecoveryBullet (gameObject);
		}

		if (Physics.Linecast (lastpos, transform.position, out hit, MapLayer)) {

			if (hit.transform.gameObject.CompareTag("TasksObject")) {
				return;
			}

			if (hit.collider.sharedMaterial == null) {
				SpawnDecal (hit, metalHitEffect);
				GameObject.Find ("BulletPool").GetComponent<AlliedShooterBulletPool> ().RecoveryBullet (gameObject);
				//Destroy (gameObject);
				return;
			}

			/*
			string meterialName = hit.collider.sharedMaterial.name;

			switch (meterialName) 
			{
			case "Metal":
				SpawnDecal (hit, metalHitEffect);
				break;
			case "Sand":
				SpawnDecal (hit, sandHitEffect);
				break;
			case "Stone":
				SpawnDecal (hit, stoneHitEffect);
				break;
			case "Wood":
				SpawnDecal (hit, woodHitEffect);
				break;
			}
			*/
			SpawnDecal (hit, metalHitEffect);
			GameObject.Find ("BulletPool").GetComponent<AlliedShooterBulletPool> ().RecoveryBullet (gameObject);
			//Destroy (gameObject);


			//print (hit.transform.name);
			//GameObject d = Instantiate<GameObject> (decal);
			//d.transform.position = hit.point + hit.normal * 0.001f;
			//d.transform.rotation = Quaternion.LookRotation (-hit.normal);
			//Destroy (d, 1);
		}
        
		lastpos = transform.position;
	}

	public void Recovery()
	{
		GameObject.Find ("BulletPool").GetComponent<AlliedShooterBulletPool> ().RecoveryBullet (gameObject);
	}
	public void Meat(RaycastHit hit)
	{
		if(hit.transform.GetComponent<HitPosition>() != null)
			{
			hit.transform.GetComponent<HitPosition>().Damage(damage);
			if (hit.transform.GetComponent<HitPosition> ().enemystat) {
				if (hit.transform.GetComponent<HitPosition> ().enemystat.gameObject.GetComponent<AudioController> ()) {
					hit.transform.GetComponent<HitPosition> ().enemystat.gameObject.GetComponent<AudioController> ().PlaySounds_4_NoRandomAndCd ();
				}
			}
			if (hit.transform.GetComponent<HitPosition> ().GetComponent<Rigidbody>()) {
			hit.transform.GetComponent<Rigidbody> ().AddForce (transform.forward * 50);
			}
			}
	}

	public void Metal(RaycastHit hit)
	{
		if(hit.transform.GetComponent<HitPosition>() != null)
		{
			hit.transform.GetComponent<HitPosition>().Damage(damage);
			if (hit.transform.GetComponent<HitPosition> ().enemystat) {
				//hit.transform.GetComponent<HitPosition> ().enemystat.gameObject.GetComponent<AudioController> ().PlaySounds_4_NoRandomAndCd ();
			}
			if (hit.transform.GetComponent<HitPosition> ().GetComponent<Rigidbody>()) {
				hit.transform.GetComponent<Rigidbody> ().AddForce (transform.forward * 50);
			}
		}
	}

	void SpawnDecal(RaycastHit hit, GameObject prefab)
	{
		GameObject spawnDecal = GameObject.Instantiate (prefab, hit.point, Quaternion.LookRotation (hit.normal));
		spawnDecal.transform.SetParent (hit.collider.transform);
		Destroy (spawnDecal.gameObject, 10);
		//hit.transform.GetComponent<Rigidbody> ().AddForce (transform.forward * 100);
	}
		
}
