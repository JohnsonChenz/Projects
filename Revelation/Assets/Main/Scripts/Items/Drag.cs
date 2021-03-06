using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {

	public Transform canvas;
	public Transform old;
	public CharaterInventory charaterinventory;
	public Item item;
	public string typeList;

	private Transform myTransform;
	private RectTransform myRectTransform;

	public bool isDragable;


	// Use this for initialization
	void Start () {

		charaterinventory = GameObject.FindGameObjectWithTag ("MainCharater").transform.GetComponent<CharaterInventory> ();
		if (typeList == "") {
			typeList = "Ground";
		}
		canvas = GameObject.Find ("CanvasInventory").transform;

		myRectTransform = this.transform as RectTransform;

	}

	void OnEnable () {

		charaterinventory = GameObject.FindGameObjectWithTag ("MainCharater").transform.GetComponent<CharaterInventory> ();
		if (typeList == "") {
			typeList = "Ground";
		}
		canvas = GameObject.Find ("CanvasInventory").transform;

		myRectTransform = this.transform as RectTransform;

	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!charaterinventory.isInventoryActive || !isDragable)
			return;
		old = transform.parent;
		transform.SetParent (canvas);
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
		charaterinventory.isDragging = true;
	}

	public void OnDrag(PointerEventData eventData)
	{

		if (!charaterinventory.isInventoryActive || !isDragable)
			return;
		
		transform.SetParent (canvas);
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(myRectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
		{
			myRectTransform.position = globalMousePos;
		}
		charaterinventory.isDragging = true;
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		if (!charaterinventory.isInventoryActive || !isDragable)
			return;

		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		charaterinventory.isDragging = false;

		if (transform.parent == canvas)
			transform.SetParent (old);
		    transform.transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, 0);
		    transform.transform.localScale = new Vector3 (1, 1, 1);
		    transform.transform.localRotation = Quaternion.Euler(0, 0, 0);
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right) {
			if (typeList == "Inventory" && item.typeItem != "Use W" && item.typeItem != "Use W2" && item.typeItem != "Use W3") {
				//charaterinventory.Remove (this);
				charaterinventory.UseItem (this, false);
				print ("pressed");
			} else if (typeList == "Inventory" && (item.typeItem == "Use W" || item.typeItem == "Use W2" || item.typeItem == "Use W3")) {
				charaterinventory.UnequipItem (this);
			} else if (typeList == "Craft") {
				charaterinventory.UnequipItem (this);
			}
			else if (typeList == "Ground" && item.typeItem == "Other") 
			{

				//charaterinventory.TakeItem (this);
			}
			else if (typeList == "Ground" && item.typeItem == "First Weapon") 
			{

				//charaterinventory.TakeWeapon (this);
			}

		}
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.localRotation = Quaternion.Euler(0, 0, 0);



        //charaterinventory.Remove (this);

    }

	public void Delete()
	{
		transform.SetParent (transform);
		item.typeItem = "";

		charaterinventory.Remove (this);
	}

	public void Use()
	{
		charaterinventory.UseItem (this, false);
	}
}
