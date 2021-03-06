using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleProgress : MonoBehaviour {

	float Sec;
	Image img;
	// Use this for initialization
	void Start () {
		img = this.GetComponent<Image> ();
		img.color = new Color (0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Sec > 0) {
			if (img.fillAmount < 1) {
				img.fillAmount += (Time.deltaTime / Sec);
			} else {
				img.color = new Color (0, 0, 0, 0);
			}
		}
	}

	public void StartCircleProgress(string sec)
	{
		Sec = float.Parse(sec);
		img.color = new Color (255, 255, 255, 255);
		img.fillAmount = 0;
	}
}
