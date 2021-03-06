using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Charater/status")]
public class CharaterStatus : ScriptableObject {

		public bool isAiming;
		public bool isSprint;
		public bool isGround;
	    public bool isSwitching;
	    public bool isReloading;
	    public bool isLocking;
	    public bool isDeath;
	    public bool isPickuping;
	    public bool isThrowingGrenade;
	    public bool isFireing;
	    public bool isDoAction;
	    //public bool isPaused;
	    public bool isHealing;
}
