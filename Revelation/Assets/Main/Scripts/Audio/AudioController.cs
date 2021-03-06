using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public AudioClip[] AudiosFootSteps;
	public AudioClip[] Audios_0;
	public AudioClip[] Audios_1;
	public AudioClip[] Audios_2;
	public AudioClip[] Audios_3;
	public AudioClip[] Audios_4;
	public AudioClip[] Audios_5;
	public AudioClip[] Audios_6;
	AudioSource AS;
	MoveControl movecontrol;
	int min = 0;
	public int max;
	public float cooldown;
	float cd;
	public bool Not_YESORNO;

	public GameObject MainCharater;
	public CharaterStatus charaterstatus;

	// Use this for initialization
	void Start () {
		MainCharater = GameObject.Find ("ybot").gameObject;
		AS = GetComponent<AudioSource> ();
		movecontrol = MainCharater.GetComponent<MoveControl> ();
		charaterstatus = movecontrol.charaterstatus;
	}

	void Update()
	{
		if (cd > 0) {
			cd -= Time.deltaTime;
		} else {
			cd = 0;
		}

		if (charaterstatus.isDeath == true && this.tag != "MainCharater") {
			AS.Stop();
		} else if (charaterstatus.isDeath == false && this.tag != "MainCharater"){
			AS.Play();
		}
	}



	public void PlaySounds_0(int ID)
	{
		AS.Stop ();
		AS.PlayOneShot (Audios_0 [ID]);
	}

	public void PlaySounds_1()
	{

		int YESORNO = Random.Range (0, 2);

		if (Not_YESORNO) {
			YESORNO = 1;
		}

		if(YESORNO == 0 || cd > 0)
		{
			return;
		}

		max = -1;
		for (int i = 0; i < Audios_1.Length; i++) {
			max++;
		}
		int RandomRange = Random.Range (min, max);
		AS.Stop ();
		AS.PlayOneShot (Audios_1 [RandomRange]);

		cd = cooldown;
	}

	public void PlaySounds_2()
	{
		int YESORNO = Random.Range (0, 2);


		if (Not_YESORNO) {
			YESORNO = 1;
		}

		if(YESORNO == 0 || cd > 0)
		{
			return;
		}

		max = -1;
		for (int i = 0; i < Audios_2.Length; i++) {
			max++;
		}
		int RandomRange = Random.Range (min, max);
		AS.Stop ();
		AS.PlayOneShot (Audios_2 [RandomRange]);

		cd = cooldown;
	}

	public void PlaySounds_3()
	{
		int YESORNO = Random.Range (0, 2);

		if (Not_YESORNO) {
			YESORNO = 1;
		}

		if(YESORNO == 0 || cd > 0)
		{
			return;
		}

		max = -1;
		for (int i = 0; i < Audios_3.Length; i++) {
			max++;
		}
		int RandomRange = Random.Range (min, max);
		AS.Stop ();
		AS.PlayOneShot (Audios_3 [RandomRange]);

		cd = cooldown;
	}

	public void PlaySounds_4_NoRandomAndCd()
	{

		max = -1;
		for (int i = 0; i < Audios_4.Length; i++) {
			max++;
		}
		int RandomRange = Random.Range (min, max);
		AS.PlayOneShot (Audios_4 [RandomRange]);

		cd = cooldown;
	}

	public void PlaySounds_5_NoRandomAndCd()
	{
		max = -1;
		for (int i = 0; i < Audios_5.Length; i++) {
			max++;
		}
		int RandomRange = Random.Range (min, max);
		AS.PlayOneShot (Audios_5 [RandomRange]);

		cd = cooldown;
	}

	public void PlaySounds_6_NoRandomAndCd()
	{
		max = -1;
		for (int i = 0; i < Audios_6.Length; i++) {
			max++;
		}
		int RandomRange = Random.Range (min, max);
		AS.PlayOneShot (Audios_6 [RandomRange]);

		cd = cooldown;
	}

	public void FootSteps(int ID)
	{


		if ((movecontrol.charaterstatus.isAiming)){
			return;
		}

		if (movecontrol.IsWalking && ID == 0) {
			AS.PlayOneShot (AudiosFootSteps [ID]);
		}
		if (movecontrol.IsRunning && ID == 1) {
			AS.PlayOneShot (AudiosFootSteps [ID]);
		}
	}
}
