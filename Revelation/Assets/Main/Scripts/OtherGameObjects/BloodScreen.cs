using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodScreen : MonoBehaviour {

	Image image;
	public float Amount;
	public float FadeSpeed;
	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
	}

	public void GiveBloodScreen(float amount)
	{
		Amount = amount;
	}
	// Update is called once per frame
	void Update () {
		if (Amount > 0) {
		    Amount -= Time.deltaTime * FadeSpeed;
			image.color = new Color (image.color.r, image.color.g, image.color.b, Amount);
		} else if(Amount < 0){
			Amount = 0;
			image.color = new Color (image.color.r, image.color.g, image.color.b, Amount);
		}
	}
}
