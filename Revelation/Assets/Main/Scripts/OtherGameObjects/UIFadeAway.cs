using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeAway : MonoBehaviour {

	public float fadeDelay;
	public bool CanStartFade = false;
	public float fadeValue;
	CanvasGroup canvasgroup;

	// Use this for initialization
	void Start () {
		Invoke ("uiFadeAway", fadeDelay);
		canvasgroup = GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (CanStartFade) {
			canvasgroup.alpha -= fadeValue * Time.deltaTime;
			if (canvasgroup.alpha == 0) {
				Destroy (gameObject);
			}
		}
	}

	public void uiFadeAway()
	{
		CanStartFade = true;
	}
}
