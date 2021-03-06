using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botton : MonoBehaviour {

	public bool isTriggered;

	// Use this for initialization
	void Start () {
		isTriggered = false;
	}

	public void Press()
	{
		if (!isTriggered) {
			isTriggered = true;
			float delay = GetComponent<InteractObjects> ().InteractDelay;
			if(delay > 0)
			{
				this.GetComponent<InteractObjects> ().Invoke ("NextObjects", delay);
			}
			else
			{
				this.GetComponent<InteractObjects> ().NextObjects();
			}
			this.GetComponent<InteractObjects> ().Enable = false;
		}
	}

}
