using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBulletPool : MonoBehaviour {

	public GameObject Bullets;
	public int InsCount = 20;
	// Use this for initialization
	public Queue<GameObject> pool = new Queue<GameObject>();
	public bool Spawned = false;
	void Awake () {
		if (!Spawned) {
			for (int i = 0; i < InsCount; i++) {
				GameObject go = Instantiate (Bullets) as GameObject;
				pool.Enqueue (go);
				go.SetActive (false);
			}
			Spawned = true;
		}
	}

	public void SpawnFlameBullet(Vector3 position, Quaternion rotation, Transform parent)
	{
		if (pool.Count > 0) {
			GameObject reuse = pool.Dequeue ();
			reuse.transform.position = position;
			reuse.transform.rotation = rotation;
			reuse.SetActive (true);
			reuse.transform.SetParent (parent);
		} else {
			GameObject go = Instantiate (Bullets) as GameObject;
			go.transform.position = position;
			go.transform.rotation = rotation;
		}
	}

	public void RecoveryFlameBullet(GameObject recovery)
	{
		pool.Enqueue (recovery);
		recovery.transform.SetParent (null);
		recovery.GetComponent<ParticleSystem> ().time = 0;
		recovery.GetComponent<ParticleSystem> ().Stop();
		recovery.SetActive(false);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
