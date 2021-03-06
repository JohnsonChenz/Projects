using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoncountReceiver : MonoBehaviour {

	public TownBoss townboss;
	public bool isStage_1_Mons;
	public bool isStage_2_Mons;
	// Use this for initialization
	void Start () {
		
	}

	void OnDestroy()
	{
		if(townboss)
		{
			if (!isStage_1_Mons && !isStage_2_Mons) {
				townboss.Mon_Count--;
			}
			if (isStage_1_Mons) {
				townboss.Boss_Stage_Kill_Goal (1);
			}
			if (isStage_2_Mons) {
				townboss.Boss_Stage_Kill_Goal (2);
			}

		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
