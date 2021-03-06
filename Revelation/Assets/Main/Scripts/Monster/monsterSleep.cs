using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class monsterSleep : MonoBehaviour {

	Animator anim;
	public float DetectedDistance = 5f;
	public Transform player;
	public AI2 AI2;
	NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		anim = GetComponent<Animator> ();
		anim.speed = 0f;
		AI2.enemystat.CantDmg = true;
		agent = GetComponent<NavMeshAgent> ();
		agent.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 direction = player.position - this.transform.position;
		float angle = Vector3.Angle (direction, this.transform.forward);
		direction.y = 0;

		if ((Vector3.Distance (player.position, this.transform.position) < DetectedDistance) && angle < 30) {
			anim.speed = 1f;
			Invoke ("getUp", 9f);



		}
	}


	void getUp(){
		anim.SetBool ("wake up", true);
		AI2.enabled = true;
		AI2.enemystat.CantDmg = false;
		agent.enabled = true;
	}

}
