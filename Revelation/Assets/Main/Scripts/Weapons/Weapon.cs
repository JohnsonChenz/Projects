using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {

	public WeaponProperties WeaponProperties;
	public Transform shotPoint;
	public Transform targetLook;

	public GameObject Camera;
	//public GameObject decal;
	public Animator anim;

	public Transform MainCharacter;
	public GameObject Explodebullet;
	public GameObject bullet;
	public GameObject FlameBullet;
	public GameObject GravityBullet;


	public Transform GunDistanceObject;

	public Transform LeftHandTarget;

	public float WeaponDistance;

	public GameObject MuzzleFlash;
	AudioSource audioSource;

	public GameObject WeaponSelf;
	//public GameObject WeaponBodySelf;
	public bool isEquipped;
	public bool isUsing = false;
	public bool isOnBack;

	public AudioClip shootClip;
	public AudioClip Reload;

	public CharaterStatus charaterStatus;

	//public bool opportunityToAim;
	public float distance;

	public Interact interact;

	public GameObject shell;
	public Transform shellPosition;

	public GameObject clips;
	public Transform clipsPosition;

	public CharaterInventory charaterinventory;


	public bool AbleToShoot;

	public bool isExplodeWeapon;
	public bool isFlameWeapon;
	public bool isSnipeWeapon;
	public bool isShotGunWeapon;
	public bool isGravityWeapon;

	public float damage;
    public float recoil;
    public float Maxspread;
    public float Minspread;
    float gunspread;
    public float RateOfFire;
	public float ScopeDistance;
	public int MaxAmmo;
	public int Ammo;
	public int BpAmmo = 0;
	public int MaxBpAmmo;
	public string ammoType;
	public string ammoTypeText;

    public bool IsAimingDebug;

    public Sprite Crosshair;
	public GameObject Scope;

    public CameraHander CameraRecoil;
    public Crosshair CrosshairSpread;

	public Transform PutBackTrans;

	public GameObject CrosshairManager;

	public Color BulletColor;

	void Start()
	{
		AbleToShoot = true;
		CrosshairManager = GameObject.Find ("CrosshairManager").gameObject;
		//WeaponSelf = GetComponent<GameObject>();
		audioSource = GetComponent<AudioSource> ();
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		anim = MainCharacter.GetComponent<Animator> ();
		charaterinventory = MainCharacter.GetComponent<CharaterInventory> ();
		isUsing = false;
		Invoke ("checkAmmo", 1);
	}

	void OnEnable()
	{
		AbleToShoot = true;
		CrosshairManager = GameObject.Find ("CrosshairManager").gameObject;
		//WeaponSelf = GetComponent<GameObject>();
		audioSource = GetComponent<AudioSource> ();
		MainCharacter = GameObject.FindGameObjectWithTag ("MainCharater").transform;
		anim = MainCharacter.GetComponent<Animator> ();
		charaterinventory = MainCharacter.GetComponent<CharaterInventory> ();
		isUsing = false;
		Invoke ("checkAmmo", 1);
	}

	void OnDisable()
	{
		CancelInvoke ("checkAmmo");
	}
	// Update is called once per frame
	void Update () {



		if (charaterStatus.isDeath || !WeaponSelf.activeInHierarchy || !isEquipped || !isUsing) {
			return;
		}


        if(gunspread > Maxspread)
        {
            gunspread = Maxspread;
        }

        if (gunspread > Minspread)
        {
            gunspread -= Time.deltaTime * Maxspread;
        }
        else
        {
            gunspread = Minspread;
        }

		Vector3 origin = shotPoint.position;
		Vector3 dir = targetLook.position;

		shotPoint.LookAt (targetLook);

		//RaycastHit hit;
		Debug.DrawLine (origin, dir, Color.black);
		//Debug.DrawLine (Camera.transform.position, dir, Color.black);


		//RayCastAiming ();


		if (anim.GetBool ("IsReloading")) {
			charaterStatus.isReloading = true;
		} else {
			charaterStatus.isReloading = false;
		}

		if (IsAimingDebug) {
		    charaterStatus.isAiming = true;
		}



		if ((Input.GetMouseButton (1) && !charaterinventory.isInventoryActive && !charaterStatus.isReloading && !charaterStatus.isSwitching && !charaterStatus.isDoAction) || IsAimingDebug) {
			if (isSnipeWeapon) {
				if (!Scope.activeInHierarchy) {
					CrosshairManager.SetActive (false);
					Scope.SetActive (true);
				}
				if (CameraRecoil.camTrans.GetComponent<Camera> ().fieldOfView != ScopeDistance) {
					CameraRecoil.camTrans.GetComponent<Camera> ().fieldOfView = ScopeDistance;
				}
			}
			charaterStatus.isAiming = true;
		} 
		else if(Input.GetMouseButton (0) && !charaterinventory.isInventoryActive && !charaterStatus.isReloading && !charaterStatus.isSwitching && !charaterStatus.isDoAction)
		{
			charaterStatus.isAiming = true;
		}
		else 
		{
			if (isSnipeWeapon) {
				if (Scope.activeInHierarchy) {
					CrosshairManager.SetActive (true);
					Scope.SetActive (false);
				}
			}
			if (CameraRecoil.camTrans.GetComponent<Camera> ().fieldOfView != 60) {
				CameraRecoil.camTrans.GetComponent<Camera> ().fieldOfView = 60;
			}
				
			charaterStatus.isAiming = false;
		}

		//if (!debugAiming)
		//charaterStatus.isAiming = Input.GetMouseButton (1);
		//else
		//charaterStatus.isAiming = isAiming;
		AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(1);

		if (Input.GetMouseButton (0) && !charaterStatus.isSwitching && !charaterStatus.isReloading && charaterStatus.isAiming && AbleToShoot && !anim.GetBool("IsReloading") && stateinfo.IsName("Gun")) {
			if (Ammo != 0) {
				Ammo--;
				AbleToShoot = false;
				charaterStatus.isFireing = true;
                anim.CrossFadeInFixedTime("Fire", 0.01f);
                Shoot ();
			} 
			else if(Ammo == 0 && BpAmmo != 0)
			{
				charaterStatus.isAiming = false;
				charaterStatus.isReloading = true;
				audioSource.PlayOneShot (Reload);
				anim.SetBool ("IsReloading", true);
			}
		}


			
		if (Input.GetKeyDown (KeyCode.R) && !charaterStatus.isSwitching && !charaterStatus.isReloading && !charaterinventory.isInventoryActive) {
			if (Ammo < MaxAmmo && BpAmmo > 0) {
				charaterStatus.isAiming = false;
				charaterStatus.isReloading = true;
				anim.SetBool ("IsReloading", true);
				audioSource.PlayOneShot (Reload);
			}
		}



	}

	public void Shoot()
	{
        charaterinventory.charaterik.r_Hand.gameObject.GetComponent<Animator>().CrossFadeInFixedTime("Shot", 0.01f);
        anim.SetBool("Fire", false);
		AddShell ();

        CrosshairSpread.ShotCrosshairSpread(recoil);

        CameraRecoil.Recoil(recoil);

		gunspread += (Maxspread / 5);
       

		if (isExplodeWeapon) {
			GameObject.Find ("BulletPool").GetComponent<ExplodeBulletPool> ().SpawnExplodeBullet (shotPoint.position, shotPoint.rotation);

		} else if (isFlameWeapon) {
			GameObject.Find ("BulletPool").GetComponent<FlameBulletPool> ().SpawnFlameBullet (shotPoint.position, shotPoint.rotation, transform);

		} else if (isGravityWeapon) {
			GameObject bullet = Instantiate (GravityBullet, shotPoint.position, shotPoint.rotation);
		} else if (isShotGunWeapon) {
			for(int i = 0 ; i < 5 ; i++)
			{
				if (gunspread > Maxspread / 4 && gunspread!=0) {
					shotPoint.Rotate (shotPoint.rotation.x + Random.Range (-gunspread, gunspread), shotPoint.rotation.y + Random.Range (-gunspread, gunspread), shotPoint.rotation.z + Random.Range (-gunspread, gunspread));
				}
				GameObject.Find ("BulletPool").GetComponent<BulletPool> ().SpawnBullet (shotPoint.position, shotPoint.rotation, damage, BulletColor);
			}
		}
		else if (!isExplodeWeapon && !isFlameWeapon && !isGravityWeapon && !isShotGunWeapon) 
		{
			if (gunspread > Maxspread / 4 && gunspread!=0) {
				shotPoint.Rotate (shotPoint.rotation.x + Random.Range (-gunspread, gunspread), shotPoint.rotation.y + Random.Range (-gunspread, gunspread), shotPoint.rotation.z + Random.Range (-gunspread, gunspread));
			}
			GameObject.Find ("BulletPool").GetComponent<BulletPool> ().SpawnBullet (shotPoint.position, shotPoint.rotation, damage, BulletColor);

            
		} 



		Camera.GetComponent<CameraShake> ().shakeMagnitude = 0.02f;
		Camera.GetComponent<CameraShake> ().shakeTime = 0.1f;
		Camera.GetComponent<CameraShake> ().ShakeIt ();

        Invoke ("EnableShoot", RateOfFire);
		Invoke ("Recoil", 0.05f);
		audioSource.PlayOneShot (shootClip);
		//muzzleFlash.Play ();
		MuzzleFlash.SetActive (false);
		MuzzleFlash.SetActive (true);
	}

	public void Recoil()
	{
        charaterinventory.charaterik.r_Hand.gameObject.GetComponent<Animator>().SetBool("Shot", false);
        charaterStatus.isFireing = false;
   
    }

	public void EnableShoot()
	{
		AbleToShoot = true;
	}

	public void AddShell()
	{
		GameObject newShell = Instantiate (shell);
		newShell.transform.position = shellPosition.position;

		Quaternion rot = shellPosition.rotation;
		newShell.transform.rotation = rot;

		newShell.transform.parent = null;
		newShell.GetComponent<Rigidbody> ().AddForce (-newShell.transform.forward * Random.Range (80, 120));
		Destroy (newShell, 3);



	}

	public void AddClips()
	{

		GameObject newClips = Instantiate (clips);
		newClips.transform.position = clipsPosition.position;

		Quaternion rot = clipsPosition.rotation;
		newClips.transform.rotation = rot;

		newClips.transform.parent = null;
		newClips.GetComponent<Rigidbody> ().AddForce (newClips.transform.forward);
		Destroy (newClips, 3);



	}

	void checkAmmo()
	{
		Invoke ("checkAmmo", 1);
		for (int i = 0; i < charaterinventory.OtherCell.childCount; i++) {
			
			if (charaterinventory.OtherCell.GetChild (i).GetComponent<Drag> ().item.nameItem == ammoType) {
				BpAmmo = charaterinventory.OtherCell.GetChild (i).GetComponent<Drag> ().item.count;
				//ammoTypeText = charaterinventory.parentCell.GetChild (i).GetComponent<Drag> ().item.InventoryName;
				//Debug.Log ("有彈藥");
				return;
			}
			if (charaterinventory.OtherCell.GetChild (i).GetComponent<Drag> ().item.nameItem != ammoType) {
				BpAmmo = 0;
				//Debug.Log ("沒彈藥");
			}

		}

		if (charaterinventory.OtherCell.childCount == 0) {
			BpAmmo = 0;
			//Debug.Log ("沒彈藥");
			}
        
		/*
		foreach (Transform child in charaterinventory.parentCell) {
			if (child.GetComponent<Drag> ().item.nameItem == ammoType) {
				BpAmmo = child.GetComponent<Drag> ().item.count;
				ammoTypeText = child.GetComponent<Drag> ().item.InventoryName;
				//Debug.Log ("有彈藥");
				return;
			}
			if (child.GetComponent<Drag> ().item.nameItem != ammoType) {
				BpAmmo = 0;
				//Debug.Log ("沒彈藥");
			}
		}
        */
	}

	public void RayCastAiming()
	{
		//Debug.DrawLine (transform.position + transform.up, targetLook.position, Color.green);

		distance = Vector3.Distance (GunDistanceObject.position, targetLook.position);



		if (distance > WeaponDistance) {
			//opportunityToAim = true;

		} 
		else 
		{
			//opportunityToAim = false;
		}
	}
}
