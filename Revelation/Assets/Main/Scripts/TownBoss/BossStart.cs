using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStart : MonoBehaviour {

	public TownBoss townboss;
	public bool IsTriggered;
	public GameObject TranslateObj;
	// Use this for initialization
	void Start () {
		townboss.GetComponent<TownBoss> ().enabled = false;
		townboss.GetComponent<TownBoss> ().isInvincible = true;
		townboss.GetComponent<TownBoss> ().Stage = 0;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "MainCharater" && !IsTriggered) {
			GetComponent<SceneBlocker> ().BlockScene ();
		    Invoke ("Bossstart", 16f);
			IsTriggered = true;
		}
	}

	void Bossstart()
	{
	    townboss.GetComponent<TownBoss> ().enabled = true;
		townboss.GetComponent<Animator> ().SetTrigger ("Morn");
		TranslateObj.GetComponent<Translate>().ITweenTranslate();
	}
	// Update is called once per frame
	void Update () {
		
	}
}
