using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour {

	public float x;
	public float y;
	public float z;
	public float time;
	public float delay;

	public bool BackToOriginPos;

	public Vector3 pos;
	public float Backdelay;

	public bool test;
	void Start()
	{
		pos = transform.position;
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.F5) && test) {
			ITweenTranslate ();
		}
	}
	public void ITweenTranslate()
	{
		Hashtable args = new Hashtable();


		args.Add("amount", new Vector3(x, y, z));
		args.Add("easetype", iTween.EaseType.linear);
		args.Add("time", time);
		args.Add("delay", delay);
		if (BackToOriginPos) {
			args.Add ("oncomplete", "complete");
			args.Add ("oncompleteparams", "end");
			args.Add ("oncompletetarget", this.gameObject);
		}
		iTween.MoveAdd (this.gameObject, args);
	}

	void complete()
	{
		Invoke ("backtooriginpos", Backdelay);
	}

	void backtooriginpos()
	{
		transform.position = pos;
	}
}
