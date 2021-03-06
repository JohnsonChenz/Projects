using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutilAnims : MonoBehaviour {

	public RuntimeAnimatorController[] Anims; 

	Animator anim;

	int Switch = 0;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
		RandomAnim ();
	}
		
	void RandomAnim()
	{
		Switch = Random.Range (0, Anims.Length);
		anim.runtimeAnimatorController = Anims [Switch];
	}
	// Update is called once per frame
	void Update () {
		
	}
}
