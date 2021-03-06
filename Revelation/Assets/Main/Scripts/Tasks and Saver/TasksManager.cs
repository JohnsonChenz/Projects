using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TasksManager : MonoBehaviour {

	public GameObject Task;
	public GameObject Tips;

	public CharaterInventory charaterinventory;
	public CharaterIK characterIk;
	public ActorEvent actorevent;
	public MoveControl movecontrol;
	public Transform MainCharacter;

	public bool Loaditem;

	public GameObject KtInfo;

	public bool GamePaused;
	public bool DenyGamePaused;
	// Use this for initialization
	void Start () {
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		movecontrol = MainCharacter.GetComponent<MoveControl> ();
		actorevent = MainCharacter.GetComponent<ActorEvent> ();
		charaterinventory = MainCharacter.GetComponent<CharaterInventory> ();
		characterIk = MainCharacter.GetComponent<CharaterIK> ();

		//KtInfo = GameObject.Find ("InfoText");
		//KtInfo.SetActive (false);
		if (Loaditem) {
			Invoke ("LoadItem", 1f);
		} else {
			//charaterinventory.itemsaver.itemTemp.Clear ();
		}
	}


	public void SaveItem()
	{
		charaterinventory.itemsaver.itemTemp.Clear ();
		for(int i = 0; i<charaterinventory.item.Count ; i++)
		{
			Item t = charaterinventory.item[i];
			charaterinventory.itemsaver.itemTemp.Add (t);

		}
	}

	public void LoadItem()
	{
		for (int i = 0; i < charaterinventory.itemsaver.itemTemp.Count; i++) {
			Object obj = Resources.Load (charaterinventory.itemsaver.itemTemp [i].PrefabPath);
			GameObject gobj = Instantiate (obj) as GameObject;
			Item it = gobj.GetComponent<Item> ();
			it.count = charaterinventory.itemsaver.itemTemp [i].count;
			it.WeaponOnHand = GameObject.Find (it.ModelName);
			print (it.typeItem);
			charaterinventory.AddItems (gobj);
			Destroy (gobj);
		}
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.M))
		{
			if (KtInfo.activeInHierarchy) {
				KtInfo.SetActive (false);
			} else {
				KtInfo.SetActive (true);
			}
		}
		if(Input.GetKeyDown(KeyCode.I))
		{
			//UpdateTipsUI("Object : Discover this area.");
		}
	}

	public void Denygamepaused(float sec)
	{
		CancelInvoke ();
		DenyGamePaused = true;
		Invoke ("b", sec);
	}

	void b()
	{
		DenyGamePaused = false;
	}
	public void UpdateTaskUI(string text,float delay)
	{
		Task.transform.localScale = new Vector3 (0, 0, 0);
		iTween.Stop (Task);
		ITweenScale(Task,0.15f,1f,1,0.2f,delay,true,"TaskTwiceAnim");
		Task.transform.GetChild (0).GetChild(0).GetComponent<Text> ().text = text;

	}

	public void UpdateTipsUI(string text,float delay)
	{
		Tips.transform.localScale = new Vector3 (0, 0, 0);
		iTween.Stop (Tips);
		ITweenScale(Tips,0.15f,1f,1,0.2f,delay,true,"TipsTwiceAnim");
		Tips.transform.GetChild (0).GetComponent<Text> ().text = text;

	}

	void ITweenScale(GameObject gameobject,float x,float y,float z,float time,float delay,bool IsTwice,string TwiceMethod)
	{
		Hashtable args = new Hashtable();
		args.Add("scale", new Vector3(x, y, z));
		args.Add("time", time);
		args.Add("delay", delay);
		args.Add("easeType", iTween.EaseType.linear);

		if (IsTwice) {
			args.Add ("oncomplete", TwiceMethod);
			args.Add ("oncompleteparams", "end");
			args.Add ("oncompletetarget", this.gameObject);
		}

		iTween.ScaleTo(gameobject, args);
	}

	void TaskTwiceAnim()
	{
		ITweenScale(Task,1,1,1,0.1f,0,false,"");
	}

	void TipsTwiceAnim()
	{
		ITweenScale(Tips,1,1,1,0.1f,0,true,"TipsEndAnim");
	}

	void TipsEndAnim()
	{
		ITweenScale(Tips,0.15f,1f,1,0.1f,6f,true,"TipsEndAnimTwice");
	}

	void TipsEndAnimTwice()
	{
		ITweenScale(Tips,0,0,1,0.1f,0,false,"");
	}
}
