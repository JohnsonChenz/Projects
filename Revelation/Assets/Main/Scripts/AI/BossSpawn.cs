using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour {
	
	public List<GameObject> MonsterList = new List<GameObject>();
	public GameObject Monster;
	public Transform[] Pos;
	//public int Count;
	public int MonsterCount;
	public int MaxAmount;
	public float CoolDown;
	public bool CanSpawn = false;
	public float Count;
	public TasksManager tasksmanager;
	public Transform Scene;

	// Use this for initialization
	void Start () {
		MonsterCount = 0;
		Count = CoolDown;
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		Scene = GameObject.FindGameObjectWithTag ("Scene").transform;
	}

	public void Spawn()
	{
		
		this.GetComponent<AI2> ().IsSpawningMonster = true;
		for (int i = 0; i < MonsterList.Count; i++) {
			if (MonsterList [i]) {
				if (MonsterList [i].GetComponent<EnemyStat> ().health == 0) {
					MonsterList.Remove (MonsterList [i]);
					MonsterCount--;
				}
			} else {
				MonsterList.Remove (MonsterList [i]);
				MonsterCount--;
			}
		}

		Invoke ("SpawnStart", 1f);
	}


	void SpawnStart()
	{
		CanSpawn = false;
		this.GetComponent<AI2> ().IsSpawningMonster = false;
		if (MonsterCount < MaxAmount) {
			for (int i = 0; i < MaxAmount; i++) {
				GameObject Obj = Instantiate (Monster, Pos [i].position, Pos [i].rotation,Scene);
				Obj.SetActive (true);
				MonsterList.Add (Obj);
				Count = CoolDown;
				MonsterCount++;
			}
		} else {
			Count = CoolDown;
		}
			
		this.GetComponent<AI2> ().anim.SetBool ("isRunning", true);
	}
	// Update is called once per frame
	void Update () {

		if (tasksmanager.GamePaused) {
			return;
		}

		if (Count > 0 && this.GetComponent<AI2> ().enabled == true) {
			Count -= Time.deltaTime;
		} else if(this.GetComponent<AI2> ().enabled == true) {
			Count = 0;
			CanSpawn = true;
		}
	}
}
