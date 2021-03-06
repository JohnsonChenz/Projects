using UnityEngine;
using System.Collections;
using HighlightingSystem;

public class HighlighterController : MonoBehaviour
{
	public bool seeThrough = true;
	protected bool _seeThrough = true;

	public Color color;
	public Color color2;
	public bool On;
	public bool flash;
	public float flashfreq;
	protected Highlighter h;

	// 
	protected void Awake()
	{
		h = GetComponent<Highlighter>();
		if (h == null) { h = gameObject.AddComponent<Highlighter>(); }
	}

	// 
	void OnEnable()
	{
		if (seeThrough) { h.SeeThroughOn(); }
		else { h.SeeThroughOff(); }
	}

	// 
	protected void Start() 
	{
		On = false;
	}

	// 
	protected void Update()
	{
		if (_seeThrough != seeThrough)
		{
			_seeThrough = seeThrough;
			if (_seeThrough) { h.SeeThroughOn(); }
			else { h.SeeThroughOff(); }
		}

		if (On) {
			//h.On (color);
			if (flash) {
				h.FlashingOn (color2, color, flashfreq);
			} else {
				h.On (color);
			}
			//h.FlashingOn (0.5f);
		} 
		else
		{
				h.FlashingOff ();
		}
	}
		
}