using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeHp : MonoBehaviour {

	public TownBoss townboss;
	public string side;

	public GameObject[] Blood;
	public int blood_l;
	public bool Destroyed;
	public Transform PipeTar;

	// Use this for initialization
	void Start () {
		blood_l = -1;
		Destroyed = false;
	}

	public void TakeAwayHealth(float hp)
	{
		if (Destroyed || townboss.isInvincible) {
			return;
		}
		if (side == "Left") {
			if (townboss.Pipe_Left_HP > 0) {
				townboss.Pipe_Left_HP -= hp;
			}
		}
		if (side == "Right") {
			if (townboss.Pipe_Right_HP > 0) {
				townboss.Pipe_Right_HP -= hp;
			} 
		}
	}

	void BloodSpat()
	{
		blood_l++;
		Blood[blood_l].SetActive(true);
		Destroy(Blood[blood_l],2f);
		if (blood_l < Blood.Length) {
			Invoke ("BloodSpat", 0.1f);
		} 

		if (blood_l == Blood.Length - 1) {
			PipeTar.SetParent (null);
			PipeTar.gameObject.AddComponent<Rigidbody> ();
			townboss.Stunned ();
		}
	}

	// Update is called once per frame
	void Update () {
		
		if (Destroyed) {
			return;
		}

		if (side == "Left") {
			if (townboss.Pipe_Left_HP <= 0) {
				BloodSpat ();
				Destroyed = true;
				townboss.isInvincible = true;
			}
		}
		if (side == "Right") {
			if (townboss.Pipe_Right_HP <= 0) {
				BloodSpat ();
				Destroyed = true;
				townboss.isInvincible = true;
			}
		}
	}
}
