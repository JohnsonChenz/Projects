using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destroy : MonoBehaviour {

    public Slider healthbar;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (healthbar.value <= 0)
        {
            Destroy(gameObject);
        }
    }
}
