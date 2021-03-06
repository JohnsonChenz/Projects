using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerashake : MonoBehaviour {

	public float delay;
	public float x;
	public float y;
	public float z;
	public float time;

	public bool NotAuto;
	public GameObject Camtarget;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable()
	{
		if (!NotAuto) {
			Invoke ("ITweenShake", delay);
		}
	}

	public void ITweenShake()
	{
		Hashtable args = new Hashtable();

		args.Add("amount", new Vector3(x, y, z));
;

		args.Add("orienttopath", true);

		args.Add ("easetype", iTween.EaseType.linear);
		args.Add("time", time);

	
		iTween.ShakePosition(this.gameObject, args);

	}

	public void ITweenShaketarget()
	{
		Hashtable args = new Hashtable();

		args.Add("amount", new Vector3(x, y, z));

		args.Add("orienttopath", false);
		args.Add ("easetype", iTween.EaseType.linear);
		args.Add("time", time);


		iTween.ShakePosition(Camtarget, args);

	}
}
