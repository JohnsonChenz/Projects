using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour {

	public Transform Player;
	public LayerMask layer;
	public ActorEvent actorevent;
	public GameObject[] meatHitEffect;
	public GameObject metalHitEffect;
	public GameObject Cam;
	public float hitdistance;
	public float hitradius;
	public float damage;

	public Vector3 p1;
	public Vector3 p2;

	private float currenthitdistance;
	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		actorevent = Player.GetComponent<ActorEvent> ();
	}

	void OnTriggerEnter(Collider other)
	{


		if (other.transform.GetComponent<HitPosition> () && actorevent.WeaponControll == true) {

		}
	}

	void Update()
	{
		RaycastHit hit;
		p1 = transform.position;
		p2 = transform.position + transform.forward * hitdistance;

		Debug.DrawLine (p1, p2, Color.green);
		if(Physics.CapsuleCast (p1,p2,hitradius,transform.up,out hit,hitdistance,layer) && actorevent.WeaponControll == true)
		{
			if (hit.transform.GetComponent<HitPosition> ()) {
				hit.transform.GetComponent<HitPosition> ().Damage (damage);
				Cam.GetComponent<CameraShake> ().shakeMagnitude = 0.02f;
				Cam.GetComponent<CameraShake> ().shakeTime = 0.2f;
				Cam.GetComponent<CameraShake> ().ShakeIt ();
				string meterialName = hit.collider.sharedMaterial.name;
				switch (meterialName) 
				{
				case "Meat":
					SpawnDecal (hit, meatHitEffect [Random.Range (0, meatHitEffect.Length)]);
					break;
				case "Metal":
					if (metalHitEffect) {
						SpawnDecal (hit, metalHitEffect);
					}
					break;
				}

				Player.GetComponent<ybotDamage>().ap.fillAmount += 0.01f;
				hit.transform.GetComponent<Rigidbody> ().AddForceAtPosition (hit.normal * 50, hit.point);
				GetComponent<AudioController> ().PlaySounds_1 ();
			}
			Debug.Log ("hit");
		}
       

		/*
		if (Physics.SphereCast (transform.position, hitradius, transform.forward, out hit, hitdistance, layer, QueryTriggerInteraction.UseGlobal) && actorevent.WeaponControll == true) {
			hit.transform.GetComponent<HitPosition> ().Damage (damage);
			Cam.GetComponent<CameraShake> ().shakeMagnitude = 0.02f;
			Cam.GetComponent<CameraShake> ().shakeTime = 0.2f;
			Cam.GetComponent<CameraShake> ().ShakeIt ();
			SpawnDecal (hit, meatHitEffect [Random.Range (0, meatHitEffect.Length)]);
			hit.transform.GetComponent<Rigidbody> ().AddForce (-hit.transform.forward * 50);
			currenthitdistance = hit.distance;
		} else {
			currenthitdistance = hitdistance;
		}
		*/
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

	void SpawnDecal(RaycastHit hit, GameObject prefab)
	{
		GameObject spawnDecal = GameObject.Instantiate (prefab, hit.point, Quaternion.LookRotation (hit.normal));
		spawnDecal.transform.SetParent (hit.collider.transform);
		Destroy (spawnDecal.gameObject, 10);
		//hit.transform.GetComponent<Rigidbody> ().AddForce (transform.forward * 100);
	}

	// Update is called once per frame

}
