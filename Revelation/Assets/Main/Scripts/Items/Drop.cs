using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI; 

public class Drop : MonoBehaviour, IDropHandler {

	CharaterInventory charaterinventory;
	public Interact interact;

	public Drag drag;
	public string typeCell;

	public void OnDrop(PointerEventData eventData)
	{
		drag = eventData.pointerDrag.GetComponent<Drag> ();

		if (!drag.isDragable)
			return;



		if (drag.item.typeItem == "First Weapon" && typeCell == "First Weapon") {

			charaterinventory.UseItem (drag, false);


		} 
		else if (drag.item.typeItem == "Use W" && typeCell == "Inventory") 
		{

			charaterinventory.UnequipItem (drag);



		}


		if (drag.item.typeItem == "Second Weapon" && typeCell == "Second Weapon") {

			charaterinventory.UseItem (drag, false);


		} 
		else if (drag.item.typeItem == "Use W2" && typeCell == "Inventory") 
		{
			charaterinventory.UnequipItem (drag);




		}

		if (drag.item.typeItem == "Melee Weapon" && typeCell == "Melee Weapon") {


			charaterinventory.UseItem (drag, false);


		} 
		else if (drag.item.typeItem == "Use W3" && typeCell == "Inventory") 
		{

			charaterinventory.UnequipItem (drag);


		}


		if (drag.item.typeItem == "First Weapon" && typeCell == "CraftGroundWep" && charaterinventory.CraftGround.childCount == 0) {
			drag.transform.SetParent (transform);

			drag.typeList = "Inventory";
			drag.item.typeItem = "Craft1";

		} 
		else if (drag.item.typeItem == "Craft1" && typeCell == "Inventory") 
		{
			drag.transform.SetParent (transform.GetChild (0));


			drag.typeList = "Inventory";
			drag.item.typeItem = "First Weapon"; //

		}

		if (drag.item.typeItem == "Rock" && typeCell == "CraftGround2" && charaterinventory.CraftGroundStone.childCount == 0) {
			drag.transform.SetParent (transform);

			drag.typeList = "Inventory";
			drag.item.typeItem = "Craft2";

		} 
		else if (drag.item.typeItem == "Craft2" && typeCell == "Inventory") 
		{
			drag.transform.SetParent (transform.GetChild (0));



			drag.typeList = "Inventory";
			drag.item.typeItem = "Rock"; // 

		}

		if (drag.item.typeItem == "M9" && typeCell == "Inventory") 
		{
			drag.transform.SetParent (transform.GetChild (0));

			drag.typeList = "Inventory";
			drag.item.typeItem = "Second Weapon"; //

		}


		if (typeCell == "DeleteGround" && drag.item.typeItem != "Use W" && drag.item.typeItem != "Use W2" && drag.item.typeItem != "Use W3") {

			drag.transform.SetParent (transform);
			drag.item.typeItem = "";

			charaterinventory.Remove (drag);
		}

		drag.transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, 0);
		drag.transform.localScale = new Vector3 (1, 1, 1);
		drag.transform.localRotation = Quaternion.Euler(0, 0, 0);
		charaterinventory.itemOnTheGround.Clear ();
		charaterinventory.isDragging = false;
	}

	// Use this for initialization
	void Start () {
		charaterinventory = GameObject.FindGameObjectWithTag ("MainCharater").GetComponent<CharaterInventory> ();
		interact = GameObject.Find ("InteractPoint").GetComponent<Interact> ();
	}
		
}
