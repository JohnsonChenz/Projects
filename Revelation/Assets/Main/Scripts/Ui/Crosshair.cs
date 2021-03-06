using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Crosshair : MonoBehaviour {

	public float currentSpread;
	public float speedSpread;
    public float maxspread;

    public float vertical;
	public float horizontal;
	public float moveAmount;

    float recoil;

	public Parts[] parts;
	float t;
	float curSpread;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    public void ShotCrosshairSpread(float amount)
    {
        recoil += amount;
    }

	void Update () {
		
		vertical = Input.GetAxis ("Vertical");
		horizontal = Input.GetAxis ("Horizontal");
		moveAmount = Mathf.Clamp01 (Mathf.Abs (vertical) + Mathf.Abs (horizontal));

        if (recoil > 1f)
        {
            recoil -= Time.deltaTime * 10;
        }
        else
        {
            recoil = 1;
        }

        if (moveAmount > 0)
			currentSpread = 20 * (3 + moveAmount) * recoil;
		else
			currentSpread = 20 * recoil;

        if(currentSpread > maxspread)
        {
            currentSpread = maxspread;
        }
		CrosshairUpdate();

	}

	public void CrosshairUpdate()
	{
		t = 0.005f * speedSpread;
		curSpread = Mathf.Lerp (curSpread, currentSpread, t);

		for (int i = 0; i < parts.Length; i++) {
			Parts p = parts [i];
			p.trans.anchoredPosition = p.pos * curSpread;
		}


	}

	[System.Serializable]
	public class Parts
	{
		public RectTransform trans;
		public Vector2 pos;
	}
}
