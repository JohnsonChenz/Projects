using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarFixer : MonoBehaviour {

	public Transform ScrollView;

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable()
	{
		ScrollView.GetComponent<ScrollRect> ().content = this.GetComponent<RectTransform> ();
	}
	// Update is called once per frame
	void Update () {
		
	}
}
