using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeToBlack : MonoBehaviour {

	Image image;

	public bool FadeToBlack;
	public bool FadeToZero;

	public float Amount;
	public float FadeSpeed;
	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
		Invoke ("Zero", 2f);
		image.color = new Color (image.color.r, image.color.g, image.color.b, 1);
	}
	public void ZeroSlow()
	{
		Invoke ("Zero", 2f);
		image.color = new Color (image.color.r, image.color.g, image.color.b, 1);
	}

	public void Black()
	{
		Amount = 0;
		FadeToBlack = true;
		FadeToZero = false;
	}

	public void Zero()
	{
		Amount = 1;
		FadeToBlack = false;
		FadeToZero = true;
	}
	// Update is called once per frame
	void Update () {

		/*
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			Black ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			Invoke ("Zero", 1f);
			image.color = new Color (image.color.r, image.color.g, image.color.b, 1);
		}
        */
		if (FadeToBlack) {
			if (Amount < 1) {
				Amount += Time.deltaTime * FadeSpeed;
				image.color = new Color (image.color.r, image.color.g, image.color.b, Amount);
			} else if (Amount > 0) {
				FadeToBlack = false;
				Amount = 1;
				image.color = new Color (image.color.r, image.color.g, image.color.b, Amount);
			}
			if (FadeToZero) {
				FadeToZero = false;
			}
		} else if (FadeToZero) {
			if (Amount > 0) {
				Amount -= Time.deltaTime * FadeSpeed;
				image.color = new Color (image.color.r, image.color.g, image.color.b, Amount);
			} else if (Amount < 0) {
				Amount = 0;
				FadeToZero = false;
				image.color = new Color (image.color.r, image.color.g, image.color.b, Amount);
			}
			if (FadeToBlack) {
				FadeToBlack = false;
			}

		}
	}
}
