using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public string player;

    void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == player)
        {
            SceneManager.LoadScene(2);
        }


    }


}
