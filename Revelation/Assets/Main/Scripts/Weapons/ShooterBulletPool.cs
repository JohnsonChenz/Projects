using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBulletPool : MonoBehaviour {

	public GameObject Bullets;
	public int InsCount = 20;
	// Use this for initialization
	public Queue<GameObject> pool = new Queue<GameObject>();
	public Vector3 OriginScale;
	public bool Spawned = false;
	void Awake () {
		if (!Spawned) {
			for (int i = 0; i < InsCount; i++) {
				GameObject go = Instantiate (Bullets) as GameObject;
				OriginScale = go.transform.localScale;
				pool.Enqueue (go);
				go.SetActive (false);
			}
			Spawned = true;
		}
	}

	public void SpawnBullet(Vector3 position, Quaternion rotation, float damage, Color color, int speed, float scale)
	{
		if (pool.Count > 0) {
			GameObject reuse = pool.Dequeue ();
			reuse.transform.position = position;
			reuse.transform.rotation = rotation;
			reuse.SetActive (true);
			reuse.GetComponent<ShooterBullet> ().damage = damage;
			reuse.GetComponent<ShooterBullet> ().Speed = speed;
			reuse.transform.localScale = OriginScale * scale;
			//reuse.transform.localScale = new Vector3 (reuse.transform.localScale.x, reuse.transform.localScale.y, reuse.transform.localScale.z) * scale;
			reuse.GetComponent<Renderer>().material.SetColor("_EmissionColor", color * 4f);
		} else {
			GameObject go = Instantiate (Bullets) as GameObject;
			go.transform.position = position;
			go.transform.rotation = rotation;
		}
	}

	public void RecoveryBullet(GameObject recovery)
	{
		pool.Enqueue (recovery);
		recovery.SetActive(false);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
