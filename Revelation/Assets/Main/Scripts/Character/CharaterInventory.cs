using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaterInventory : MonoBehaviour {

	public CharaterIK charaterik;

	public Animator anim;

	public Transform LeftHandTarget;
	public bool WeaponAvailable;

	public Interact interact;

	public CharaterStatus charaterstatus;

	public Weapon activeWeapon;
	public Weapon OldactiveWeapon;
	public Melee activeMeleeWeapon;

	public int Weaponcount;
	public int SwitchWeaponAgain;

	public Crosshair crosshair;

	public List<Item> item = new List<Item>();
	public ItemSaver itemsaver;
	//public List<Item> itemTemp = new List<Item>();
	public List<Item> itemOnTheGround = new List<Item>();

	public GameObject CanvasInventory;

	public GameObject cell;
	public GameObject cellCraft;
	public GameObject cellItem;
	public GameObject BigItemcell;
	public GameObject CraftItemcell;
	public Transform WeaponCell;
	public Transform OtherCell;
	public Transform CraftCell;

	public Transform WeaponGround;
	public Transform FirstWeaponCell;
	public Transform SecondWeaponCell;
	public Transform MeleeWeaponCell;
	public Transform WeaponIconCell;

	public GameObject RockCell;
	public Transform rockCellGround;

	public Transform CraftGround;
	public Transform CraftGroundWep;
	public Transform CraftGroundStone;
	public Transform CraftGroundDesign;
	public Transform CraftGroundWorks;

	public GameObject ItemAddNotifyCell;
	public Transform ItemAddNotifyGround;

	public int RockCount;

	public Transform AmmoTextGround;

	public Transform CameraDepth;
	public GameObject InventoryCamera;


	public Transform RightHand;
	//public Transform Back;

	public GameObject objweapon;
	public Image[] CurrentWeaponIcon;
	public Image[] CurrentWeaponSortIcon;

	public bool isDragging = false;
	public bool isInventoryActive = false;

	public TasksManager tasksmanager;

	void Start()
	{
		tasksmanager = GameObject.Find ("TasksManager").GetComponent<TasksManager> ();
		Cursor.visible = false;
		RockCount = 0;
		charaterik = GetComponent<CharaterIK>();
		anim = GetComponent<Animator> ();
		WeaponAvailable = false;
		charaterstatus.isSwitching = false;
		SwitchWeaponAgain = 0;
		Weaponcount = 0;
		foreach (Crosshair.Parts xxx in crosshair.parts)
		{
			xxx.trans.gameObject.SetActive(false);
		}
		Invoke ("CurrentWeaponsIcon", 0.01f);
	}



	void Update()
	{

		/*
		if (Input.GetKeyDown (KeyCode.P)) {
			GameObject[] obj = GameObject.FindGameObjectsWithTag ("Monster");
			for (int i = 0; i < obj.Length; i++) {
				if (obj [i].GetComponent<Animator> ().speed == 1) {
					obj [i].GetComponent<Animator> ().speed = 0;
				} else {
					obj [i].GetComponent<Animator> ().speed = 1;
				}
			}
		}
        */
		if (anim.GetInteger ("Switch") > 0) {
			charaterstatus.isSwitching = true;
		} else {
			charaterstatus.isSwitching = false;
		}

		if (CanvasInventory.GetComponent<Canvas> ().enabled) {
			isInventoryActive = true;
			tasksmanager.GamePaused = true;
			if ((charaterstatus.isDoAction || tasksmanager.DenyGamePaused) && !charaterstatus.isHealing) {
				//InventoryCamera.SetActive (false);
				//CameraDepth.GetComponent<RapidBlurEffect> ().enabled = false;
				CameraDepth.Find("UICamera").gameObject.SetActive(true);
				CameraDepth.Find("UICamera").transform.GetComponent<RapidBlurEffect> ().enabled = false;
				CameraDepth.GetComponent<UnityStandardAssets.ImageEffects.DepthOfFieldDeprecated> ().enabled = false;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				CanvasInventory.GetComponent<Canvas> ().enabled = false;
			}
		}
		else
		{
			isInventoryActive = false;
		}

		if (charaterstatus.isDeath) {
			CameraDepth.Find("UICamera").gameObject.SetActive(false);
			CameraDepth.Find("UICamera").transform.GetComponent<RapidBlurEffect> ().enabled = false;
			CameraDepth.GetComponent<UnityStandardAssets.ImageEffects.DepthOfFieldDeprecated> ().enabled = false;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			CanvasInventory.GetComponent<Canvas> ().enabled = false;
		}

		if (activeWeapon != null) {
			AmmoTextGround.transform.GetChild(0).GetComponent<Text>().text = activeWeapon.Ammo.ToString();
			AmmoTextGround.transform.GetChild (1).GetComponent<Text> ().text = activeWeapon.ammoTypeText;
			AmmoTextGround.transform.GetChild(2).GetComponent<Text>().text = activeWeapon.BpAmmo.ToString();
			AmmoTextGround.transform.GetChild(3).GetComponent<Text>().text = "/";
		} else {
			for(int i = 0 ; i < 4 ; i++)
			{
				AmmoTextGround.transform.GetChild(i).GetComponent<Text>().text = null;
			}
		}


		if (charaterstatus.isAiming) {
			anim.SetBool ("IsAiming", true);
		}
		else
		{
			anim.SetBool ("IsAiming", false);
		}

		ResetIk ();


		if (charaterstatus.isDeath == true || charaterstatus.isDoAction) {
			if (!charaterstatus.isDoAction) {
				WeaponAvailable = false;
			}
			if (charaterstatus.isHealing) {
				InventoryActive ();
			}
			return;
		}



		if (!charaterstatus.isAiming && !charaterstatus.isSwitching && !charaterstatus.isLocking && !charaterstatus.isReloading && !charaterstatus.isPickuping && !charaterstatus.isThrowingGrenade && !charaterstatus.isDoAction) {
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				if (FirstWeaponCell.childCount > 0 && FirstWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Weapon> ().isEquipped && !FirstWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Weapon> ().isUsing) {
					if (Weaponcount == 0) {
						anim.SetInteger("Switch", 1);
						Weaponcount = 1;
						WeaponAvailable = false;
					} else {
						anim.SetInteger("Switch", Weaponcount);
						Weaponcount = 0;
						SwitchWeaponAgain = 1;
						WeaponAvailable = false;
					}
					CurrentWeaponsIcon ();
				}
			}

			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				if (SecondWeaponCell.childCount > 0 && SecondWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Weapon> ().isEquipped && !SecondWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Weapon> ().isUsing) {
					if (Weaponcount == 0) {
						anim.SetInteger("Switch", 2);
						Weaponcount = 2;
						WeaponAvailable = false;
					} else {
						anim.SetInteger("Switch", Weaponcount);
						Weaponcount = 0;
						SwitchWeaponAgain = 2;
						WeaponAvailable = false;
					}
					CurrentWeaponsIcon();
				}
			}


			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				if (MeleeWeaponCell.childCount > 0 && MeleeWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Melee> ().isEquipped && !MeleeWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Melee> ().isUsing) {
					if (Weaponcount == 0) {
						anim.SetInteger("Switch", 3);
						Weaponcount = 3;
						WeaponAvailable = false;
					} else {
						anim.SetInteger("Switch", Weaponcount);
						Weaponcount = 0;
						SwitchWeaponAgain = 3;
						WeaponAvailable = false;
					}
					CurrentWeaponsIcon();
				}
			}

			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				anim.SetInteger ("Switch", Weaponcount);
				Weaponcount = 0;
				SwitchWeaponAgain = 0;
				WeaponAvailable = false;
				CurrentWeaponsIcon();
			}

			if (Input.GetKeyDown (KeyCode.Alpha5)) {
				if (OtherCell.childCount > 0 && GetComponent<ybotDamage> ().hp.fillAmount < 1) {
					for (int i = 0; i < OtherCell.childCount; i++) {
						Drag drag = OtherCell.GetChild (i).GetComponent<Drag> ();
						if (drag.item.nameItem == "FirstAidKit") {
							drag.item.count--;
							if (drag.item.count < 1) {
								NoSpawnRemove (drag);
							}
							OtherCell.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = OtherCell.GetChild(i).GetComponent<Drag> ().item.count.ToString();
							GetComponent<ybotDamage> ().Starthealing ("0.3");
							return;
						}
						if (i == OtherCell.childCount && drag.item.nameItem != "FirstAidKit") {
							print ("noMedic");
							return;
						}
					}
				}
			}

			if(Input.GetKeyDown(KeyCode.G))
			{
				//WeaponAvailable = false;
				//anim.SetBool ("IsThrowing", true);
			}
		}


		if(Input.GetKeyDown(KeyCode.K))
		{
			itemsaver.itemTemp.Clear ();
			for(int i = 0; i<item.Count ; i++)
			{
				Item t = item[i];
				itemsaver.itemTemp.Add (t);
			}
		}

		if (Input.GetKeyDown (KeyCode.Y)) {
			item.Clear ();
			for(int i = 0; i<itemsaver.itemTemp.Count ; i++)
			{
				Object obj = Resources.Load (itemsaver.itemTemp[i].PrefabPath);
				GameObject gobj = Instantiate( obj ) as GameObject;
				Item it = gobj.GetComponent<Item> ();
				it.count = itemsaver.itemTemp [i].count;
				it.WeaponOnHand = GameObject.Find (it.ModelName);
				print (it.typeItem);
				AddItems (gobj);
				Destroy (gobj);
			}
		}

		if(Input.GetKeyDown(KeyCode.L))
		{
			for(int i = 0; i<itemsaver.itemTemp.Count ; i++)
			{
				Object obj = Resources.Load (itemsaver.itemTemp[i].PrefabPath);
				GameObject gobj = Instantiate( obj ) as GameObject;
				Item it = gobj.GetComponent<Item> ();
				it.count = itemsaver.itemTemp [i].count;
				it.WeaponOnHand = GameObject.Find (it.ModelName);
				print (it.typeItem);
				AddItems (gobj);
				Destroy (gobj);
			}
		}
			
        
		//CountView ();
		InventoryActive ();

	}

	void CurrentWeaponsIcon()
	{

		if (FirstWeaponCell.childCount == 1) {
			CurrentWeaponIcon [0].sprite = Resources.Load<Sprite> (FirstWeaponCell.transform.GetChild(0).GetComponent<Drag>().item.SpritePath);
			if (Weaponcount == 1 || SwitchWeaponAgain == 1) {
				CurrentWeaponIcon [0].color = new Color (CurrentWeaponIcon [0].color.r, CurrentWeaponIcon [0].color.g, CurrentWeaponIcon [0].color.b, 1);
				CurrentWeaponSortIcon[0].color = new Color (CurrentWeaponIcon [0].color.r, CurrentWeaponIcon [0].color.g, CurrentWeaponIcon [0].color.b, 1);
			} else if(Weaponcount != 1){
				CurrentWeaponIcon [0].color = new Color (CurrentWeaponIcon [0].color.r, CurrentWeaponIcon [0].color.g, CurrentWeaponIcon [0].color.b, 1);
				CurrentWeaponSortIcon[0].color = new Color (CurrentWeaponIcon [0].color.r, CurrentWeaponIcon [0].color.g, CurrentWeaponIcon [0].color.b, 0.1f);
			}
		} 
		else 
		{
				CurrentWeaponIcon [0].sprite = null;
			    CurrentWeaponIcon [0].color = new Color (CurrentWeaponIcon [0].color.r, CurrentWeaponIcon [0].color.g, CurrentWeaponIcon [0].color.b, 0);
			CurrentWeaponSortIcon[0].color = new Color (CurrentWeaponIcon [0].color.r, CurrentWeaponIcon [0].color.g, CurrentWeaponIcon [0].color.b, 0.1f);
		}
        

		if (SecondWeaponCell.childCount == 1) {
			CurrentWeaponIcon [1].sprite = Resources.Load<Sprite> (SecondWeaponCell.transform.GetChild(0).GetComponent<Drag>().item.SpritePath);
			if (Weaponcount == 2 || SwitchWeaponAgain == 2) {
				CurrentWeaponIcon [1].color = new Color (CurrentWeaponIcon [1].color.r, CurrentWeaponIcon [1].color.g, CurrentWeaponIcon [1].color.b, 1);
				CurrentWeaponSortIcon[1].color = new Color (CurrentWeaponIcon [1].color.r, CurrentWeaponIcon [1].color.g, CurrentWeaponIcon [1].color.b, 1);
			} else if(Weaponcount != 2){
				CurrentWeaponIcon [1].color = new Color (CurrentWeaponIcon [1].color.r, CurrentWeaponIcon [1].color.g, CurrentWeaponIcon [1].color.b, 1);
				CurrentWeaponSortIcon[1].color = new Color (CurrentWeaponIcon [1].color.r, CurrentWeaponIcon [1].color.g, CurrentWeaponIcon [1].color.b, 0.1f);
			}
		} 
		else 
		{
			CurrentWeaponIcon [1].sprite = null;
			CurrentWeaponIcon [1].color = new Color (CurrentWeaponIcon [1].color.r, CurrentWeaponIcon [1].color.g, CurrentWeaponIcon [1].color.b, 0);
			CurrentWeaponSortIcon[1].color = new Color (CurrentWeaponIcon [1].color.r, CurrentWeaponIcon [1].color.g, CurrentWeaponIcon [1].color.b, 0.1f);
		}

		if (MeleeWeaponCell.childCount == 1) {
			CurrentWeaponIcon [2].sprite = Resources.Load<Sprite> (MeleeWeaponCell.transform.GetChild(0).GetComponent<Drag>().item.SpritePath);
			if (Weaponcount == 3 || SwitchWeaponAgain == 3) {
				CurrentWeaponIcon [2].color = new Color (CurrentWeaponIcon [2].color.r, CurrentWeaponIcon [2].color.g, CurrentWeaponIcon [2].color.b, 1);
				CurrentWeaponSortIcon[2].color = new Color (CurrentWeaponIcon [2].color.r, CurrentWeaponIcon [2].color.g, CurrentWeaponIcon [2].color.b, 1f);
			} else if(Weaponcount != 3){
				CurrentWeaponIcon [2].color = new Color (CurrentWeaponIcon [2].color.r, CurrentWeaponIcon [2].color.g, CurrentWeaponIcon [2].color.b, 1);
				CurrentWeaponSortIcon[2].color = new Color (CurrentWeaponIcon [2].color.r, CurrentWeaponIcon [2].color.g, CurrentWeaponIcon [2].color.b, 0.1f);
			}
		} 
		else 
		{
			CurrentWeaponIcon [2].sprite = null;
			CurrentWeaponIcon [2].color = new Color (CurrentWeaponIcon [2].color.r, CurrentWeaponIcon [2].color.g, CurrentWeaponIcon [2].color.b, 0);
			CurrentWeaponSortIcon[2].color = new Color (CurrentWeaponIcon [2].color.r, CurrentWeaponIcon [2].color.g, CurrentWeaponIcon [2].color.b, 0.1f);
		}
	}

	public void SwitchWeapon()
	{
		charaterik.r_Hand.gameObject.GetComponent<Animator>().runtimeAnimatorController = null;
		anim.SetBool("IsHoldingWeapon", true);
		anim.SetBool ("IsReloading", false);
		Image WeaponIcon = WeaponIconCell.transform.GetChild (0).GetComponent<Image> ();

		if (Weaponcount == 2) {

			ResetWeapon ();

			//anim.SetInteger ("Switch", 0);
			//WeaponAvailable = true;


			activeWeapon = SecondWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Weapon> ();
			activeWeapon.WeaponSelf.transform.SetParent (SecondWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.transform);
			activeWeapon.WeaponSelf.transform.localPosition = new Vector3 (0, 0, 0);
			Quaternion ResetRot = Quaternion.Euler (0, 0, 0);
			activeWeapon.WeaponSelf.transform.localRotation = ResetRot;
			activeWeapon.isUsing = true;

			charaterik.l_Hand_Target = activeWeapon.LeftHandTarget;
			charaterik.RightHandAnim = activeWeapon.WeaponProperties.RightHandAnim;
			charaterik.r_Hand.gameObject.GetComponent<Animator>().runtimeAnimatorController = activeWeapon.WeaponProperties.RightHandAnim;
			charaterik.r_Hand.localPosition = activeWeapon.WeaponProperties.rHandPos;
			Quaternion rotRight = Quaternion.Euler (activeWeapon.WeaponProperties.rHandRot.x, activeWeapon.WeaponProperties.rHandRot.y, activeWeapon.WeaponProperties.rHandRot.z);
			charaterik.r_Hand.localRotation = rotRight;

			WeaponIcon.color = new Color(WeaponIcon.color.r, WeaponIcon.color.g,WeaponIcon.color.b, 1);
			WeaponIcon.sprite = Resources.Load<Sprite> (SecondWeaponCell.transform.GetChild(0).GetComponent<Drag>().item.SpriteBigPath);

			anim.SetBool("HoldingKnife", false);
			anim.SetBool("HoldingGun", true);


			foreach (Crosshair.Parts xxx in crosshair.parts)
			{
				xxx.trans.gameObject.SetActive(true);
				xxx.trans.gameObject.GetComponent<Image>().sprite = activeWeapon.Crosshair;
			}


		}

		if (Weaponcount == 1) {

			ResetWeapon ();

			//WeaponAvailable = true;
			//anim.SetInteger ("Switch", 0);
		
			activeWeapon = FirstWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Weapon> ();
			activeWeapon.WeaponSelf.transform.SetParent (FirstWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.transform);
			activeWeapon.WeaponSelf.transform.localPosition = new Vector3 (0, 0, 0);
			Quaternion ResetRot = Quaternion.Euler (0, 0, 0);
			activeWeapon.WeaponSelf.transform.localRotation = ResetRot;
			activeWeapon.isUsing = true;

			charaterik.l_Hand_Target = activeWeapon.LeftHandTarget;
			charaterik.RightHandAnim = activeWeapon.WeaponProperties.RightHandAnim;
			charaterik.r_Hand.gameObject.GetComponent<Animator>().runtimeAnimatorController = activeWeapon.WeaponProperties.RightHandAnim;
			charaterik.r_Hand.localPosition = activeWeapon.WeaponProperties.rHandPos;
			Quaternion rotRight = Quaternion.Euler (activeWeapon.WeaponProperties.rHandRot.x, activeWeapon.WeaponProperties.rHandRot.y, activeWeapon.WeaponProperties.rHandRot.z);
			charaterik.r_Hand.localRotation = rotRight;

			WeaponIcon.color = new Color(WeaponIcon.color.r, WeaponIcon.color.g,WeaponIcon.color.b, 1);
			WeaponIcon.sprite = Resources.Load<Sprite> (FirstWeaponCell.transform.GetChild(0).GetComponent<Drag>().item.SpriteBigPath);

			anim.SetBool("HoldingKnife", false);
			anim.SetBool("HoldingGun", true);

			foreach (Crosshair.Parts xxx in crosshair.parts)
			{
				xxx.trans.gameObject.SetActive(true);
				xxx.trans.gameObject.GetComponent<Image>().sprite = activeWeapon.Crosshair;
			}
		}

		if (Weaponcount == 3) {

			ResetWeapon ();

			//anim.SetInteger ("Switch", 0);

			activeMeleeWeapon = MeleeWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Melee> ();
			activeMeleeWeapon.isUsing = true;

			activeMeleeWeapon.WeaponSelf.transform.SetParent (MeleeWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.transform);
			activeMeleeWeapon.WeaponSelf.transform.localPosition = new Vector3 (0, 0, 0);
			Quaternion ResetRot = Quaternion.Euler (0, 0, 0);
			activeMeleeWeapon.WeaponSelf.transform.localRotation = ResetRot;

			activeMeleeWeapon.WeaponSelf.SetActive (true);

			activeWeapon = null;
			WeaponAvailable = false;

			WeaponIcon.color = new Color(WeaponIcon.color.r, WeaponIcon.color.g,WeaponIcon.color.b, 1);
			WeaponIcon.sprite = Resources.Load<Sprite> (MeleeWeaponCell.transform.GetChild(0).GetComponent<Drag>().item.SpriteBigPath);

			anim.SetBool("HoldingGun", false);
			anim.SetBool("HoldingKnife", true);

			foreach (Crosshair.Parts xxx in crosshair.parts)
			{
				xxx.trans.gameObject.SetActive(false);
			}
		}

		if (Weaponcount == 0) {



			WeaponIcon.color = new Color(WeaponIcon.color.r, WeaponIcon.color.g,WeaponIcon.color.b, 0);
			WeaponIcon.sprite = null;
			ResetWeapon ();
			DisableWeapon ();




			foreach (Crosshair.Parts xxx in crosshair.parts)
			{
				xxx.trans.gameObject.SetActive(false);
			}
		}


	}

	public void SwitchWeaponEnd()
	{
		Invoke ("CheckWeapon", 0.01f);
		if (activeWeapon != null) {
			WeaponAvailable = true;
		} else {
			WeaponAvailable = false;
		}
		if (SwitchWeaponAgain == 1) {
			anim.SetInteger ("Switch", 1);
			Weaponcount = 1;
		}
		if (SwitchWeaponAgain == 2) {
			anim.SetInteger ("Switch", 2);
			Weaponcount = 2;
		}
		if (SwitchWeaponAgain == 3) {
			anim.SetInteger ("Switch", 3);
			Weaponcount = 3;
		}
		if (SwitchWeaponAgain == 0){
			anim.SetInteger ("Switch", 0);
		}
		SwitchWeaponAgain = 0;

	}

	public void ResetWeapon()
	{
		if (activeWeapon != null) {
			OldactiveWeapon = activeWeapon;
			activeWeapon = null;
		}


		if (OldactiveWeapon != null) {
			OldactiveWeapon.isUsing = false;
			OldactiveWeapon.WeaponSelf.transform.SetParent (OldactiveWeapon.PutBackTrans);
			OldactiveWeapon.WeaponSelf.transform.localPosition = OldactiveWeapon.WeaponProperties.BackPos;
			Quaternion rotBack = Quaternion.Euler (OldactiveWeapon.WeaponProperties.BackRot.x, OldactiveWeapon.WeaponProperties.BackRot.y, OldactiveWeapon.WeaponProperties.BackRot.z);
			OldactiveWeapon.WeaponSelf.transform.localRotation = rotBack;

		}

		if (activeMeleeWeapon != null) {
			activeMeleeWeapon.isUsing = false;
			activeMeleeWeapon.WeaponSelf.transform.SetParent (activeMeleeWeapon.PutBackTrans);
			activeMeleeWeapon.WeaponSelf.transform.localPosition = activeMeleeWeapon.WeaponProperties.BackPos;
			Quaternion rotBack = Quaternion.Euler (activeMeleeWeapon.WeaponProperties.BackRot.x, activeMeleeWeapon.WeaponProperties.BackRot.y, activeMeleeWeapon.WeaponProperties.BackRot.z);
			activeMeleeWeapon.WeaponSelf.transform.localRotation = rotBack;
			activeMeleeWeapon = null;
		}

		if (MeleeWeaponCell.childCount > 0) {
			MeleeWeaponCell.transform.GetChild (0).GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Melee>().isUsing = false;
		}


	}

	public void DisableWeapon()
	{
		activeWeapon = null;
		if (SwitchWeaponAgain == 0) {
			anim.SetBool ("IsHoldingWeapon", false);
			anim.SetBool ("HoldingGun", false);
			anim.SetBool ("HoldingKnife", false);
		}
		WeaponAvailable = false;
	}

	public void CheckWeaponBodyWhenDisable(Drag drag)
	{
		if ((drag.item.typeItem == "Use W" || drag.item.typeItem == "Use W2")) {
			if (drag.item.WeaponOnHand.GetComponent<Weapon> ().isUsing) {
				if (Weaponcount == 1) {
					anim.SetInteger ("Switch", 1);
				}
				if (Weaponcount == 2) {
					anim.SetInteger ("Switch", 2);
				}
				Weaponcount = 0;

				drag.item.WeaponOnHand.GetComponent<Weapon> ().WeaponSelf.SetActive (false);
				activeWeapon = null;
				WeaponAvailable = false;
			} else {
				drag.item.WeaponOnHand.GetComponent<Weapon> ().WeaponSelf.SetActive (false);
			}
		}
		if (drag.item.typeItem == "Use W3") {
			if (drag.item.WeaponOnHand.GetComponent<Melee> ().isUsing) {
				if (Weaponcount == 3) {
					anim.SetInteger ("Switch", 3);
				}
				Weaponcount = 0;
				drag.item.WeaponOnHand.GetComponent<Melee> ().WeaponSelf.SetActive (false);
				activeWeapon = null;
				WeaponAvailable = false;
			} else {
				drag.item.WeaponOnHand.GetComponent<Melee> ().WeaponSelf.SetActive (false);
			}
		}

	}

	public void CheckWeapon()
	{

		if (FirstWeaponCell.childCount > 0) {

			Transform obj;

			obj = FirstWeaponCell.transform.GetChild (0);

			Weapon wep = obj.GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Weapon> ();
			if (wep.isEquipped) {
				if (!wep.isUsing) {
					wep.WeaponSelf.SetActive (true);
					wep.WeaponSelf.transform.SetParent (wep.PutBackTrans);
					wep.WeaponSelf.transform.localPosition = wep.WeaponProperties.BackPos;
					Quaternion rotBack = Quaternion.Euler (wep.WeaponProperties.BackRot.x, wep.WeaponProperties.BackRot.y, wep.WeaponProperties.BackRot.z);
					wep.WeaponSelf.transform.localRotation = rotBack;

				}
			}


		}

		if (SecondWeaponCell.childCount > 0) {

			Transform obj;

			obj = SecondWeaponCell.transform.GetChild (0);
			Weapon wep = obj.GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Weapon> ();
			if (wep.isEquipped) {
				if (!wep.isUsing) {
					wep.WeaponSelf.SetActive (true);
					wep.WeaponSelf.transform.SetParent (wep.PutBackTrans);
					wep.WeaponSelf.transform.localPosition = wep.WeaponProperties.BackPos;
					Quaternion rotBack = Quaternion.Euler (wep.WeaponProperties.BackRot.x, wep.WeaponProperties.BackRot.y, wep.WeaponProperties.BackRot.z);
					wep.WeaponSelf.transform.localRotation = rotBack;

				}
			}
		}

		if (MeleeWeaponCell.childCount > 0) {

			Transform obj;

			obj = MeleeWeaponCell.transform.GetChild (0);
			Melee wep = obj.GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Melee> ();
			if (wep.isEquipped) {
				if (!wep.isUsing) {
					wep.WeaponSelf.SetActive (true);
					wep.WeaponSelf.transform.SetParent (wep.PutBackTrans);
					wep.WeaponSelf.transform.localPosition = wep.WeaponProperties.BackPos;
					Quaternion rotBack = Quaternion.Euler (wep.WeaponProperties.BackRot.x, wep.WeaponProperties.BackRot.y, wep.WeaponProperties.BackRot.z);
					wep.WeaponSelf.transform.localRotation = rotBack;

				}
			}
		}

	}




	public void ResetIk()
	{
		if (WeaponAvailable == false && charaterik.interactTarget == false) {
			charaterik.lh_Weight -= Time.deltaTime * charaterik.lh_WeightMinusMtp;
		} else if (WeaponAvailable == true && charaterik.interactTarget == false) {
			if (charaterik.lh_Weight < 0.05f) {
				charaterik.lh_Weight += Time.deltaTime * charaterik.lh_WeightAddMtp;
			} else {
				charaterik.lh_Weight += Time.deltaTime * charaterik.lh_WeightAddMtp * 10;
			}
		} else {
			charaterik.lh_Weight = 0;
		}
		charaterik.lh_Weight = Mathf.Clamp (charaterik.lh_Weight, 0, 1);
		//charaterik.rh_Weight = Mathf.Clamp (charaterik.rh_Weight, 0, 1);
	}





	public void Craft()
	{
		Drag dragWep = CraftGroundWep.GetChild (0).GetComponent<Drag> ();
		Drag dragStone = CraftGroundStone.GetChild (0).GetComponent<Drag> ();
		Drag dragDesignMap = CraftGroundDesign.GetChild (0).GetComponent<Drag> ();

		if (dragWep.item.CraftNumber == dragStone.item.CraftNumber && dragWep.item.CraftNumber == dragDesignMap.item.CraftNumber) {
			SpawnItem (dragWep);
			NoSpawnRemove (dragWep);
			NoSpawnRemove (dragStone);
			NoSpawnRemove (dragDesignMap);
			print ("合成成功");
		}
		else 
		{
			print ("合成失敗:合成物品錯誤");
		}




	}


	void CountView()
	{
		if (rockCellGround.childCount > 0) {
			rockCellGround.GetChild (0).transform.GetChild(1).GetComponent<Text> ().text = RockCount.ToString();
		}
	}

	public void UseItem(Drag drag, bool isByPicking)
	{
		if (charaterstatus.isSwitching || charaterstatus.isReloading)
			return;
		if (drag.item.typeItem == "First Weapon" && (WeaponGround.gameObject.activeInHierarchy || isByPicking)) {
			if (FirstWeaponCell.childCount != 0) {
				if (charaterstatus.isSwitching) {
					return;
				}
				Drag dragOld = FirstWeaponCell.transform.GetChild (0).GetComponent<Drag> ();
				UnequipItem(dragOld);

			}
				GameObject newCell = Instantiate (BigItemcell);
				newCell.GetComponent<Drag> ().item = drag.item;
				Destroy (drag.gameObject);
				newCell.transform.SetParent (FirstWeaponCell);

				newCell.GetComponent<Drag>().item.WeaponOnHand.GetComponent<Weapon> ().isEquipped = true;
				newCell.GetComponent<Drag> ().typeList = "Inventory";
				newCell.GetComponent<Drag>().item.typeItem = "Use W";
			    //ResetUiScale (newCell);
			    ResetUiScale (newCell);

				newCell.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (newCell.GetComponent<Drag>().item.SpriteItemBigPath);
			newCell.transform.GetChild (1).GetComponent<Text> ().text = newCell.GetComponent<Drag> ().item.InventoryName;

			if (Weaponcount == 0) {
				Weaponcount = 1;
				anim.SetInteger ("Switch", 1);
			}
		}

		if (drag.item.typeItem == "Second Weapon" && (WeaponGround.gameObject.activeInHierarchy || isByPicking)) {
			if (SecondWeaponCell.childCount != 0) 
			{
				if (charaterstatus.isSwitching) {
					return;
				}
				Drag dragOld = SecondWeaponCell.transform.GetChild (0).GetComponent<Drag> ();
				UnequipItem(dragOld);
			}
				GameObject newCell = Instantiate (BigItemcell);
				newCell.GetComponent<Drag> ().item = drag.item;
				Destroy (drag.gameObject);
				newCell.transform.SetParent (SecondWeaponCell);
				newCell.GetComponent<Drag>().item.WeaponOnHand.GetComponent<Weapon> ().isEquipped = true;
				newCell.GetComponent<Drag> ().typeList = "Inventory";
				newCell.GetComponent<Drag>().item.typeItem = "Use W2";
			    ResetUiScale (newCell);

				newCell.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (newCell.GetComponent<Drag>().item.SpriteItemBigPath);
			newCell.transform.GetChild (1).GetComponent<Text> ().text = newCell.GetComponent<Drag> ().item.InventoryName;

			if (Weaponcount == 0) {
				Weaponcount = 2;
				anim.SetInteger ("Switch", 2);
			}
		}


		if (drag.item.typeItem == "Melee Weapon" && (WeaponGround.gameObject.activeInHierarchy || isByPicking)) {
			if (MeleeWeaponCell.childCount != 0) 
			{
				if (charaterstatus.isSwitching) {
					return;
				}
				Drag dragOld = MeleeWeaponCell.transform.GetChild (0).GetComponent<Drag> ();
				UnequipItem(dragOld);
			}
				GameObject newCell = Instantiate (BigItemcell);
				newCell.GetComponent<Drag> ().item = drag.item;
				Destroy (drag.gameObject);
				newCell.transform.SetParent (MeleeWeaponCell);
				newCell.GetComponent<Drag>().item.WeaponOnHand.GetComponent<Melee> ().isEquipped = true;
				newCell.GetComponent<Drag> ().typeList = "Inventory";
				newCell.GetComponent<Drag>().item.typeItem = "Use W3";
			    ResetUiScale (newCell);

				newCell.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (newCell.GetComponent<Drag>().item.SpriteItemBigPath);
			newCell.transform.GetChild (1).GetComponent<Text> ().text = newCell.GetComponent<Drag> ().item.InventoryName;

			if (Weaponcount == 0) {
				Weaponcount = 3;
				anim.SetInteger ("Switch", 3);
			}
		}

		if ((drag.item.typeItem == "First Weapon" || drag.item.typeItem == "Second Weapon" || drag.item.typeItem == "Melee Weapon") && CraftGround.gameObject.activeInHierarchy && !isByPicking ) {
			if (CraftGroundWep.childCount != 0) {
				Drag dragOld = CraftGroundWep.transform.GetChild (0).GetComponent<Drag> ();
				UnequipItem(dragOld);
			}

			GameObject newCell = Instantiate (CraftItemcell);
			newCell.GetComponent<Drag> ().item = drag.item;
			Destroy (drag.gameObject);
			newCell.transform.SetParent (CraftGroundWep);
			newCell.GetComponent<Drag> ().typeList = "Craft";
			ResetUiScale (newCell);

			newCell.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (newCell.GetComponent<Drag>().item.SpriteItemBigPath);
			newCell.transform.GetChild (1).GetComponent<Text> ().text = newCell.GetComponent<Drag> ().item.InventoryName;
		}

		if ((drag.item.typeItem == "Chip" && !isByPicking && CraftGround.gameObject.activeInHierarchy)) {

			if (CraftGroundDesign.childCount != 0) {
				Drag dragOld = CraftGroundDesign.transform.GetChild (0).GetComponent<Drag> ();
				UnequipItem(dragOld);
			}

			GameObject newCell = Instantiate (CraftItemcell);
			newCell.GetComponent<Drag> ().item = drag.item;
			Destroy (drag.gameObject);
			newCell.transform.SetParent (CraftGroundDesign);
			newCell.GetComponent<Drag> ().typeList = "Craft";
			ResetUiScale (newCell);

			newCell.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (newCell.GetComponent<Drag>().item.SpriteItemBigPath);
			newCell.transform.GetChild (1).GetComponent<Text> ().text = newCell.GetComponent<Drag> ().item.InventoryName;
		}

		if ((drag.item.typeItem == "Stone" && !isByPicking && CraftGround.gameObject.activeInHierarchy)) {
			
			if (CraftGroundStone.childCount != 0) {
				Drag dragOld = CraftGroundStone.transform.GetChild (0).GetComponent<Drag> ();
				UnequipItem(dragOld);
			}

				GameObject newCell = Instantiate (CraftItemcell);
				newCell.GetComponent<Drag> ().item = drag.item;
				Destroy (drag.gameObject);
				newCell.transform.SetParent (CraftGroundStone);
				newCell.GetComponent<Drag> ().typeList = "Craft";
			    ResetUiScale (newCell);

				newCell.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (newCell.GetComponent<Drag>().item.SpriteItemBigPath);
				newCell.transform.GetChild (1).GetComponent<Text> ().text = newCell.GetComponent<Drag> ().item.InventoryName;
		}
		print ("use: " + drag.item.nameItem);
		Invoke ("CheckWeapon", 0.01f);
		Invoke ("CurrentWeaponsIcon", 0.01f);
	}

	public void UnequipItem(Drag drag)
	{
		if (charaterstatus.isSwitching || charaterstatus.isReloading) {
			return;
		}

		GameObject newCell = null;

		if (drag.item.typeItem == "Use W" || drag.item.typeItem == "Use W2" || drag.item.typeItem == "Use W3" || drag.item.typeItem == "First Weapon" || drag.item.typeItem == "Second Weapon" || drag.item.typeItem == "Melee Weapon") {
			newCell = Instantiate (cell);
			newCell.transform.SetParent (WeaponCell);
			newCell.transform.GetChild (2).GetComponent<Text> ().text = drag.item.InventoryName;
		} 
		else if(drag.item.typeItem == "Bullet" || drag.item.typeItem == "Other" || drag.item.typeItem == "Key"){
			newCell = Instantiate (cellItem);
			newCell.transform.SetParent (OtherCell);
			newCell.transform.GetChild (2).GetComponent<Text> ().text = drag.item.InventoryName;
		}
		else if(drag.item.typeItem == "Chip" || drag.item.typeItem == "Stone"){
			newCell = Instantiate (cellCraft);
			newCell.transform.SetParent (CraftCell);
			newCell.transform.GetChild (2).GetComponent<Text> ().text = drag.item.InventoryName;
		}

		newCell.GetComponent<Drag> ().item = drag.item;
		Destroy (drag.gameObject);

		newCell.GetComponent<Drag> ().typeList = "Inventory";

		ResetUiScale (newCell);
		//newCell.transform.GetChild (1).GetComponent<Text> ().text = drag.item.InventoryName;
		newCell.transform.GetChild (0).GetComponent<Image>().sprite = Resources.Load<Sprite>(newCell.GetComponent<Drag>().item.SpritePath);
		newCell.transform.GetChild (1).GetComponent<Text> ().text = null;

		if (drag.item.typeItem == "Use W") {

			CheckWeaponBodyWhenDisable (drag);
			drag.item.WeaponOnHand.GetComponent<Weapon> ().isEquipped = false;
			drag.item.WeaponOnHand.GetComponent<Weapon> ().isUsing = false;

			drag.item.typeItem = "First Weapon";

			if (Weaponcount == 1) {
				WeaponAvailable = false;
				charaterik.lh_Weight = 0;
				Weaponcount = 0;
				anim.SetInteger ("Switch", 1);
			}
		}

		if (drag.item.typeItem == "Use W2") {

			CheckWeaponBodyWhenDisable (drag);
			drag.item.WeaponOnHand.GetComponent<Weapon> ().isEquipped = false;
			drag.item.WeaponOnHand.GetComponent<Weapon> ().isUsing = false;

			drag.item.typeItem = "Second Weapon";

			if (Weaponcount == 2) {
				WeaponAvailable = false;
				charaterik.lh_Weight = 0;
				Weaponcount = 0;
				anim.SetInteger ("Switch", 2);
			}
		}

		if (drag.item.typeItem == "Use W3") {

			CheckWeaponBodyWhenDisable (drag);
			drag.item.WeaponOnHand.GetComponent<Melee> ().isEquipped = false;
			drag.item.WeaponOnHand.GetComponent<Melee> ().isUsing = false;

			drag.item.typeItem = "Melee Weapon";

			if (Weaponcount == 3) {
				WeaponAvailable = false;
				charaterik.lh_Weight = 0;
				Weaponcount = 0;
				anim.SetInteger ("Switch", 3);
			}
		}

		print ("use: " + drag.item.nameItem);
		Invoke ("CheckWeapon", 0.01f);
		Invoke ("CurrentWeaponsIcon", 0.01f);
	}



	void SpawnItem(Drag OldDrag)
	{
		Object obj = Resources.Load (OldDrag.item.PrefabUpgraded);

		GameObject gobj = Instantiate( obj ) as GameObject;
		Item it = gobj.GetComponent<Item> ();
		item.Add (it);
		print (it.typeItem);
		Destroy (gobj);

		Drag drag;
		GameObject newCell = Instantiate (CraftItemcell);
		newCell.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (it.SpritePath);
		drag = newCell.GetComponent<Drag> ();
		drag.item = it;
		drag.typeList = "Craft";

		newCell.transform.SetParent (CraftGroundWep);
		ResetUiScale (newCell);
		it.WeaponOnHand = GameObject.Find (it.ModelName);
	}


	public void AddItems(GameObject IT)
	{

		Item it = IT.GetComponent<Item> ();

		GameObject AddCell = Instantiate (ItemAddNotifyCell);

		AddCell.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite> (it.SpritePath + "white");
		if (!it.CountAble) {
			AddCell.transform.GetChild (1).GetComponent<Text> ().text = it.InventoryName;
		} else {
			AddCell.transform.GetChild (1).GetComponent<Text> ().text = it.InventoryName + " (" + it.count.ToString () + ")";
		}
		AddCell.transform.SetParent (ItemAddNotifyGround);
		ResetUiScale (AddCell);


		if(it.typeItem == "Bullet" || it.typeItem == "Medic" || it.typeItem == "Armor"){
			for (int i = 0; i < OtherCell.childCount; i++) {
				if (it.nameItem == OtherCell.GetChild (i).GetComponent<Drag>().item.nameItem && it.CountAble) {
					print ("same");
					print (OtherCell.GetChild (i).GetComponent<Drag> ().item.count);
					print (OtherCell.GetChild (i).GetComponent<Drag> ().item.nameItem);
					OtherCell.GetChild (i).GetComponent<Drag> ().item.count += it.count;
					OtherCell.GetChild (i).transform.GetChild (1).GetComponent<Text> ().text = OtherCell.GetChild (i).GetComponent<Drag> ().item.count.ToString ();
					return;
				}
				Debug.Log (OtherCell.GetChild (i).GetComponent<Drag> ().item.nameItem);
			}
		}


		/*
		foreach (Transform child in parentCell) {
			if (it.nameItem == child.GetComponent<Drag>().item.nameItem && it.CountAble) {
				print ("same");
				print (child.GetComponent<Drag> ().item.count);
				print (child.GetComponent<Drag> ().item.nameItem);
				child.GetComponent<Drag> ().item.count += it.count;
				child.transform.GetChild (1).GetComponent<Text> ().text = child.GetComponent<Drag> ().item.count.ToString ();
				return;
			}
		}
		*/
		item.Add (it);
		//print (it.typeItem);

		if (it.typeItem != "Rocks") {
			Drag drag;
			GameObject newCell = null;

			if (it.typeItem == "First Weapon" || it.typeItem == "Second Weapon" || it.typeItem == "Melee Weapon") {
				newCell = Instantiate (cell);
			} 
			else if(it.typeItem == "Bullet" || it.typeItem == "Other" || it.typeItem == "Key" || it.typeItem == "Medic" || it.typeItem == "Armor"){
				newCell = Instantiate (cellItem);
			}
			else if(it.typeItem == "Stone" || it.typeItem == "Chip"){
				newCell = Instantiate (cellCraft);
			}

			drag = newCell.GetComponent<Drag> ();
			drag.item = it;
			drag.typeList = "Inventory";

			if (FirstWeaponCell.childCount == 0 && it.typeItem == "First Weapon") {
				UseItem (drag, true);
			} 
			else if (SecondWeaponCell.childCount == 0 && it.typeItem == "Second Weapon") {
				UseItem (drag, true);
			} 
			else if (MeleeWeaponCell.childCount == 0 && it.typeItem == "Melee Weapon") {
				UseItem (drag, true);
			} 
			else 
			{
				if (drag.item.typeItem == "First Weapon" || drag.item.typeItem == "Second Weapon" || drag.item.typeItem == "Melee Weapon") {
					newCell.transform.SetParent (WeaponCell);
					newCell.transform.GetChild (2).GetComponent<Text> ().text = drag.item.InventoryName;
				} 
				else if(drag.item.typeItem == "Bullet" || drag.item.typeItem == "Other" || drag.item.typeItem == "Key" || drag.item.typeItem == "Medic" || drag.item.typeItem == "Armor"){
					newCell.transform.GetChild (2).GetComponent<Text> ().text = drag.item.InventoryName;
					newCell.transform.SetParent (OtherCell);
				}
				else if(drag.item.typeItem == "Stone" || drag.item.typeItem == "Chip"){
					newCell.transform.GetChild (2).GetComponent<Text> ().text = drag.item.InventoryName;
					newCell.transform.SetParent (CraftCell);
				}
			}

			ResetUiScale (newCell);


			//newCell.transform.GetChild (1).GetComponent<Text> ().text = drag.item.InventoryName;
			if (it.CountAble) {
				newCell.transform.GetChild (1).GetComponent<Text> ().text = drag.item.count.ToString ();
			} else {
				newCell.transform.GetChild (1).GetComponent<Text> ().text = null;
			}
		} 
		else 
		{
			if (rockCellGround.childCount == 0) {
				RockCount++;
				Drag drag;
				GameObject newCell = Instantiate (RockCell);
				newCell.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (it.SpritePath);
				newCell.transform.GetChild(1).GetComponent<Text> ().text = RockCount.ToString();


				drag = newCell.GetComponent<Drag> ();
				drag.item = it;
				drag.typeList = "Inventory";

				newCell.transform.SetParent (rockCellGround);
				ResetUiScale (newCell);
			} else {
				RockCount++;
				//item.Remove (it);
				print ("already rock inside");
			}

		}

	}


	public void InventoryActive()
	{
		if (Input.GetKeyDown (KeyCode.Tab)) 
		{
			if (isInventoryActive && !isDragging) {
				//InventoryCamera.SetActive (false);
				tasksmanager.GamePaused = false;
				CameraDepth.GetComponent<UnityStandardAssets.ImageEffects.DepthOfFieldDeprecated> ().enabled = false;
				CameraDepth.Find("UICamera").gameObject.SetActive(true);
				CameraDepth.Find("UICamera").transform.GetComponent<RapidBlurEffect> ().enabled = false;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				CanvasInventory.GetComponent<Canvas> ().enabled = false;
				//inventory.SetActive (false);
				//charaterstatus.isPaused = false;
			}
			else if(!isInventoryActive && !tasksmanager.DenyGamePaused)
			{
				tasksmanager.GamePaused = true;
				CameraDepth.GetComponent<UnityStandardAssets.ImageEffects.DepthOfFieldDeprecated> ().enabled = true;
				CameraDepth.Find("UICamera").gameObject.SetActive(false);
				CameraDepth.Find("UICamera").transform.GetComponent<RapidBlurEffect> ().enabled = true;
				//InventoryCamera.SetActive (true);
				CanvasInventory.GetComponent<Canvas> ().enabled = true;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				//charaterstatus.isPaused = true;
			}
		}

	}

	public void Remove(Drag drag)
	{
		Item it = drag.item;
		GameObject newObj = Instantiate<GameObject> (Resources.Load<GameObject> (it.PrefabPath));
		newObj.transform.position = transform.position + transform.forward + transform.up;
		newObj.GetComponent<Item> ().count = drag.item.count;
		Destroy (drag.gameObject);
		item.Remove (it);
	}

	public void NoSpawnRemove(Drag drag)
	{
		Item it = drag.item;
		Destroy (drag.gameObject);
		item.Remove (it);
	}

	public void PickUpStart()
	{
		WeaponAvailable = false;
		charaterik.lh_Weight -= Time.deltaTime * 30;
		charaterik.lh_Weight = Mathf.Clamp (charaterik.lh_Weight, 0, 1);
	}


	public void PickUpEnd()
	{
		if (Weaponcount == 1 || Weaponcount == 2) {
			WeaponAvailable = true;
		}
	}

	public void ThrowStart()
	{
		charaterik.rh_Weight -= Time.deltaTime * 30;
		charaterik.rh_Weight = Mathf.Clamp (charaterik.lh_Weight, 0, 1);
	}

	public void ThrowEnd()
	{
		anim.SetBool ("IsThrowing", false);
		if (Weaponcount == 1 || Weaponcount == 2) {
			WeaponAvailable = true;
		}
	}


	public void ReloadStart()
	{
		WeaponAvailable = false;
	}

	public void ReloadShell()
	{
		activeWeapon.AddClips();
	}

	public void ReloadEnd()
	{
		WeaponAvailable = true;
		anim.SetBool("IsReloading",false);
		int addAmmo = activeWeapon.MaxAmmo;
		addAmmo -= activeWeapon.Ammo;

		for (int i = 0; i < OtherCell.childCount; i++) {
			if (OtherCell.GetChild(i).GetComponent<Drag> ().item.nameItem == activeWeapon.ammoType) {

				if (OtherCell.GetChild(i).GetComponent<Drag> ().item.count - addAmmo > 0) {
					OtherCell.GetChild(i).GetComponent<Drag> ().item.count -= addAmmo;
					activeWeapon.Ammo += addAmmo;
				} else {
					activeWeapon.Ammo += activeWeapon.BpAmmo;
					OtherCell.GetChild(i).GetComponent<Drag> ().item.count = 0;
					activeWeapon.BpAmmo = 0;
					NoSpawnRemove (OtherCell.GetChild(i).GetComponent<Drag> ());
					for(int x = 0 ; x < item.Count ; x++)
					{
						if(item[x].nameItem == OtherCell.GetChild(i).GetComponent<Drag> ().item.nameItem)
						{
							item.Remove(item[x]);
						}
					}
				}

				OtherCell.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = OtherCell.GetChild(i).GetComponent<Drag> ().item.count.ToString();
			} 
		}
		/*
		foreach (Transform child in parentCell) {
			if (child.GetComponent<Drag> ().item.nameItem == activeWeapon.ammoType) {

				if (child.GetComponent<Drag> ().item.count - addAmmo > 0) {
					child.GetComponent<Drag> ().item.count -= addAmmo;
					activeWeapon.Ammo += addAmmo;
				} else {
					activeWeapon.Ammo += activeWeapon.BpAmmo;
					child.GetComponent<Drag> ().item.count = 0;
					activeWeapon.BpAmmo = 0;
					NoSpawnRemove (child.GetComponent<Drag> ());
				}

				child.transform.GetChild(1).GetComponent<Text>().text = child.GetComponent<Drag> ().item.count.ToString();
			} 
		}
*/
		charaterstatus.isReloading = false;
	}

	public void ResetUiScale(GameObject gameobject)
	{
		gameobject.transform.localPosition = new Vector3 (gameobject.transform.localPosition.x, gameobject.transform.localPosition.y, 0);
		gameobject.transform.localScale = new Vector3 (1, 1, 1);
		gameobject.transform.localRotation = Quaternion.Euler(0, 0, 0);
	}
}
