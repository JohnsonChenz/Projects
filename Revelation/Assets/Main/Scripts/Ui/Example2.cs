using UnityEngine;
using UnityEngine.EventSystems; // 1
using UnityEngine.UI; // 1
using System.Collections;
using System.Collections.Generic;
public class Example2 : MonoBehaviour , IPointerExitHandler{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.gameObject.SetActive (false);
	}

}
