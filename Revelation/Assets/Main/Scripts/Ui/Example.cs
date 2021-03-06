using UnityEngine;
using UnityEngine.EventSystems; // 1
using UnityEngine.UI; // 1
using System.Collections;
using System.Collections.Generic;

public class Example : MonoBehaviour // 2
, IPointerEnterHandler
, IPointerExitHandler
, IBeginDragHandler
, IDragHandler
, IEndDragHandler
, IPointerClickHandler
// ... And many more available!
{
	Image sprite;
	public Color OverColor;
	public Color DragColor;
	public Color OriginColor;
	public int Child;
	//public int InformationChild;
	public int InformationImageTitleChild;
	public int InformationImageInfoChild;
	public CharaterInventory charaterinventory;
	public bool off = false;
	string Title;
	string Info;
	public Transform CanvasInventory;
	public Transform Image;
	public Image ItemIcon;
	public Transform[] bars;
	public Sprite[] ColorizedBorder;
	bool IsEntered;

	//public Transform ImageBackground;
	Vector3 OriginalPos;
	public float AdjustImagePosTop;
	public float AdjustImagePosBottom;

	public float PosX;
	public float PosY;
	void Awake()
	{
		CanvasInventory = GameObject.Find ("BagItems").transform;
		sprite = transform.GetChild(Child).GetComponent<Image>();
		//OriginColor = transform.GetChild (Child).GetComponent<Image> ().color;
		charaterinventory = GameObject.FindGameObjectWithTag ("MainCharater").transform.GetComponent<CharaterInventory> ();

	}

	void Update()
	{
		if (IsEntered) {
			//Image.position = ImageBackground.position;
		}

	}


	void Start()
	{

		OriginalPos = Image.localPosition;
		Title = transform.GetComponent<Drag> ().item.InventoryName;
		Info = transform.GetComponent<Drag> ().item.InventoryInfo;
		transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite> (transform.GetComponent<Drag> ().item.SpritePath);
		ItemIcon.sprite = Resources.Load<Sprite> (transform.GetComponent<Drag> ().item.SpritePath + "white");
		Image.transform.GetChild (InformationImageTitleChild).GetComponent<Text> ().text = Title;
		Image.transform.GetChild (InformationImageInfoChild).GetComponent<Text> ().text = Info;
		Invoke ("AmmoAndBpammo",1);	
		if (transform.GetComponent<Drag> ().item.HasBar) {
			foreach (Transform child in bars) {
				child.transform.gameObject.SetActive (true);
			}
			bars[0].GetComponent<Slider>().value = transform.GetComponent<Drag> ().item.damage;
			bars[1].GetComponent<Slider>().value = transform.GetComponent<Drag> ().item.ammo;
			bars[2].GetComponent<Slider>().value = transform.GetComponent<Drag> ().item.recoil;
			bars[3].GetComponent<Slider>().value = transform.GetComponent<Drag> ().item.accuracy;
			bars[4].GetComponent<Slider>().value = transform.GetComponent<Drag> ().item.rateoffire;
		} 
		else
		{
			foreach (Transform child in bars) {
				child.transform.gameObject.SetActive (false);
			}

		}

	}

	void AmmoAndBpammo()
	{
		Invoke ("AmmoAndBpammo",1);	
		if (transform.GetComponent<Drag> ().item.WeaponOnHand) {
			if (transform.GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Weapon> ()) {
				transform.GetChild (3).GetComponent<Text> ().text = transform.GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Weapon> ().Ammo.ToString () + " /";
				transform.GetChild (4).GetComponent<Text> ().text = transform.GetComponent<Drag> ().item.WeaponOnHand.GetComponent<Weapon> ().BpAmmo.ToString ();
			}
		}
	}
	public void OnPointerEnter(PointerEventData eventData)
	{

		//sprite.color = OverColor;
		if (GetComponent<Drag> ().item.nameItem == "M8Upgraded") {
			Image.GetComponent<Image>().sprite = ColorizedBorder [0];
		}
		if (GetComponent<Drag> ().item.nameItem == "RevolverUpgraded") {
			Image.GetComponent<Image>().sprite = ColorizedBorder [1];
		}
		if (GetComponent<Drag> ().item.nameItem == "KatanaUpgraded") {
			Image.GetComponent<Image>().sprite = ColorizedBorder [2];
		}
		if (GetComponent<Drag> ().item.nameItem == "SR1Upgrade") {
			Image.GetComponent<Image>().sprite = ColorizedBorder [3];
		}

		Image.SetParent (CanvasInventory);
		//ImageBackground.gameObject.SetActive (true);
		if (transform.GetComponent<Drag> ().item.HasBar) {
			Image.gameObject.SetActive (true);
		}
		transform.GetChild (1).GetComponent<Text> ().color = Color.white;
		transform.GetChild (2).GetComponent<Text> ().color = Color.white;
		transform.GetChild (3).GetComponent<Text> ().color = Color.white;
		transform.GetChild (4).GetComponent<Text> ().color = Color.white;
		Image.transform.localPosition = new Vector3 (PosX, Image.localPosition.y, Image.localPosition.z);


		IsEntered = true;

		if (Image.transform.localPosition.y < -415) {
			Image.transform.localPosition = new Vector3 (Image.localPosition.x, AdjustImagePosBottom, Image.localPosition.z);
		}

		if (Image.transform.localPosition.y > 344) {
			Image.transform.localPosition = new Vector3 (Image.localPosition.x, AdjustImagePosTop, Image.localPosition.z);
		}
		this.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (transform.GetComponent<Drag>().item.SpritePath + "white");

	}

	public void OnPointerExit(PointerEventData eventData)
	{
		transform.GetChild (1).GetComponent<Text> ().color = Color.black;
		transform.GetChild (2).GetComponent<Text> ().color = Color.black;	
		transform.GetChild (3).GetComponent<Text> ().color = Color.black;
		transform.GetChild (4).GetComponent<Text> ().color = Color.black;
		IsEntered = false;
		//sprite.color = OriginColor;
		Image.SetParent (this.transform);
		//ImageBackground.gameObject.SetActive (false);
		Image.gameObject.SetActive (false);
		Image.localPosition = OriginalPos;
		this.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (transform.GetComponent<Drag>().item.SpritePath);
	}


	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right) {
			Image.SetParent (this.transform);
			Image.gameObject.SetActive (false);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		//sprite.color = DragColor;
	}

	void OnDestroy()
	{
		IsEntered = false;
		//sprite.color = OriginColor;
		Image.SetParent (this.transform);
		Image.gameObject.SetActive (false);
	}

	public void OnDrag(PointerEventData eventData)
	{

		//sprite.color = DragColor;
	}
	public void OnEndDrag(PointerEventData eventData)
	{

		//sprite.color = OriginColor;
	}

}