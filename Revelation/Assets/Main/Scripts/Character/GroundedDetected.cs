using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedDetected : MonoBehaviour {

	public LayerMask layer;
	public float hitdistance;
	public float hitradius;

	public float currenthitdistance;

	public bool ishit;

	public Vector3 p2;

	//public Transform transform;

	public Vector3 Origin;
	public Vector3 Direction;
	public CharaterStatus charaterstatus;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		RaycastHit hit;
		Origin = transform.position;
		Direction = transform.up;

		p2 = Origin + Direction * currenthitdistance;

		if (Physics.SphereCast (Origin, hitradius, Direction, out hit, hitdistance, layer, QueryTriggerInteraction.UseGlobal)) {
			charaterstatus.isGround = true;
			ishit = true;
			currenthitdistance = hit.distance;
		} else {
			charaterstatus.isGround = false;
			ishit = false;
			currenthitdistance = hitdistance;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Debug.DrawLine (Origin, p2);
		Gizmos.DrawWireSphere (p2, hitradius);
	}
}
