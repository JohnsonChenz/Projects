using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemWhenDead : MonoBehaviour {


	public GameObject[] objs;
	bool HasSpawned;
	public bool isRandom;
	public Transform Scene;

	[System.Serializable]
	public class randomobjs
	{
		public GameObject[] objs2;
	}

	public randomobjs[] randomobj;

	// Use this for initialization
	void Start () {
		Scene = GameObject.FindGameObjectWithTag ("Scene").transform;
	}

	void OnEnable()
	{
		Scene = GameObject.FindGameObjectWithTag ("Scene").transform;
	}
	public void SpawnItem()
	{

		if (!isRandom) {
			if (!HasSpawned) {
				HasSpawned = true;
				for (int i = 0; i < objs.Length; i++) {
					if (objs [i]) {
						GameObject newObj = Instantiate<GameObject> (objs [i],Scene.transform);
						newObj.transform.position = transform.position + (transform.forward * i) + transform.up;
					}
				}
			}
		} else {
			if (!HasSpawned) {
				HasSpawned = true;

			    int r = Random.Range (0, randomobj.Length);
				for(int i = 0 ; i < randomobj[r].objs2.Length ; i++)
			    {
					GameObject newObj = Instantiate<GameObject> (randomobj[r].objs2[i],Scene.transform);
					newObj.transform.position = transform.position + (transform.forward * i) + transform.up;	
				}
			}
		}


	}
	// Update is called once per frame
	void Update () {
		
	}
}
