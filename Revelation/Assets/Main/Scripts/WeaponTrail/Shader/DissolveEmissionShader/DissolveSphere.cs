using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSphere : MonoBehaviour {

    Material mat;

	float dissolveOverTime=-1;



    private void Start() {
        mat = GetComponent<Renderer>().material;
    }

    private void Update() {
       // mat.SetFloat("_DissolveAmount", Mathf.Sin(Time.time) / 2 + 0.5f);



			dissolveOverTime += Time.deltaTime * 0.75f;
			mat.SetFloat ("_DissolveAmount", dissolveOverTime);



    }
}