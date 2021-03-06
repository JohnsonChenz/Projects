using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public string NameItemSWeaponOnHand;

	public string typeItem;
	public string nameItem;
	public string InventoryName;
	public string InventoryInfo;
	public int count;
	public int countMax;
	public int CraftNumber;
	public bool CountAble;
	public bool isUpgraded;
	public bool isCraftable;

	public string SpritePath;
	public string SpriteColorPath;
	public string SpriteItemBigPath;
	public string SpriteBigPath;
	public string PrefabPath;

	public string PrefabUpgraded;

	public string ModelName;
	public GameObject WeaponOnHand;

	public bool HasBar;

	public int ammo;
	public int damage;
	public int rateoffire;
	public int recoil;
	public int accuracy;

	public bool NonePick;
	// Use this for initialization

	void Awake()
	{
		WeaponOnHand = GameObject.Find (ModelName);
	}
	void Start () {
		
		WeaponOnHand = GameObject.Find (ModelName);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
