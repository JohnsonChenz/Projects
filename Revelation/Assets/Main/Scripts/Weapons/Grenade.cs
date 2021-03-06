using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

	public float delay = 3f;
	public float radius = 5f;
	public float force = 700f;
	public float damage = 20;
	public float Speed;
	float countdown;

	bool hasExploded = false;

	public LayerMask layers;

	Vector3 lastpos;
	//public ParticleSystem explosionEffect;
	public GameObject explosionEffect;

	Vector3 forces = new Vector3(10.0f, 0.0f, 0.0f);


	// Use this for initialization
	void Start () {
		//lastpos = transform.position;
		//countdown = delay;
		//transform.GetComponent<Rigidbody> ().AddForce (transform.forward * Speed);
		//transform.GetComponent<Rigidbody> ().AddForce (transform.up * Speed / 8);
		//lastpos = transform.position;

	}

	void OnEnable()
	{
		//GetComponent<Rigidbody> ().velocity = Vector3.zero;
		//GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		hasExploded = false;
		countdown = delay;
		//transform.GetComponent<Rigidbody> ().AddForce (transform.forward * Speed);
		//transform.GetComponent<Rigidbody> ().AddForce (transform.up * Speed / 8);
		lastpos = transform.position;
	}
	void OnDisable()
	{
		
	}
	// Update is called once per frame
	void Update () {

		if (!gameObject.activeInHierarchy)
			return;
		
		transform.Translate (Vector3.forward * Speed * Time.deltaTime);

		RaycastHit hit;

		countdown -= Time.deltaTime;

		if (Physics.Linecast (lastpos, transform.position, out hit, layers)) {
			Explode ();
		}

		if (countdown <= 0f && !hasExploded) {
			Explode ();
			hasExploded = true;
		}
		lastpos = transform.position;
	}

	void Explode()
	{
		Instantiate (explosionEffect, transform.position, transform.rotation);
		Collider[] colliders = Physics.OverlapSphere (transform.position, radius);

		foreach(Collider nearbyObject in colliders)
		{

				EnemyStat enemystat = nearbyObject.GetComponent<EnemyStat> ();
			HitPosition hitposition = nearbyObject.GetComponent<HitPosition> ();
			    TownBoss townboss = nearbyObject.GetComponent<TownBoss> ();
			    ObjHit objhit = nearbyObject.GetComponent<ObjHit> ();


				if (enemystat != null) {
					enemystat.TakeAwayHealth (damage);
				}

			if (townboss != null) {
				townboss.TakeAwayHealth (damage);
			}

			if (hitposition != null) {
				if (hitposition.pipehp != null) {
					hitposition.Damage (damage * 0.05f);
				}
			}

			if (objhit != null) {
				objhit.Damage (damage);
			}
				Rigidbody rb = nearbyObject.GetComponent<Rigidbody> ();

				if (rb != null) {
				//rb.AddForce(forces, ForceMode.Impulse);
				rb.AddExplosionForce (force * 5f, transform.position, radius);
				rb.AddForce (transform.up * force);
				}
		}

		GameObject.Find ("BulletPool").GetComponent<ExplodeBulletPool> ().RecoveryExplodeBullet (gameObject);
	}
}
