using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXControllerV3D : MonoBehaviour
{

    public AudioSource loopingSFX;
    public GameObject[] waveSfxPrefabs;

    private float globalProgress;

	public bool canFire = false;
	public bool onFire = false;
	public bool canFire2 = false;

	//public LaserAttack LaserAttack;

    public void SetGlobalProgress(float gp)
    {
        globalProgress = gp;
    }

    void Update()
    {
		if (Input.GetKeyDown ("n")) {
			canFire = true;
			onFire = true;
			canFire2 = true;
		}else if (Input.GetKeyUp ("n")) {
			canFire = false;
			onFire = false;
		}


		//博士雷射


		//博士雷射^^^^^^^^^^


		if (canFire == true)
        {
            Instantiate(waveSfxPrefabs[Random.Range(0, waveSfxPrefabs.Length)], transform.position, transform.rotation);
			canFire = false;
        }

        loopingSFX.volume = globalProgress;
    }
}
