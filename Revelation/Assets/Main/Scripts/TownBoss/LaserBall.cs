using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBall : MonoBehaviour {

	public bool ScaleOn;

	public float StartScale;
	public float MaxScale;
	public float MinimunScale;

	public int PlusMutiple;
	public int MinusMutiple;

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable()
	{
		transform.localScale = new Vector3 (StartScale, StartScale, StartScale);
	}

	// Update is called once per frame
	void Update () {
		if (ScaleOn) {
			if (transform.localScale.x <= MaxScale && transform.localScale.y <= MaxScale && transform.localScale.z <= MaxScale) {
				transform.localScale += new Vector3 (0.15f, 0.15f, 0.15f) * Time.deltaTime * PlusMutiple;
			}
		} else {
			if (transform.localScale.x > MinimunScale && transform.localScale.y > MinimunScale && transform.localScale.z > MinimunScale) {
				transform.localScale -= new Vector3 (0.15f, 0.15f, 0.15f) * Time.deltaTime * MinusMutiple;;
			} else {
				gameObject.SetActive (false);
			}
		}
	}
}
