using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	public float x;
	public float y;
	public float z;
	public float time;
	public float delay;

	public Quaternion rot;

	public bool BackToOriginRot;
	public float Backdelay;

	void Start()
	{
		rot = transform.rotation;
	}

	public void ITweenRotate()
	{
		Hashtable args = new Hashtable();

		args.Add("rotation", new Vector3(x, y, z));

		args.Add("x", x);
		args.Add("y", y);
		args.Add("z", z);
		args.Add("time", time);
		args.Add("delay", delay);
		args.Add ("easeType", iTween.EaseType.linear);
		if (BackToOriginRot) {
			args.Add ("oncomplete", "complete");
			args.Add ("oncompleteparams", "end");
			args.Add ("oncompletetarget", this.gameObject);
		}
		iTween.RotateAdd(this.gameObject, args);
	}


	void complete()
	{
		Invoke ("backtooriginrot", Backdelay);
	}

	void backtooriginrot()
	{
		transform.rotation = rot;
	}
}
