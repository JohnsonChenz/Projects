using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjHit : MonoBehaviour {


	public float health;

	public GameObject[] TriggerObjs;

	public ParticleSystem ExplodeParticle;

	// Use this for initialization
	void Start () {
		
	}

	public void Damage(float damage)
	{
		if (health < 1) {
			Death ();
		} else {
			health -= damage;
		}
	}

	public void Death()
	{
		if (ExplodeParticle) {
			ExplodeParticle.Play ();
		}

		if (this.GetComponent<InteractObjects> ()) {
			this.GetComponent<InteractObjects> ().NextObjects ();
		}

		for (int i = 0; i < TriggerObjs.Length; i++) {
			TriggerObjs [i].SetActive (true);
		}
		gameObject.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
