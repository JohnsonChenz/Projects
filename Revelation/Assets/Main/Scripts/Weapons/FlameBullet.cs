using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBullet : MonoBehaviour {


	public float damage = 20;
	public float burntime = 20;
	public int RecoverTime = 2;
	// Use this for initialization
	void Start () {
		
	}

	void OnEnable()
	{
		GetComponent<ParticleSystem> ().time = 0;
		GetComponent<ParticleSystem> ().Play();
		Invoke ("Recovery", RecoverTime);
	}

	void Recovery()
	{
		GameObject.Find ("BulletPool").GetComponent<FlameBulletPool> ().RecoveryFlameBullet (gameObject);
	}
	void OnParticleCollision(GameObject other)
	{
		EnemyStat enemystat = other.GetComponent<EnemyStat> ();
		HitPosition hitposition = other.GetComponent<HitPosition> ();

		print (other.name);

		if (enemystat != null) {
			enemystat.TakeAwayHealthBurn (damage, burntime);
		}

		if (hitposition != null) {
			hitposition.TakeAwayHealthBurn (damage, burntime);
		}
	}

}
